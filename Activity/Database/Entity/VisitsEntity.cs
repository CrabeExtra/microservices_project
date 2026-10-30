namespace Activity.Database.Entity;

public class VisitsEntity
{
    public int Id { get; set; }
    
    public DateTime VisitedAt { get; set; }

    public bool FirstVisit { get; set; }
}