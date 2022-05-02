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
    /// <response code="200">Succesful response returns the ID of the added document</response>
    /// <response code="204">An empty file was sent to the server</response>
    /// <response code="400">The server cannot or will not process the request due to something that is perceived to be a client error</response>
    /// <response code="401">Unauthorized client needs to authenticate first</response>
    /// <response code="422">semantic errors occured</response>
    /// <response code="500">An unexpected error occured</response>
    /// <response code="504">A timeout occured</response>
    [ProducesResponseType(200, Type = typeof(Guid))]
    [ProducesResponseType(204, Type = typeof(NoContentResult))]
    [ProducesResponseType(400, Type = typeof(BadRequestResult))]
    [ProducesResponseType(401, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(422, Type = typeof(UnprocessableEntityResult))]
    [ProducesResponseType(500, Type = typeof(Exception))]
    [ProducesResponseType(504, Type = typeof(TimeoutException))]
    [DisableRequestSizeLimit]
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
    /// <response code="200">Succesful response document deleted</response>
    /// <response code="204">No file found to delete</response>
    /// <response code="400">The server cannot or will not process the request due to something that is perceived to be a client error</response>
    /// <response code="401">Unauthorized client needs to authenticate first</response
    /// <response code="403">Forbidden The client does not have access rights to the content</response>
    /// <response code="422">semantic errors occured</response>
    /// <response code="500">An unexpected error occured</response>
    /// <response code="504">A timeout occured</response>
    [ProducesResponseType(200, Type = typeof(FileContentResult))]
    [ProducesResponseType(204, Type = typeof(NoContentResult))]
    [ProducesResponseType(400, Type = typeof(BadRequestResult))]
    [ProducesResponseType(401, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(403, Type = typeof(ForbidResult))]
    [ProducesResponseType(422, Type = typeof(UnprocessableEntityResult))]
    [ProducesResponseType(500, Type = typeof(Exception))]
    [ProducesResponseType(504, Type = typeof(TimeoutException))]
    [HttpPost("Remove/{fileId}")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> RemoveAsync(Guid fileId)
    {
        if (!User.HasClaim(x => x.Value == Claims.AuthorizeMakerChecker))
            return await Task.FromResult((IActionResult)Forbid());

        var currentUserId = Guid.Parse(User.Claims.FirstOrDefault(c => c.Type == "Identity")?.Value!);

        return await _fileService.DeleteAsync(fileId, currentUserId).ResultAsync();
    }


    /// <summary> Get a specified file </summary>
    /// <param name="id"></param>
    /// <returns>FileContentResult</returns>
    /// <remarks> Get a previously stored file on from the server </remarks>
    /// <response code="200">Succesful response file returned</response>
    /// <response code="204">No file found with specified ID</response>
    /// <response code="400">The server cannot or will not process the request due to something that is perceived to be a client error</response>
    /// <response code="401">Unauthorized client needs to authenticate first</response>
    /// <response code="403">Forbidden The client does not have access rights to the content</response>
    /// <response code="422">semantic errors occured</response>
    /// <response code="500">An unexpected error occured</response>
    /// <response code="504">A timeout occured</response>
    [ProducesResponseType(200, Type = typeof(FileContentResult))]
    [ProducesResponseType(204, Type = typeof(NoContentResult))]
    [ProducesResponseType(400, Type = typeof(BadRequestResult))]
    [ProducesResponseType(401, Type = typeof(UnauthorizedResult))]
    [ProducesResponseType(403, Type = typeof(ForbidResult))]
    [ProducesResponseType(422, Type = typeof(UnprocessableEntityResult))]
    [ProducesResponseType(500, Type = typeof(Exception))]
    [HttpGet("{id}")]
    [MapToApiVersion("1")]
    public async Task<IActionResult> GetAsync(Guid id)
    {
        if (!User.HasClaim(x => x.Value == Claims.AuthorizeMakerChecker))
            return await Task.FromResult((IActionResult)Forbid());

        var file = await _fileService.GetAsync(id);

        //_contentTypeProvider.TryGetContentType(file.ContentType, out var contentType);

        return File(
            file?.Data?.Bytes ?? Array.Empty<byte>(),
            file?.Data?.ContentType ?? string.Empty,
            file?.Data?.Name ?? string.Empty
            );
    }
}


