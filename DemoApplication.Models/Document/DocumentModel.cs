using System;

namespace DemoApplication.Models;

/// <summary>
/// the document entity definition
/// </summary>
public class DocumentModel
{
    /// <summary>
    /// the primary key of the entity
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// the related id of the entity that will need authorization
    /// </summary>
    //public Guid RelatedId { get; set; }

    /// <summary>
    /// the sequence number in the case where 
    /// </summary>
    //public int SequenceNumber { get; set; }

    /// <summary>
    /// the name of the document
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// the subject the same as email style
    /// </summary>
    //public string DocumentSubject { get; set; }

    /// <summary>
    /// is this a document or just free text
    /// </summary>
    //public bool IsFreeText { get; set; }

    /// <summary>
    /// the actual contents of the document
    /// </summary>
    public byte[] Contents { get; set; }

    /// <summary>
    /// the date and time when the document was loaded
    /// </summary>
    public DateTimeOffset DateTimeOffsetLoaded { get; set; }

    /// <summary>
    /// the id of the user creating this document
    /// </summary>
    public Guid UserId { get; set; } //part of related MakerChecker
}
