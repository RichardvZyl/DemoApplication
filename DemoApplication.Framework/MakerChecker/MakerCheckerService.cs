using System.Text.Json;
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

public sealed class MakerCheckerService : IMakerCheckerService
{
    #region Private Members
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICryptographyService _cryptographyService;
    private readonly IMakerCheckerRepository _makerCheckerRepository;
    private readonly IAuditTrailService _auditTrailService; //for logging
    private readonly IUserService _userService; //For suspend users
    private readonly INotificationService _notificationService; //To Add notifications
    private readonly IEntitlementExceptionsService _entitlementExceptionsService; //For entitlement exceptions
    private readonly IAuthService _authService;

    #endregion

    #region Constructor
    public MakerCheckerService
    (
        IUnitOfWork unitOfWork,
        ICryptographyService cryptographyService,
        IAuditTrailService auditTrailService,
        IMakerCheckerRepository makerCheckerRepository,
        IUserService userService,
        INotificationService notificationService,
        IEntitlementExceptionsService entitlementExceptionsService,
        IAuthService authService
    )
    {
        _unitOfWork = unitOfWork;
        _cryptographyService = cryptographyService;
        _auditTrailService = auditTrailService;
        _makerCheckerRepository = makerCheckerRepository;
        _userService = userService;
        _notificationService = notificationService;
        _entitlementExceptionsService = entitlementExceptionsService;
        _authService = authService;
    }
    #endregion


    #region Create
    public async Task<IResult<Guid>> AddAsync(NewMakerCheckerModel model, Guid userId)
    {
        //If actions is create new user then the MakerChecker model is empty so use user validation instead
        var validation = (model.Action != MakerCheckerActionsEnum.CreateUser)
                ? await new AddMakerCheckerModelValidator().ValidateAsync(model)
                : await ValidateUser(model.Model);

        if (model.Action == MakerCheckerActionsEnum.CreateUser)
        {
            var userModel = JsonSerializer.Deserialize<NewUserModel>(model.Model);
            userModel!.Password = await _cryptographyService.Encrypt(userModel.Password);
            model.Model = JsonSerializer.Serialize(userModel);
        }

        if (validation.Failed)
            return Result<Guid>.Fail(validation.Message);

        var makerChecker = MakerCheckerFactory.Create(model, userId, model!.Action);

        await _makerCheckerRepository.AddAsync(makerChecker);

        await _unitOfWork.SaveChangesAsync();

        var auditResult = await _auditTrailService.AddAsync(makerChecker, userId);

        if (auditResult.Failed)
        {
            await DeleteAsync(makerChecker.Id, userId);
            return Result<Guid>.Fail(auditResult.Message);
        }

        await _notificationService.AddAsync(userId, SeverityEnum.Serious, "Maker Checker Action Required", RolesEnum.Supervisor, EntityEnum.MakerChecker, makerChecker.Id.ToString());

        return Result<Guid>.Success(makerChecker.Id);
    }

    private async Task<IResult<Guid>> ValidateUser(string modelJson)
    {//Validate the new user model so we can return any problems before saving the request
        var model = await ReworkModel(modelJson, false);

        var userValidation = await new AddUserModelValidator().ValidateAsync(model);

        return userValidation.Failed
            ? Result<Guid>.Fail(userValidation.Message)
            : Result<Guid>.Success();
    }
    #endregion

    #region Read
    public async Task<MakerCheckerModel> GetByIdAsync(Guid id)
        => await _makerCheckerRepository.GetByIdAsync(id);

    public async Task<IEnumerable<Guid>> GetDocumentsByIdAsync(Guid id)
        => await _makerCheckerRepository.GetDocumentsByIdAsync(id);

    public async Task<IEnumerable<MakerCheckerModel>> ListAsync()
        => await _makerCheckerRepository.Queryable.Select(MakerCheckerExpression.Model!).ToListAsync();

    public async Task<IEnumerable<MakerCheckerModel>> ListNonActionedAsync()
        => await _makerCheckerRepository.Queryable.Where(MakerCheckerExpression.NonActioned()!).Select(MakerCheckerExpression.Model!).ToListAsync();
    #endregion

