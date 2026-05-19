namespace Activity.Database.Entity;

public class NotificationEntity
{
    public int Id { get; set; }
    
    public DateTime Created { get; set; } = "";

    public string Message { get; set; } = "";
}