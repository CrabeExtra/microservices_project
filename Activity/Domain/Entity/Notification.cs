

namespace Activity.Domain.Entity;

public class Notification
{
    public Guid Id { get; set; }

    public DateTime Created { get; set; } = "";

    public string Message { get; set; } = "";
}