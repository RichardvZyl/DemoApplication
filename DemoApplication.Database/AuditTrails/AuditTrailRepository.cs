using Abstractions.EntityFrameworkCore;
using DemoApplication.Domain;
using DemoApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Database.AuditTrails
{
    public sealed class AuditTrailRepository : EFRepository<AuditTrail>, IAuditTrailRepository
    {
        public AuditTrailRepository(Context context) : base(context) { }

        public async Task<AuditTrailModel> GetByIdAsync(Guid id)
            => await Queryable.Where(AuditTrailExpression.Id(id)!).Select(AuditTrailExpression.Model!).SingleOrDefaultAsync() ?? new();

        public async Task<IEnumerable<AuditTrailModel>> ListLastMonth()
            => await Queryable.Where(AuditTrailExpression.LastMonth()!).Select(AuditTrailExpression.Model!).ToListAsync();
    }
}
