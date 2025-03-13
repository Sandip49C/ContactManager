using System.ComponentModel.DataAnnotations;

namespace ContactManager.Models
{
    public class Note
    {
        public int Id { get; set; }

        public int ContactId { get; set; }
        public Contact? Contact { get; set; } // Made nullable to fix CS8618

        [Required]
        public string Content { get; set; } = string.Empty;
    }
}