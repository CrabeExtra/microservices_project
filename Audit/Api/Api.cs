using Microsoft.AspNetCore.Mvc;

namespace Audit.Api;

[ApiController]
[Route("api/contact/[controller]")]
public abstract class BaseController : ControllerBase;