namespace InvestmentPortfolio.Models
{
    public class GitHubBranch
    {
        public string name { get; set; }
        public Commit commit { get; set; }
        public string _protected { get; set; }
    }
}
