using Abstractions.Results;
using DemoApplication.Models;

namespace DemoApplication.Framework;

public interface IMakerCheckerService
{
    Task<IResult<Guid>> AddAsync(NewMakerCheckerModel model, Guid activeUserId);

    Task<MakerCheckerModel> GetByIdAsync(Guid id);
    Task<IEnumerable<Guid>> GetDocumentsByIdAsync(Guid id);
    Task<IEnumerable<MakerCheckerModel>> ListAsync();
    Task<IEnumerable<MakerCheckerModel>> ListNonActionedAsync();

    Task<IResult> DenyAsync(Guid makerCheckerId, Guid activeUserId);
    Task<IResult> ApproveAsync(Guid makerCheckerId, Guid activeUserId);
}
