namespace eBoardAPI.Models.Parent
{
    public class CreateParentDto
    {
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
        public string Email { get; set; } = string.Empty;
    }
}
