using System.Collections.Generic;

namespace InvestmentPortfolio.Models
{
    public class SnapshotData
    {
        public string totalAssetOfBtc { get; set; }
        public List<SnapShotBalanceData> balances { get; set; }
    }
}
