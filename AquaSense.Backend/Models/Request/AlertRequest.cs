namespace AquaSense.Backend.Models.Request;

public class AlertRequest
{
    public Guid ReadingId { get; set; }
    public Guid RuleId { get; set; }
    public DateTime? TriggeredAt { get; set; }
    public bool IsResolved { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public Guid? ResolvedBy { get; set; }
}
