using Microsoft.AspNetCore.Mvc;

namespace Audit.Api;

[ApiController]
[Route("api/audit/[controller]")]
public abstract class BaseController : ControllerBase;