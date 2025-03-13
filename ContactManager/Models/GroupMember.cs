namespace ContactManager.Models
{
    public class GroupMember
    {
        public int Id { get; set; }

        public int ContactId { get; set; }
        public Contact? Contact { get; set; } // Made nullable to fix CS8618

        public int GroupId { get; set; }
        public Group? Group { get; set; } // Made nullable to fix CS8618
    }
}