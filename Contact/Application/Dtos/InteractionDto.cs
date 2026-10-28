namespace Contact.Application.Dtos;

public class InteractionDto
{
    public String Email { get; set; } = "";
    public String MainText { get; set; } = "";
}

public class InteractionResponseDto
{
    public Boolean Success { get; set; }
    public String? Reason { get; set; }
}