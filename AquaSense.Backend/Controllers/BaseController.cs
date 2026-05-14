using Microsoft.AspNetCore.Mvc;

namespace AquaSense.Backend.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    protected BaseController(ILogger logger)
    {
        Logger = logger;
    }

    protected ILogger Logger { get; }
}
