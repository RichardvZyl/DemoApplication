using Abstractions.Repositories;
using DemoApplication.Domain;
using DemoApplication.Enums;
using DemoApplication.Models;

namespace DemoApplication.Database;

public interface IMakerCheckerRepository : IRepository<MakerChecker>
{
    Task<MakerCheckerModel> GetByIdAsync(Guid id);

    Task<MakerCheckerActionsEnum> GetActionByIdAsync(Guid id);
    Task<IEnumerable<Guid>> GetDocumentsByIdAsync(Guid id);
    Task ApproveDenyAsync(MakerChecker makerChecker);

}
