using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using DemoApplication.Domain;
using DemoApplication.Enums;
using DemoApplication.Models;

namespace DemoApplication.Database;

public static class MakerCheckerExpression
{
    #region Get By Values
    public static Expression<Func<MakerChecker, bool>> MakerAction(MakerCheckerActionsEnum action) => makerChecker => makerChecker.Action == action;
    public static Expression<Func<MakerChecker, bool>> MakerUser(Guid id) => makerChecker => makerChecker.MakerUser == id;
    public static Expression<Func<MakerChecker, bool>> CheckerUser(Guid id) => makerChecker => makerChecker.CheckerUser == id;
    public static Expression<Func<MakerChecker, bool>> Accepted(bool accepted) => makerChecker => makerChecker.Accepted == accepted;
    public static Expression<Func<MakerChecker, bool>> Id(Guid id) => makerChecker => makerChecker.Id == id;
    public static Expression<Func<MakerChecker, bool>> NonActioned() => makerChecker => makerChecker.CheckerDate == null;
    #endregion

    #region Values
    public static Expression<Func<MakerChecker, MakerCheckerActionsEnum>> Action() => makerChecker => makerChecker.Action;
    public static Expression<Func<MakerChecker, Guid>> MakerUser() => makerChecker => makerChecker.MakerUser;
    public static Expression<Func<MakerChecker, Guid>> CheckerUser() => makerChecker => makerChecker.CheckerUser;
    public static Expression<Func<MakerChecker, DateTimeOffset>> MakerDate() => makerChecker => makerChecker.MakerDate;
    public static Expression<Func<MakerChecker, DateTimeOffset?>> CheckerDate() => makerChecker => makerChecker.CheckerDate;
    public static Expression<Func<MakerChecker, bool>> Accepted() => makerChecker => makerChecker.Accepted;
    public static Expression<Func<MakerChecker, string>> Motivation() => makerChecker => makerChecker.Motivation;
    public static Expression<Func<MakerChecker, string>> MakerModel() => makerChecker => makerChecker.Model;
    public static Expression<Func<MakerChecker, IEnumerable<Guid>>> Files() => makerChecker => makerChecker.Files;
    public static Expression<Func<MakerChecker, MakerCheckerModel>> Model => makerChecker => new MakerCheckerModel
    {
        Id = makerChecker.Id,
        Action = makerChecker.Action,
        MakerUser = makerChecker.MakerUser,
        CheckerUser = makerChecker.CheckerUser,
        MakerDate = makerChecker.MakerDate,
        CheckerDate = makerChecker.CheckerDate,
        Accepted = makerChecker.Accepted,
        Motivation = makerChecker.Motivation,
        Files = makerChecker.Files,
        Model = makerChecker.Model
    };
    #endregion

}
