using InvestmentPortfolio.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;

namespace InvestmentPortfolio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        static HttpClient httpClient = new HttpClient();
        [HttpGet("/getSpotDetails")]
        public async Task<ActionResult> GetSpotDetails()
        {
            

            SpotModel spot = new SpotModel(httpClient);
            var response = await spot.OnGet();

            return Ok(response);
        }

        [HttpGet("/getInterestDetails")]
        public async Task<ActionResult> GetInterestDetails()
        {


            InterestModel interest = new InterestModel(httpClient);
            var response = await interest.OnGet();

            return Ok(response);
        }
    }
}
