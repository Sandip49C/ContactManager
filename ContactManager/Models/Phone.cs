namespace ContactManager.Models
{
    public class Phone
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
    }
}