    #region Update
    public async Task<IResult> ApproveAsync(Guid makerCheckerId, Guid activeUserId)
    {
        var makerCheckerAction = await _makerCheckerRepository.GetActionByIdAsync(makerCheckerId);

        var auditResult = await _auditTrailService.AddAsync(makerCheckerId, activeUserId);

        if (auditResult.Failed)
            return await Result.FailAsync(auditResult.Message);

        var actionResult = makerCheckerAction switch
        {
            MakerCheckerActionsEnum.CreateUser =>
                await CreateUser(makerCheckerId, activeUserId),

            MakerCheckerActionsEnum.Entitlement =>
                await Entitlement(makerCheckerId, activeUserId),

            MakerCheckerActionsEnum.SuspendUser =>
                await InactivateUser(makerCheckerId, activeUserId),

            MakerCheckerActionsEnum.ChangePassword =>
                await ChangePassword(makerCheckerId, activeUserId),

            _ => await Result.FailAsync("Action not supported")
        };

        if (actionResult.Failed)
            return await Result.FailAsync(actionResult.Message);

        var makerChecker = new MakerChecker(makerCheckerId);

        makerChecker.Approve(activeUserId);

        await _makerCheckerRepository.ApproveDenyAsync(makerChecker);

        await _unitOfWork.SaveChangesAsync();

        await _notificationService.AddAsync(activeUserId, SeverityEnum.Serious, "Maker Checker Approved", RolesEnum.Clerk, EntityEnum.MakerChecker, makerChecker.Id.ToString());

        return actionResult;
    }
    public async Task<IResult> DenyAsync(Guid makerCheckerId, Guid activeUserId)
    {
        var makerChecker = new MakerChecker(makerCheckerId);

        var auditResult = await _auditTrailService.AddAsync(makerChecker, activeUserId);

        if (auditResult.Failed)
            return Result.Fail(auditResult.Message);

        makerChecker.Deny(activeUserId);

        await _makerCheckerRepository.ApproveDenyAsync(makerChecker);

        await _unitOfWork.SaveChangesAsync();

        await _notificationService.AddAsync(activeUserId, SeverityEnum.Serious, "Maker Checker Declined", RolesEnum.Clerk, EntityEnum.MakerChecker, makerChecker.Id.ToString());

        return await Result.SuccessAsync();
    }
    #endregion

    #region Delete
    private async Task<IResult> DeleteAsync(Guid id, Guid activeUserId)
    {
        var auditResult = await _auditTrailService.AddAsync(new MakerChecker(id), activeUserId);

        if (auditResult.Failed)
            return Result.Fail(auditResult.Message);

        await _makerCheckerRepository.DeleteAsync(id);

        await _unitOfWork.SaveChangesAsync();

        return Result.Success();
    }
    #endregion


    #region Approve Actions
    private async Task<IResult> CreateUser(Guid id, Guid activeUserId)
    {
        var makerCheckerData = GetByIdAsync(id);

        var userModel = await ReworkModel(makerCheckerData.Result.Model, true);

        return await _userService.AddAsync(userModel, activeUserId);
    }
    private async Task<IResult> Entitlement(Guid id, Guid activeUserId)
    {
        var makerCheckerData = GetByIdAsync(id);

        var roleChangeModel = JsonSerializer.Deserialize<RoleChangeModel>(makerCheckerData.Result.Model);

        var result = await _authService.ChangeRole(roleChangeModel!, activeUserId);

        return result.Succeeded && _entitlementExceptionsService.ModelHasExceptions(roleChangeModel!.ClaimExceptions)
            ? await _entitlementExceptionsService.AddAsync(roleChangeModel.ClaimExceptions, roleChangeModel.Login, activeUserId)
            : result;
    }
    private async Task<IResult> ChangePassword(Guid id, Guid activeUserId)
    {
        var makerCheckerData = GetByIdAsync(id);

        var passwordChangeModel = JsonSerializer.Deserialize<AuthModel>(makerCheckerData.Result.Model);

        return await _authService.ChangePassword(passwordChangeModel!, activeUserId);
    }
    private async Task<IResult> InactivateUser(Guid id, Guid activeUserId)
    {
        var makerCheckerData = GetByIdAsync(id);

        var inactiveUserId = JsonSerializer.Deserialize<Guid>(makerCheckerData.Result.Model);

        return await _userService.SuspendAsync(inactiveUserId, activeUserId);
    }

    private async Task<UserModel> ReworkModel(string modelJson, bool decrypt)
    {//Encrypt password when saving model and decrypt when actioning (accepting) model
        var model = JsonSerializer.Deserialize<NewUserModel>(modelJson!);

        var authModel = new AuthModel
        {
            Login = model!.Email, //All Roles that have access to maker checker can decrypt so no need to ever encrypt
            Password = decrypt ? await _cryptographyService.Decrypt(model.Password) : model.Password,
            Role = RolesEnum.Clerk
        };

        return new UserModel
        {
            Auth = authModel,
            Email = model.Email,
            Name = model.FirstName,
            Surname = model.LastName
        };
    }
    #endregion

}
