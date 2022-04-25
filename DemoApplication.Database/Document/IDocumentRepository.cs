using DemoApplication.Domain;
using Abstractions.Repositories;
using System;
using System.Threading.Tasks;

namespace DemoApplication.Database;

public interface IDocumentRepository : IRepository<Document>
{
    Task<bool> AnyByIdAsync(Guid id);
    Task<Document> GetByIdAsync(Guid id);
    Task<string> GetNameByIdAsync(Guid id);
}
