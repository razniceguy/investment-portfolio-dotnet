using System.Collections.Generic;

namespace InvestmentPortfolio.Models
{
    public class DailyAccount
    {
        public int code { get; set; }
        public string msg { get; set; }
        public List<AccountSnapshot> snapshotVos { get; set; }

    }
}
