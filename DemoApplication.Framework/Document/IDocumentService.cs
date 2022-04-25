using System;
using System.Threading.Tasks;
using Abstractions.Objects;
using Abstractions.Results;
using DemoApplication.Models;

namespace DemoApplication.Framework;

public interface IDocumentService
{
    Task<IResult<BinaryFile>> GetAsync(Guid id);
    Task<IResult<Guid>> AddAsync(DocumentModel model);

    Task<IResult> DeleteAsync(Guid id, Guid activeUserId);
}
