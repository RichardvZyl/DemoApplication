using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
using Abstractions.Results;
using DemoApplication.Database;
using DemoApplication.Database.AuditTrails;
using DemoApplication.Factory;
using DemoApplication.Models;
using DemoApplication.Validator;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Framework;

public sealed class AuditTrailService : IAuditTrailService
{
    #region Private Members
    private readonly IAuditTrailRepository _auditTrailRepository;
    private readonly IUnitOfWork _unitOfWork;
    #endregion

    #region Constructor
    public AuditTrailService
    (
        IUnitOfWork unitOfWork,
        IAuditTrailRepository auditTrailRepository
    )
    {
        _auditTrailRepository = auditTrailRepository;
        _unitOfWork = unitOfWork;
    }
    #endregion

    #region CRUD
    public async Task<IResult<Guid>> AddAsync(object model, Guid userId, [CallerMemberName] string action = "")
    {
        var auditModel = new AuditTrailModel
        {//Create an audit model based on the context
            Date = DateTimeOffset.UtcNow,
            UserId = userId,
            DisplayContext = $"{action.Replace("Async", "")} - {model.GetType().Name.Replace("Model", "")}", //The context action being performed and object action is being performed on //also remove "model" from string
            Model = model.GetType().Name,
            Contents = JsonSerializer.Serialize(model)
        };

        var validation = await new AddAuditTrailModelValidator().ValidateAsync(auditModel);

        if (validation.Failed)
            return await Result<Guid>.FailAsync(validation.Message);

        var auditTrail = AuditTrailFactory.Create(auditModel);

        await _auditTrailRepository.AddAsync(auditTrail);

        await _unitOfWork.SaveChangesAsync();

        return await Result<Guid>.SuccessAsync(auditTrail.Id);
    }
    public async Task<AuditTrailModel> GetAsync(Guid id)
        => await _auditTrailRepository.GetByIdAsync(id);

    public async Task<IEnumerable<AuditTrailModel>> ListAsync()
        => await _auditTrailRepository.Queryable.Where(AuditTrailExpression.OneMonth()!).Select(AuditTrailExpression.Model!).ToListAsync();

    public async Task<IEnumerable<AuditTrailModel>> ListLastMonthAsync()
        => await _auditTrailRepository.ListLastMonth();
    #endregion
}
