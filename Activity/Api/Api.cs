namespace Activity.Api;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/activity/[controller]")]
public abstract class BaseController : ControllerBase;