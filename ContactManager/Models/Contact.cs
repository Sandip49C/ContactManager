namespace ContactManager.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category? Category { get; set; } // Nullable navigation property
        public List<Phone> Phones { get; set; } = new List<Phone>();
        public List<Email> Emails { get; set; } = new List<Email>();
    }
}