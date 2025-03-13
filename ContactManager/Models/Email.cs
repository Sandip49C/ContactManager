namespace ContactManager.Models
{
    public class Email
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string EmailAddress { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }
}