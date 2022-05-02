using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abstractions.Repositories;
using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Database.AuditTrails
{
    public interface IAuditTrailRepository : IRepository<AuditTrail>
    {
        Task<AuditTrailModel> GetByIdAsync(Guid id);
        Task<IEnumerable<AuditTrailModel>> ListLastMonth();
    }
}
