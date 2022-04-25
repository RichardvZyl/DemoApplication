using System;
using System.Threading.Tasks;
using Abstractions.Results;
using Abstractions.Security;
using DemoApplication.ClaimExceptions;
using DemoApplication.Database;
using DemoApplication.Domain;
using DemoApplication.Enums;
using DemoApplication.Factory;
using DemoApplication.Models;
using DemoApplication.Validator;

namespace DemoApplication.Framework;

public sealed class EntitlementService : IEntitlementService
{
    #region Private Members
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICryptographyService _cryptographyService;
    private readonly IEntitlementRepository _entitlementRepository;
    private readonly IAuditTrailService _auditTrailService;
    private readonly INotificationService _notificationService;
    private readonly IUserRepository _userRepository;
    private readonly IAuthRepository _authRepository;
    #endregion

    #region Constructor
    public EntitlementService
    (
        IUnitOfWork unitOfWork,
        ICryptographyService cryptographyService,
        IEntitlementRepository exceptionsRepository,
        IAuditTrailService auditTrailService,
        INotificationService notificationService,
        IUserRepository userRepository,
        IAuthRepository authRepository
    )
    {
        _unitOfWork = unitOfWork;
        _cryptographyService = cryptographyService;
        _entitlementRepository = exceptionsRepository;
        _auditTrailService = auditTrailService;
        _notificationService = notificationService;
        _userRepository = userRepository;
        _authRepository = authRepository;
    }
    #endregion

    #region Create
    public async Task<IResult> AddAsync(EntitlementModel model, string email, Guid activeUserId)
    {
        email = await _cryptographyService.Encrypt(email);

        model.UserId = await _userRepository.GetIdByEmail(email);

        var validation = await new AddEntitlementModelValidator().ValidateAsync(model);

        if (validation.Failed)
            return await Result.FailAsync(validation.Message);

        if (await _entitlementRepository.AnyByUserIdAsync(model.UserId))
            return await UpdateAsync(model, activeUserId);

        var entitlement = EntitlementFactory.Create(model);

        await _entitlementRepository.AddAsync(entitlement);

        await _unitOfWork.SaveChangesAsync();

        var auditResult = await _auditTrailService.AddAsync(model, activeUserId);

        if (auditResult.Failed)
        {
            await DeleteAsync(await _cryptographyService.Decrypt(email), activeUserId);
            return Result.Fail(auditResult.Message);
        }

        await _notificationService.AddAsync(activeUserId, SeverityEnum.Serious, "User Entitlement Changed", RolesEnum.Supervisor, EntityEnum.Entitlement, entitlement.Id.ToString());

        return await Result.SuccessAsync("Entitlement Exceptions Successfuly Added");
    }
    #endregion

    #region Read
    public async Task<EntitlementModel> GetByUserIdAsync(Guid id) 
        => await _entitlementRepository.GetModelByUserIdAsync(id);

    public async Task<EntitlementModel> GetByUserEmailAsync(string userEmail)
    {
        var userId = await _userRepository.GetIdByEmail(await _cryptographyService.Encrypt(userEmail));
        var entitlement = await _entitlementRepository.GetModelByUserIdAsync(userId);

        if (entitlement == null)
            return new EntitlementModel();

        return entitlement;
    }
    public async Task<IResult<string>> GetRoleByUserEmailAsync(string userEmail)
    {
        var role = await _authRepository.GetRoleByEmailAsync(await _cryptographyService.Encrypt(userEmail));

        return await Result<string>.SuccessAsync($"{(int)role} {role}", "Success");
    }

    public async Task<EntitlementModel> GetDefaultEntitlement(RolesEnum role)
        => await Task.FromResult(ClaimsSeedByRole.GetDefaultEntitlement(role));
    #endregion

    #region Update
    private async Task<IResult> UpdateAsync(EntitlementModel model, Guid activeUserId)
    {
        var entitlement = EntitlementFactory.Create(model);

        var auditResult = await _auditTrailService.AddAsync(model, activeUserId);

        if (auditResult.Failed)
            return Result.Fail(auditResult.Message);

        entitlement.Update(model);

        await _entitlementRepository.UpdateAsync(entitlement);

        await _unitOfWork.SaveChangesAsync();

        return await Result.SuccessAsync("Entitlement Exceptions Successfuly Updated");
    }
    #endregion

    #region Delete
    public async Task<IResult> DeleteAsync(string email, Guid activeUserId)
    {
        var userId = await _userRepository.GetIdByEmail(await _cryptographyService.Encrypt(email));
        var entitlement = await _entitlementRepository.GetModelByUserIdAsync(userId);

        var auditResult = await _auditTrailService.AddAsync(new Domain.Entitlement(entitlement.UserId), activeUserId);

        if (auditResult.Failed)
            return await Result.FailAsync(auditResult.Message);

        await _entitlementRepository.DeleteAsync(entitlement.UserId);

        await _unitOfWork.SaveChangesAsync();

        return await Result.SuccessAsync();
    }
    #endregion
}
