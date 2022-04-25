using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.EntityFrameworkCore;
using DemoApplication.Domain;
using DemoApplication.Enums;
using DemoApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace DemoApplication.Database;

public sealed class MakerCheckerRepository : EFRepository<MakerChecker>, IMakerCheckerRepository
{
    public MakerCheckerRepository(Context context) : base(context) { }

    public async Task<MakerCheckerModel> GetByIdAsync(Guid id) 
        => await Queryable.Where(MakerCheckerExpression.Id(id)).Select(MakerCheckerExpression.Model).SingleOrDefaultAsync();

    public async Task<MakerCheckerActionsEnum> GetActionByIdAsync(Guid id) 
        => await Queryable.Where(MakerCheckerExpression.Id(id)).Select(MakerCheckerExpression.Action()).SingleOrDefaultAsync();
    public async Task<IEnumerable<Guid>> GetDocumentsByIdAsync(Guid id) 
        => await Queryable.Where(MakerCheckerExpression.Id(id)).Select(MakerCheckerExpression.Files()).SingleOrDefaultAsync();
    public Task ApproveDenyAsync(MakerChecker makerChecker) 
        => UpdatePartialAsync(makerChecker.Id, new { makerChecker.CheckerDate, makerChecker.Accepted, makerChecker.CheckerUser });
}
