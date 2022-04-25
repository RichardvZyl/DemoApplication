using System;
using Abstractions.Domain;
using DemoApplication.Models;

namespace DemoApplication.Domain;

public class Document : Entity<Guid>
{
    public Document
    (
        //Guid id,
        //Guid relatedId,
        //int sequenceNumber,
        string name,
        //string documentSubject,
        //bool isFreeText,
        byte[] contents,
        Guid userId
    )
    {
        //Id = id;
        //RelatedId = relatedId;
        //SequenceNumber = sequenceNumber;
        Name = name;
        //DocumentSubject = documentSubject;
        //IsFreeText = isFreeText;
        Contents = contents;
        UserId = userId;
    }

    public Document(Guid id) : base(id) { }

    //public Guid Id { get; private set; }

    //public Guid RelatedId { get; private set; }

    //public int SequenceNumber { get; private set; }

    public string Name { get; private set; } = string.Empty;

    //public string DocumentSubject { get; private set; }

    //public bool IsFreeText { get; private set; }

    public byte[] Contents { get; private set; } = Array.Empty<byte>();

    public Guid UserId { get; private set; }

    public void UpdateDocument(DocumentModel documentModel)
    {
        Name = documentModel.Name;
        Contents = documentModel.Contents;
        UserId = documentModel.UserId; // TODO: Should another user be able to update document
    }
}
