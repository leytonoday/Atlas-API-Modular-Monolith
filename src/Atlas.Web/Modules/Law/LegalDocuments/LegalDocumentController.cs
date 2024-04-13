using Atlas.Law.Application.CQRS.LegalDocuments.Commands.CreateLegalDocuments;
using Atlas.Law.Application.CQRS.LegalDocuments.Commands.CreateLegalDocumentSummary;
using Atlas.Law.Application.CQRS.LegalDocuments.Commands.DeleteLegalDocument;
using Atlas.Law.Application.CQRS.LegalDocuments.Queries.GetLegalDocumentSummary;
using Atlas.Law.Application.CQRS.LegalDocuments.Queries.GetUserLegalDocuments;
using Atlas.Law.Infrastructure.Module;
using Atlas.Shared.Application.Abstractions;
using Atlas.Shared.Domain.Results;
using Atlas.Web.Modules.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Atlas.Web.Modules.Law.LegalDocuments;

[Route("/api/{version:apiVersion}/legal-document")]
[ApiVersion("1.0")]
public class LegalDocumentController(IExecutionContextAccessor executionContext, ILawModule lawModule) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> CreateDocuments([FromBody] CreateLegalDocumentsCommand command, CancellationToken cancellationToken)
    {
        var result = await lawModule.SendCommand(command, cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpGet]
    public async Task<IActionResult> GetUserDocuments(CancellationToken cancellationToken)
    {
        var result = await lawModule.SendQuery(new GetUserLegalDocumentsQuery(executionContext.UserId), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpGet("summary/{legalDocumentId}")]
    public async Task<IActionResult> GetLegalDocumentSummary(Guid legalDocumentId ,CancellationToken cancellationToken)
    {
        var result = await lawModule.SendQuery(new GetLegalDocumentSummaryQuery(legalDocumentId), cancellationToken);
        return Ok(Result.Success(result));
    }

    [HttpPost("summary/{legalDocumentId}")]
    public async Task<IActionResult> CreateLegalDocumentSummary(Guid legalDocumentId, CancellationToken cancellationToken)
    {
        await lawModule.SendCommand(new CreateLegalDocumentSummaryCommand(legalDocumentId), cancellationToken);
        return Ok(Result.Success());
    }

    [HttpDelete("{legalDocumentId}")]
    public async Task<IActionResult> DeleteLegalDocument(Guid legalDocumentId, CancellationToken cancellationToken)
    {
        await lawModule.SendCommand(new DeleteLegalDocumentCommand(legalDocumentId), cancellationToken);
        return Ok(Result.Success());
    }
}
