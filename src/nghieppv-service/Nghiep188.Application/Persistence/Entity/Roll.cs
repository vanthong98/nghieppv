using Nghiep188.Api.Enum;

namespace Nghiep188.Api.Persistence.Entity
{
    public class Roll
    {
        public int Id { get; set; }
        public int ServerSeedId { get; set; }
        public int Number { get; set; }
        public BetOption Option { get; set; }
        public long BetAmount { get; set; }
        public long BalanceBefore { get; set; }
        public long BalanceAfter { get; set; }
        public string? ClientSeed { get; set; }
        public int Nonce { get; set; }
        public ServerSeed? ServerSeed { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
