using Microsoft.AspNetCore.Mvc;

namespace Identity.Api;

[ApiController]
[Route("api/contact/[controller]")]
public abstract class BaseController : ControllerBase;