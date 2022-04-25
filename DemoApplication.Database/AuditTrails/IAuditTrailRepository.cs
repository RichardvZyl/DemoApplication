using DemoApplication.Domain;
using DemoApplication.Models;
using Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DemoApplication.Database.AuditTrails
{
    public interface IAuditTrailRepository : IRepository<AuditTrail>
    {
        Task<AuditTrailModel> GetByIdAsync(Guid id);
        Task<IEnumerable<AuditTrailModel>> ListLastMonth();
    }
}
