using Microsoft.AspNetCore.Mvc;

namespace Personal.Api;

[ApiController]
[Route("api/personal/[controller]")]
public abstract class BaseController : ControllerBase;