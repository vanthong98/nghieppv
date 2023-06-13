namespace Nghiep188.Api.Persistence.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public long Balance { get; set; }
        public DateTimeOffset? CreatedDate { get; set; }
    }
}
