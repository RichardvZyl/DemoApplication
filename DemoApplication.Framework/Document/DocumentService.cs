using Abstractions.Objects;
using Abstractions.Results;
using DemoApplication.Database;
using DemoApplication.Domain;
using DemoApplication.Factory;
using DemoApplication.Models;
using DemoApplication.Validator;
using Microsoft.AspNetCore.StaticFiles;

namespace DemoApplication.Framework;

public sealed class DocumentService : IDocumentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDocumentRepository _documentRepository;
    private readonly IAuditTrailService _auditTrailService;

    public DocumentService
    (
        IUnitOfWork unitOfWork,
        IDocumentRepository documentRepository,
        IAuditTrailService auditTrailService
    )
    {
        _unitOfWork = unitOfWork;
        _documentRepository = documentRepository;
        _auditTrailService = auditTrailService;
    }

    public async Task<IResult<Guid>> AddAsync(DocumentModel documentModel)
    {
        var validation = await new AddDocumentModelValidator().ValidateAsync(documentModel);

        if (validation.Failed)
            return await Result<Guid>.FailAsync(validation.Message);

        var auditResult = await _auditTrailService.AddAsync(new DocumentModel(), documentModel.UserId);

        if (auditResult.Failed)
            return Result<Guid>.Fail(auditResult.Message);

        var document = DocumentFactory.Create(documentModel);

        await _documentRepository.AddAsync(document);

        await _unitOfWork.SaveChangesAsync();

        return await Result<Guid>.SuccessAsync(document.Id);
    }

    public async Task<IResult> UpdateAsync(DocumentModel documentModel, Guid activeUserId)
    {
        var validation = await new UpdateDocumentModelValidator().ValidateAsync(documentModel);

        if (validation.Failed)
            return await Result<Guid>.FailAsync(validation.Message);

        var document = new Document(documentModel.Id);

        var auditResult = await _auditTrailService.AddAsync(document, activeUserId);

        if (auditResult.Failed)
            return await Result.FailAsync(auditResult.Message);

        document.UpdateDocument(documentModel);

        document = DocumentFactory.Create(documentModel);

        await _documentRepository.UpdateAsync(documentModel.Id, document);

        await _unitOfWork.SaveChangesAsync();

        return await Result.SuccessAsync();
    }

    public async Task<IResult<BinaryFile>> GetAsync(Guid id)
    {
        var document = await _documentRepository.GetByIdAsync(id);

        if (document is null)
            return await Result<BinaryFile>.FailAsync("File not found");

        static string GetMimeType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
                contentType = "application/octet-stream";

            return contentType;
        }

        return await Result<BinaryFile>.SuccessAsync(new BinaryFile
        (
            id,
            document.Name,
            document.Contents,
            document.Contents.Length,
            GetMimeType(document.Name)
        ));
    }

    public async Task<IResult> DeleteAsync(Guid id, Guid activeUserId)
    {
        var auditResult = await _auditTrailService.AddAsync(new Document(id), activeUserId);

        if (auditResult.Failed)
            return Result.Fail(auditResult.Message);

        await _documentRepository.DeleteAsync(id);

        await _unitOfWork.SaveChangesAsync();

        return await Result.SuccessAsync();
    }
}
