namespace eBoardAPI.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid TeacherId { get; set; }

        public string Token { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; }

        public Teacher Teacher { get; set; } = null!;
    }

}
