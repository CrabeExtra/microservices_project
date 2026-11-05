using Microsoft.AspNetCore.Mvc;

namespace Identity.Api;

[ApiController]
[Route("api/identity/[controller]")]
public abstract class BaseController : ControllerBase;