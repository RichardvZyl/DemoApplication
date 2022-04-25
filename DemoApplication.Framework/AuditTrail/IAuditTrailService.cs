using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Abstractions.Results;
using DemoApplication.Models;

namespace DemoApplication.Framework;

public interface IAuditTrailService
{
    Task<IResult<Guid>> AddAsync(object model, Guid userId, [CallerMemberName] string action = "");
    Task<AuditTrailModel> GetAsync(Guid id);
    Task<IEnumerable<AuditTrailModel>> ListAsync();
    Task<IEnumerable<AuditTrailModel>> ListLastMonthAsync();
}
