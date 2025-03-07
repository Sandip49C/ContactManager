namespace ContactManager.Models
{
    public class Notification
    {
        public string Message { get; set; }
        public string Type { get; set; } // e.g., "success", "error", "warning"
    }
}