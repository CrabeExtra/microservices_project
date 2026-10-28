namespace Contact.Application.Dtos;

public class InteractionDto
{
    public string Email { get; set; } = "";
    public string Body { get; set; } = "";
}

public class InteractionResponseDto
{
    public bool Success { get; set; }
    public string? Reason { get; set; }
}