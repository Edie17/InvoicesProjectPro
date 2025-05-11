namespace Invoices.Api.Models
{
    public class StatisticsDto
    {
        public decimal CurrentYearSum { get; set; }
        public decimal AllTimeSum { get; set; }
        public int InvoicesCount { get; set; }
    }
}
