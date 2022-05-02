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

public sealed class EntitlementExceptionsService : IEntitlementExceptionsService
{
    #region Private Members
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICryptographyService _cryptographyService;
    private readonly IEntitlementExceptionsRepository _exceptionsRepository;
    private readonly IAuditTrailService _auditTrailService;
    private readonly INotificationService _notificationService;
    private readonly IUserRepository _userRepository;
    private readonly IAuthRepository _authRepository;
    #endregion

    #region Constructor
    public EntitlementExceptionsService
    (
        IUnitOfWork unitOfWork,
        ICryptographyService cryptographyService,
        IEntitlementExceptionsRepository exceptionsRepository,
        IAuditTrailService auditTrailService,
        INotificationService notificationService,
        IUserRepository userRepository,
        IAuthRepository authRepository
    )
    {
        _unitOfWork = unitOfWork;
        _cryptographyService = cryptographyService;
        _exceptionsRepository = exceptionsRepository;
        _auditTrailService = auditTrailService;
        _notificationService = notificationService;
        _userRepository = userRepository;
        _authRepository = authRepository;
    }
    #endregion

    #region Create
    public async Task<IResult> AddAsync(EntitlementExceptionsModel model, string email, Guid activeUserId)
    {
        email = await _cryptographyService.Encrypt(email);

        model.UserId = await _userRepository.GetIdByEmail(email);

        var validation = await new AddEntitlementExceptionsModelValidator().ValidateAsync(model);

        if (validation.Failed)
            return await Result.FailAsync(validation.Message);

        if (await _exceptionsRepository.AnyByUserIdAsync(model.UserId))
            return await UpdateAsync(model, activeUserId);

        var entitlement = EntitlementFactory.Create(model);

        await _exceptionsRepository.AddAsync(entitlement);

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
    public async Task<EntitlementExceptionsModel> GetByUserIdAsync(Guid id)
    {
        var entitlement = await _exceptionsRepository.GetModelByUserIdAsync(id);

        return entitlement.ExpiresOn == null
            ? entitlement
            : DateTimeOffset.Compare((DateTimeOffset)entitlement.ExpiresOn, DateTimeOffset.UtcNow) > 0
                ? (new())
                : entitlement;
    }
    public async Task<EntitlementExceptionsModel> GetByUserEmailAsync(string userEmail)
    {
        var userId = await _userRepository.GetIdByEmail(await _cryptographyService.Encrypt(userEmail));
        var entitlement = await _exceptionsRepository.GetModelByUserIdAsync(userId);

        return entitlement == null
            ? new EntitlementExceptionsModel()
            : entitlement.ExpiresOn == null
                ? entitlement
                : DateTimeOffset.Compare((DateTimeOffset)entitlement.ExpiresOn, DateTimeOffset.UtcNow) > 0
                    ? new EntitlementExceptionsModel()
                    : entitlement;
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
    private async Task<IResult> UpdateAsync(EntitlementExceptionsModel model, Guid activeUserId)
    {
        var entitlement = EntitlementFactory.Create(model);

        var auditResult = await _auditTrailService.AddAsync(model, activeUserId);

        if (auditResult.Failed)
            return Result.Fail(auditResult.Message);

        entitlement.UpdateExceptions(model);

        await _exceptionsRepository.UpdateAsync(entitlement);

        await _unitOfWork.SaveChangesAsync();

        return await Result.SuccessAsync("Entitlement Exceptions Successfuly Updated");
    }
    #endregion

    #region Delete
    public async Task<IResult> DeleteAsync(string email, Guid activeUserId)
    {
        var userId = await _userRepository.GetIdByEmail(await _cryptographyService.Encrypt(email));
        var entitlement = await _exceptionsRepository.GetModelByUserIdAsync(userId);

        var auditResult = await _auditTrailService.AddAsync(new EntitlementExceptions(entitlement.UserId), activeUserId);

        if (auditResult.Failed)
            return await Result.FailAsync(auditResult.Message);

        await _exceptionsRepository.DeleteAsync(entitlement.UserId);

        await _unitOfWork.SaveChangesAsync();

        return await Result.SuccessAsync();
    }
    #endregion

    #region Validate
    public bool ModelHasExceptions(EntitlementExceptionsModel model)
    {//Check that the model is not blank as a blank model is useless
        var hasExceptions = false;
        foreach (var prop in model.GetType().GetProperties())
        {
            var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
            if (type == typeof(bool))
            {
                if (prop.GetValue(model) != null)
                {
                    hasExceptions = true;
                    break; //exit loop as it has been established that atleast one value is filled in
                }
            }
        }

        return hasExceptions;
    }
    #endregion
}
