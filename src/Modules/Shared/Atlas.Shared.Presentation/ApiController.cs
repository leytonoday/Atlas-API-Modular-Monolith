using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Atlas.Shared.Presentation;

/// <summary>
/// Base class for API controllers providing common functionality and error handling.
/// </summary>
[ApiController]
public abstract class ApiController : ControllerBase
{
    private ISender _sender = null!;

    /// <summary>
    /// Gets the sender.
    /// </summary>
    protected ISender Sender => _sender ??= HttpContext.RequestServices.GetService<ISender>()!;
}

