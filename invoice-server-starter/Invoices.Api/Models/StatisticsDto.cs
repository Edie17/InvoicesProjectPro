namespace Invoices.Api.Models
{
    /// <summary>
    /// Data transfer object for invoice statistics.
    /// </summary>
    public class StatisticsDto
    {
        /// <summary>
        /// Total sum of invoices in the current year.
        /// </summary>
        public decimal CurrentYearSum { get; set; }

        /// <summary>
        /// Total sum of all invoices in the system.
        /// </summary>
        public decimal AllTimeSum { get; set; }

        /// <summary>
        /// Total number of invoices in the system.
        /// </summary>
        public int InvoicesCount { get; set; }
    }
}