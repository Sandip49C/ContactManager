namespace ContactManager.Models
{
    public class Notification
    {
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
        public int UserId { get; set; }
    }
}