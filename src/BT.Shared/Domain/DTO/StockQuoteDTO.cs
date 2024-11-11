
namespace BT.Shared.Domain.DTO
{
    public class StockQuoteDTO
    {
        public DateTime? Time { get; set; }
        public string? Company { get; set; }
        public string? Ticker { get; set; }
        public string? Price { get; set; }
    }
}
