using Microsoft.AspNetCore.Mvc;

namespace Contact.Api;

[ApiController]
[Route("api/contact/[controller]")]
public abstract class BaseController : ControllerBase;