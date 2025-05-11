namespace Invoices.Api.Models
{
    public class PersonStatisticsDto
    {
        public string CompanyName { get; set; } = "";
        public int TotalInvoices { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
