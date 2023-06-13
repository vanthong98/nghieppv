namespace Nghiep188.Api.Persistence.Entity
{
    public class ServerSeed
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Value { get; set; }
        public int Nonce { get; set; }
        public string? Sha256HashedValue { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public User? User { get; set; }

        public void HideActiveServerSeed()
        {
            if (IsActive)
            {
                Value = "HIDDEN";
            }
        }
    }
}
