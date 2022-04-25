using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abstractions.AspNetCore;
using Abstractions.Results;
using DemoApplication.Entitlement;
using DemoApplication.Framework;
using DemoApplication.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DemoApplication.Api.Controllers;

/// <summary> File Controller used to upload and download files </summary>
[ApiController]
[ApiVersion("1")]
[ApiVersion("2")]
[Route("api/v{version:apiVersion}/[controller]")]
[Authorize]
public sealed class DocumentsController : ControllerBase
{
    // private readonly IContentTypeProvider _contentTypeProvider;
    // private readonly string _directory;
    private readonly IDocumentService _fileService;

    /// <summary> File Controller Constructor </summary>
    public DocumentsController
    (
        // IContentTypeProvider contentTypeProvider,
        IDocumentService fileService
        // IHostEnvironment environment
    ) =>
        // _contentTypeProvider = contentTypeProvider;
        // _directory = Path.Combine(environment.ContentRootPath, "Files");
        _fileService = fileService;

    /// <summary> Upload a new file </summary>
    /// <param name="fileModel"></param>
    /// <returns>Guid</returns>
    /// <remarks> Send a document for saving on the server </remarks>
    [DisableRequestSizeLimit]
    [ProducesResponseType(200, Type = typeof(IResult<Guid>))]
    [MapToApiVersion("1")]
    [HttpPost]
    public async Task<IActionResult> PostFile([FromForm] FileModel fileModel)
    {
        var currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Identity")?.Value!);
        //Request.ContentType = new MediaTypeHeaderValue("application/multipart/form-data").ToString();

        if (fileModel.File != null)
        {
            var documentModel = new DocumentModel();

            using (var memoryStream = new MemoryStream())
            {
                await fileModel.File.CopyToAsync(memoryStream);

                documentModel.UserId = currentUserId;
                documentModel.Contents = memoryStream.ToArray();
                documentModel.Name = fileModel.File.FileName;
            }

            return await _fileService.AddAsync(documentModel).ResultAsync();
        }

        return await Task.FromResult((IActionResult)Result.FailAsync());
    }

    /// <summary> Delete an uploaded file </summary>
    /// <param name="fileId"></param>
    /// <returns>Result</returns>
    /// <remarks> Remove a document by supplying the document Id </remarks>
    [ProducesResponseType(200, Type = typeof(FileContentResult))]
    [HttpPost("Remove/{fileId}")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> RemoveAsync(Guid fileId)
    {
        if (!User.HasClaim(x => x.Value == Claims.AuthorizeMakerChecker))
            return await Task.FromResult((IActionResult)Unauthorized());

        var currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Identity")?.Value!);

        return await _fileService.DeleteAsync(fileId, currentUserId).ResultAsync();
    }


    /// <summary> Get a specified file </summary>
    /// <param name="id"></param>
    /// <returns>FileContentResult</returns>
    /// <remarks> Get a previously stored file on from the server </remarks>
    [ProducesResponseType(200, Type = typeof(FileContentResult))]
    [HttpGet("{id}")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        if (!User.HasClaim(x => x.Value == Claims.AuthorizeMakerChecker))
            return await Task.FromResult((IActionResult)Unauthorized());

        var file = await _fileService.GetAsync(id);

        //_contentTypeProvider.TryGetContentType(file.ContentType, out var contentType);

        return File(file?.Data?.Bytes ?? Array.Empty<byte>(), file?.Data?.ContentType ?? string.Empty, file?.Data?.Name ?? string.Empty);
    }
}


