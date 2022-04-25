using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.Results;
using Abstractions.Security;
using DemoApplication.Database;
using DemoApplication.Domain;
using DemoApplication.Enums;
using DemoApplication.Factory;
using DemoApplication.Models;
using DemoApplication.Validator;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Framework;

public sealed class UserService : IUserService
{
    #region Private Members
    private readonly IAuthService _authService;
    private readonly ICryptographyService _cryptographyService;
    private readonly IAuthRepository _authRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IAuditTrailService _auditTrailService;
    private readonly INotificationService _notificationService;
    #endregion

    #region Constructor
    public UserService
    (
        IAuthService authService,
        ICryptographyService cryptographyService,
        IAuthRepository authRepository,
        IUnitOfWork unitOfWork,
        IUserRepository userRepository,
        IAuditTrailService auditTrailService,
        INotificationService notificationService
    )
    {
        _authService = authService;
        _cryptographyService = cryptographyService;
        _authRepository = authRepository;
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _auditTrailService = auditTrailService;
        _notificationService = notificationService;
    }
    #endregion


    #region Create
    public async Task<IResult<Guid>> AddAsync(UserModel model, Guid activeUserId)
    {
        var validation = await new AddUserModelValidator().ValidateAsync(model);

        if (validation.Failed)
            return await Result<Guid>.FailAsync(validation.Message);

        model.Email = await _cryptographyService.Encrypt(model.Email.ToLower());

        var authResult = await _authService.AddAsync(model.Auth);

        if (authResult.Failed)
            return await Result<Guid>.FailAsync(authResult.Message);

        var user = UserFactory.Create(model, authResult.Data!);

        await _userRepository.AddAsync(user);

        await _unitOfWork.SaveChangesAsync();

        var auditResult = await _auditTrailService.AddAsync(model, activeUserId);

        model.Email = await _cryptographyService.Decrypt(model.Email); //Decrypt after logging

        if (auditResult.Failed)
        {
            await DeleteAsync(user.Id, user.Id); //Do not create user if audit trail did not succeed
            //auth is deleted alongside a user because of 
            return Result<Guid>.Fail(auditResult.Message);
        }

        await _notificationService.AddAsync(user.Id, SeverityEnum.General, "New User Registered", RolesEnum.Supervisor, EntityEnum.User, user.Id.ToString());

        return await Result<Guid>.SuccessAsync(user.Id);

        //TODO Email user to let him know his login is working
    }
    #endregion

    #region Read
    public async Task<UserModel> GetAsync(Guid id, RolesEnum role)
    {
        var user = await _userRepository.GetByIdAsync(id) ?? await _userRepository.GetByAuthIdAsync(id);

        if (user == null)
            return new UserModel();

        user.Email = DecryptRoles.ShouldDecrypt(role) ? await _cryptographyService.Decrypt(user.Email) : "*******@*****.com";

        return user;
    }
    public async Task<IEnumerable<UserModel>> ListAsync(RolesEnum role)
    {
        var users = await _userRepository.Queryable.Select(UserExpression.Model!).ToListAsync();

        var shouldDecrypt = DecryptRoles.ShouldDecrypt(role);

        users.ForEach(async user
            => user.Email = shouldDecrypt
                ? await _cryptographyService.Decrypt(user.Email)
                : "*******@*****.com");

        return users;
    }
    #endregion

    #region Update
    public async Task<IResult> SuspendAsync(Guid id, Guid activeUserId)
    {
        var user = new User(id);

        var auditResult = await _auditTrailService.AddAsync(user, activeUserId);

        if (auditResult.Failed)
            return await Result.FailAsync(auditResult.Message);

        if (await _userRepository.GetIsActive(id))
            user.Inactivate();
        else
            user.Activate();

        await _userRepository.UpdateStatusAsync(user);

        await _unitOfWork.SaveChangesAsync();

        return await Result.SuccessAsync();
    }
    public async Task<IResult> UpdateAsync(UserModel model)
    {
        var validation = await new UpdateUserModelValidator().ValidateAsync(model);

        if (validation.Failed)
            return await Result.FailAsync(validation.Message);

        var oldDetails = await _userRepository.GetByIdAsync(model.Id);

        model.Email = await _cryptographyService.Encrypt(model.Email.ToLower());

        if (await _userRepository.AnyByEmailAsync(model.Email))
            return Result.Fail("Email not valid");

        var user = await _userRepository.GetAsync(model.Id);

        if (user is null)
            return await Result.FailAsync("User not found");

        var auditResult = await _auditTrailService.AddAsync(model, user!.Id); //a user can only update his own details

        if (auditResult.Failed)
            return Result<Guid>.Fail(auditResult.Message);

        user.ChangeFullName(model.Name, model.Surname);

        if (model.Email != oldDetails.Email)
            user.ChangeEmail(model.Email);

        await _userRepository.UpdateAsync(user.Id, user);

        await _unitOfWork.SaveChangesAsync();

        var auth = await _authRepository.GetByLoginAsync(oldDetails.Email);

        static AuthModel authToModel(Auth auth) => new()
        {
            Login = auth.Email,
            Password = auth.Password,
            Role = auth.Role
        };

        await _authService.ChangeLogin(authToModel(auth), model.Id, model.Email);

        await _notificationService.AddAsync(user.Id, SeverityEnum.General, "User Details Updated", RolesEnum.Supervisor, EntityEnum.User, user.Id.ToString());

        return await Result.SuccessAsync();
    }
    #endregion

    #region Delete
    public async Task<IResult> DeleteAsync(Guid id, Guid activeUserId)
    {
        var auditResult = await _auditTrailService.AddAsync(new User(id), activeUserId);

        if (auditResult.Failed)
            return await Result.FailAsync(auditResult.Message);

        var authId = await _userRepository.GetAuthIdByUserIdAsync(id);

        await _userRepository.DeleteAsync(id);

        await _authService.DeleteAsync(authId, activeUserId);

        await _unitOfWork.SaveChangesAsync();

        return await Result.SuccessAsync();
    }
    #endregion

}
