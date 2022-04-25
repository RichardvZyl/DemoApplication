using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Abstractions.Entitlement;
using Abstractions.Results;
using Abstractions.Security;
using DemoApplication.Database;
using DemoApplication.Domain;
using DemoApplication.Factory;
using DemoApplication.Models;
using DemoApplication.Validator;

namespace DemoApplication.Framework;

public sealed class AuthService : IAuthService
{
    #region Private Members
    private readonly IAuthRepository _authRepository;
    private readonly IHashService _hashService;
    private readonly ICryptographyService _cryptographyService;
    private readonly IJsonWebTokenService _jsonWebTokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEntitlementRepository _entitlementRepository;
    private readonly IEntitlementExceptionsRepository _entitlementExceptionsRepository;
    private readonly IAuditTrailService _auditTrailService;
    private readonly IUserRepository _userRepository;
    #endregion

    #region Constructor
    public AuthService
    (
        IAuthRepository authRepository,
        IHashService hashService,
        ICryptographyService cryptographyService,
        IJsonWebTokenService jsonWebTokenService,
        IUnitOfWork unitOfWork,
        IEntitlementRepository entitlementRepository,
        IEntitlementExceptionsRepository entitlementExceptionsRepository,
        IAuditTrailService auditTrailService,
        IUserRepository userRepository
    )
    {
        _authRepository = authRepository;
        _hashService = hashService;
        _cryptographyService = cryptographyService;
        _jsonWebTokenService = jsonWebTokenService;
        _unitOfWork = unitOfWork;
        _entitlementRepository = entitlementRepository;
        _entitlementExceptionsRepository = entitlementExceptionsRepository;
        _auditTrailService = auditTrailService;
        _userRepository = userRepository;
    }
    #endregion


    #region Create
    public async Task<IResult<Auth>> AddAsync(AuthModel model)
    {
        var validation = await new AuthModelValidator().ValidateAsync(model);

        if (validation.Failed)
            return await Result<Auth>.FailAsync(validation.Message);

        model.Login = await _cryptographyService.Encrypt(model.Login.ToLower());

        if (await _authRepository.AnyByLoginAsync(model.Login))
            return await Result<Auth>.FailAsync("Login already exists!");

        var auth = AuthFactory.Create(model);

        var password = _hashService.Create(auth.Password, auth.Salt);

        auth.ChangePassword(password);

        await _authRepository.AddAsync(auth);

        return await Result<Auth>.SuccessAsync(auth);
    }
    #endregion

    #region Update
    public async Task<IResult<Guid>> ChangeRole(RoleChangeModel? model, Guid activeUserId)
    {
        if (model is null or default(RoleChangeModel))
            return await Result<Guid>.FailAsync("Required values not present");

        model.Login = await _cryptographyService.Encrypt(model.Login);

        var auth = await _authRepository.GetByLoginAsync(model.Login);

        if (auth.Role == model.Role)
        {
            model.Login = await _cryptographyService.Decrypt(model.Login);
            return await Result<Guid>.SuccessAsync(auth.Id);
        }

        var auditResult = await _auditTrailService.AddAsync(model, activeUserId);

        if (auditResult.Failed)
            return await Result<Guid>.FailAsync(auditResult.Message);

        model.Login = await _cryptographyService.Decrypt(model.Login);

        auth.ChangeRole(model.Role);

        await _authRepository.UpdateRoleAsync(auth);

        await _unitOfWork.SaveChangesAsync();

        return await Result<Guid>.SuccessAsync(auth.Id);
    }

    public async Task<IResult<string>> ChangePassword(AuthModel model, Guid activeUserId)
    {
        var validation = await new AuthModelValidator().ValidateAsync(model);

        if (validation.Failed)
            return await Result<string>.FailAsync(validation.Message);

        model.Login = await _cryptographyService.Encrypt(model.Login);

        var auditResult = await _auditTrailService.AddAsync(model, activeUserId);

        if (auditResult.Failed)
            return await Result<string>.FailAsync(auditResult.Message);

        var auth = await _authRepository.GetByLoginAsync(model.Login);

        var password = _hashService.Create(auth.Password, auth.Salt);

        auth.ChangePassword(password);

        await _authRepository.UpdatePassword(auth);

        await _unitOfWork.SaveChangesAsync();

        return await Result<string>.SuccessAsync(auth.Email);
    }

    public async Task<IResult<string>> ChangeLogin(AuthModel model, Guid activeUserId, string newEmailEncrypted)
    {
        var validation = await new AuthModelValidator().ValidateAsync(model);

        if (validation.Failed)
            return await Result<string>.FailAsync(validation.Message);

        var auditResult = await _auditTrailService.AddAsync(model, activeUserId);

        if (auditResult.Failed)
            return await Result<string>.FailAsync(auditResult.Message);

        var auth = await _authRepository.GetByLoginAsync(model.Login);

        auth.ChangeLogin(newEmailEncrypted);

        await _authRepository.UpdateLogin(auth);

        await _unitOfWork.SaveChangesAsync();

        return await Result<string>.SuccessAsync(auth.Email);
    }
    #endregion

    #region Delete
    public async Task<IResult> DeleteAsync(Guid id, Guid activeUserId)
    {
        var auditResult = await _auditTrailService.AddAsync(new Auth(id), activeUserId);

        if (auditResult.Failed)
            return Result.Fail(auditResult.Message);

        await _authRepository.DeleteAsync(id);

        await _unitOfWork.SaveChangesAsync();

        return await Result.SuccessAsync();

    }
    #endregion


    #region SignIn
    public async Task<IResult<TokenModel>> SignInAsync(SignInModel model)
    {
        var validation = await new SignInModelValidator().ValidateAsync(model);

        if (validation.Failed)
            return await Result<TokenModel>.FailAsync(validation.Message);

        model.Login = await _cryptographyService.Encrypt(model.Login.ToLower());

        var auth = await _authRepository.GetByLoginAsync(model.Login);

        validation = await Validate(auth, model);

        return validation.Failed
            ? await Result<TokenModel>.FailAsync(validation.Message)
            : await CreateToken(auth);
    }

    private async Task<IResult<TokenModel>> CreateToken(Auth auth)
    {
        var entitlementModel = await _entitlementRepository.GetModelByUserIdAsync(auth.Id);
        var entitlementExceptionsModel = await _entitlementExceptionsRepository.GetModelByUserIdAsync(auth.Id);

        var claims = new List<Claim>();

        claims = SavedClaims<EntitlementModel>.ApplyClaims(claims, entitlementModel); // Add Saved Claims
        claims = ClaimExceptions<EntitlementExceptionsModel>.ApplyExceptions(claims, entitlementExceptionsModel);// Apply Claim Exceptions

        // Add Identity Claims
        claims.Add(new Claim("Role", auth.Role.ToString()));
        claims.Add(new Claim("Identity", auth.Id.ToString()));

        var token = _jsonWebTokenService.Encode(claims);

        var tokenModel = new TokenModel(token);

        return await Result<TokenModel>.SuccessAsync(tokenModel);
    }

    private async Task<IResult> Validate(Auth auth, SignInModel model)
    {
        var failResult = Result.FailAsync("Invalid login or password!");

        if (auth is null || auth.Id == default || model == default)
            return await failResult;

        var password = _hashService.Create(model.Password, auth.Salt);

        if (auth.Password != password)
            return await failResult;

        var userId = await _userRepository.GetIdByEmail(auth.Email);

        return !await _userRepository.GetIsActive(userId)
            ? await Result.FailAsync("User has been deactivated")
            : await Result.SuccessAsync();
    }
    #endregion
}
