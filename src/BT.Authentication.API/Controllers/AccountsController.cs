using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BT.Authentication.API.Repositories;
using BT.Shared.Domain.DTO;
using BT.Shared.Domain.DTO.Responses;
using BT.Shared.Domain.DTO.User;


namespace BT.Authentication.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountsController : ControllerBase
    {
        readonly IAccount _iaccount;
        readonly IConfiguration _configuration;

        public AccountsController(IAccount iaccount, IConfiguration configuration)
        {
            _iaccount = iaccount;
            _configuration = configuration;
        }

        //[AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<BaseAPIResponseDTO>> RegisterAsync(RegisterDTO dto)
        {
            var result = await _iaccount.RegisterAsync(dto);
            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var result = await _iaccount.VerifyEmail(token);
            return Ok(result);
        }

        //[AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<APIResponJWTDTO>> LoginAsync(LoginDTO dto)
        {
            var result = await _iaccount.LoginAsync(dto);
            return Ok(result);
        }


        //[AllowAnonymous]
        [HttpPost("refresh-token")]
        public async Task<ActionResult<APIResponJWTDTO>> RefreshToken(UserSession session)
        {
            var result = await _iaccount.RefreshTokenAsync(session);
            return Ok(result);
        }

        [HttpGet("weather")]
        public async Task<ActionResult<WeatherForecast[]>> GetWeatherForcast()
        {


            await Task.Delay(500);

            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
            var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = startDate.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            }).ToArray();

            return Ok(forecasts);
        }

        [HttpGet("stockquotes")]
        [Authorize(Roles = "EMPLOYEE_IT")]
        public async Task<ActionResult<List<StockQuoteDTO>>> GetStockQuote()
        {
            await Task.Delay(500);

            var quote = new List<StockQuoteDTO>()
            {
                new StockQuoteDTO(){ Time = DateTime.Now, Company = "Microsoft", Ticker = "MSFT", Price = "$407.59"},
                new StockQuoteDTO(){ Time = DateTime.Now, Company = "Tesla", Ticker = "TSLA", Price = "$244.60"},
                new StockQuoteDTO(){ Time = DateTime.Now, Company = "Apple", Ticker = "AAPL", Price = "$222.16"},
                new StockQuoteDTO(){ Time = DateTime.Now, Company = "Nvida", Ticker = "NVDA", Price = "$137.80"},
                new StockQuoteDTO(){ Time = DateTime.Now, Company = "Occidental Petroleum", Ticker = "OXY", Price = "$50.24"},
                new StockQuoteDTO(){ Time = DateTime.Now, Company = "Exxon Mobil Corporation", Ticker = "XOM", Price = "$118.26"},
                new StockQuoteDTO(){ Time = DateTime.Now, Company = "JPMorgan Chase & Co.", Ticker = "JPM", Price = "$220.79"},
                new StockQuoteDTO(){ Time = DateTime.Now, Company = "The Goldman Sachs Group, Inc.", Ticker = "GS", Price = "$511.77"},
                new StockQuoteDTO(){ Time = DateTime.Now, Company = "Johnson & Johnson ", Ticker = "JNJ", Price = "$158.58"},
                new StockQuoteDTO(){ Time = DateTime.Now, Company = "McDonald's Corporation", Ticker = "MCD", Price = "$292.91"}
            };

            return quote.ToList();
        }

    }
}
