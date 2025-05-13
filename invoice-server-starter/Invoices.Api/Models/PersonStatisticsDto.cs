namespace Invoices.Api.Models
{
    /// <summary>
    /// Data transfer object for person-related statistics.
    /// </summary>
    public class PersonStatisticsDto
    {
        /// <summary>
        /// Name of the company or person.
        /// </summary>
        public string CompanyName { get; set; } = "";

        /// <summary>
        /// Total number of invoices associated with this person.
        /// </summary>
        public int TotalInvoices { get; set; }

        /// <summary>
        /// Total monetary amount of all invoices associated with this person.
        /// </summary>
        public decimal TotalAmount { get; set; }
    }
}