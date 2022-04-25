using Microsoft.EntityFrameworkCore;
using DemoApplication.Domain;
using Abstractions.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DemoApplication.Database;

public sealed class DocumentRepository : EFRepository<Document>, IDocumentRepository
{
    public DocumentRepository(Context context) : base(context) { }

    public async Task<bool> AnyByIdAsync(Guid id) 
        => await Queryable.AnyAsync(DocumentExpression.Id(id));

    public async Task<Document> GetByIdAsync(Guid id) 
        => await Queryable.SingleOrDefaultAsync(DocumentExpression.Id(id));

    public async Task<string> GetNameByIdAsync(Guid id) 
        => await Queryable.Where(DocumentExpression.Id(id)).Select(DocumentExpression.Name()).SingleOrDefaultAsync();
}
