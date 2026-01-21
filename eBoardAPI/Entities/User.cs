namespace eBoardAPI.Entities
{
    public enum ROLE
    {
        Teacher,
        Parent
    }
    public class User
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public ROLE Role { get; set; } = ROLE.Teacher;
    }

}
