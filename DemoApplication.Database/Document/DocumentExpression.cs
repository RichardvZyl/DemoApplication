using System.Linq.Expressions;
using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Database;

public static class DocumentExpression
{
    #region Values
    public static Expression<Func<Document, Guid>> Id() => document => document.Id;
    public static Expression<Func<Document, string>> Name() => document => document.Name;
    public static Expression<Func<Document, byte[]>> Contents() => document => document.Contents;
    //public static Expression<Func<Document, DateTimeOffset>> DateTimeOffsetLoaded() => document => document.DateTimeOffsetLoaded;
    public static Expression<Func<Document, Guid>> UserId() => document => document.UserId;
    public static Expression<Func<Document, DocumentModel>> Model => document => new DocumentModel
    {
        Id = document.Id,
        //RelatedId = document.RelatedId,
        //SequenceNumber = document.SequenceNumber,
        Name = document.Name,
        //DocumentSubject = document.DocumentSubject,
        //IsFreeText = document.IsFreeText,
        Contents = document.Contents,
        //DateTimeOffsetLoaded = document.DateTimeOffsetLoaded,
        UserId = document.UserId
    };
    #endregion

    #region GetByValues
    public static Expression<Func<Document, bool>> Id(Guid id) => document => document.Id == id;
    public static Expression<Func<Document, bool>> UserId(Guid id) => document => document.UserId == id;
    #endregion
}
