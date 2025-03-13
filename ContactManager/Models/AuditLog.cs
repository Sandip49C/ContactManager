namespace ContactManager.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public Contact Contact { get; set; } = null!;
        public string Action { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}