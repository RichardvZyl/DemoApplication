using DemoApplication.Domain;
using DemoApplication.Models;

namespace DemoApplication.Factory;

public static class DocumentFactory
{
    public static Document Create(DocumentModel model) =>
        new
        (
            //model.Id,
            //model.RelatedId,
            //model.SequenceNumber,
            model.Name,
            //model.DocumentSubject,
            //model.IsFreeText,
            model.Contents,
            //DateTimeOffset.UtcNow, //model.DateTimeOffsetLoaded, //Part of EntityClass now
            model.UserId
        );
}
