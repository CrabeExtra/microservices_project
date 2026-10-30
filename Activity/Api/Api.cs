namespace Activity.Api;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/contact/[controller]")]
public abstract class BaseController : ControllerBase;