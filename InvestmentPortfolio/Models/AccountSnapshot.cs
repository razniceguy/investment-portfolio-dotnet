namespace InvestmentPortfolio.Models
{
    public class AccountSnapshot
    {
        public string type { get; set; }
        public long updateTime { get; set; }
        public SnapshotData data { get; set; }
    }
}
