using Invoices.Api.Models;

namespace Invoices.Api.Interfaces
{
    /// <summary>
    /// Service interface for invoice management operations.
    /// </summary>
    public interface IInvoiceManager
    {
        InvoiceDto AddInvoice(InvoiceDto invoiceDto);

        /// <summary>
        /// Gets all invoices with optional filtering.
        /// </summary>
        /// <param name="sellerId">Filter by seller ID</param>
        /// <param name="buyerId">Filter by buyer ID</param>
        /// <param name="product">Filter by product name (partial match)</param>
        /// <param name="minPrice">Filter by minimum price</param>
        /// <param name="maxPrice">Filter by maximum price</param>
        /// <param name="limit">Limit number of results</param>
        /// <returns>Filtered list of invoices</returns>
        IEnumerable<InvoiceDto> GetAllInvoices(
            ulong? sellerId = null,
            ulong? buyerId = null,
            string? product = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? limit = null,
            int? invoiceNumber = null,
            DateTime? issuedFrom = null,
            DateTime? issuedTo = null);

        /// <summary>
        /// Gets an invoice by ID.
        /// </summary>
        /// <returns>The invoice or null if not found</returns>
        InvoiceDto? GetInvoice(ulong id);

        /// <summary>
        /// Updates an existing invoice.
        /// </summary>
        /// <returns>The updated invoice or null if not found</returns>
        InvoiceDto? UpdateInvoice(ulong id, InvoiceDto invoiceDto);

        void DeleteInvoice(ulong id);
        IEnumerable<InvoiceDto> GetInvoicesByPerson(ulong personId);
        StatisticsDto GetStatistics();
        PersonStatisticsDto GetPersonStatistics(ulong personId);

        /// <summary>
        /// Gets sales invoices where the person with the given identification number is the seller.
        /// </summary>
        IEnumerable<InvoiceDto> GetSalesByIdentificationNumber(string identificationNumber);

        /// <summary>
        /// Gets total invoice amounts grouped by person ID (seller or buyer) in a single query.
        /// </summary>
        IDictionary<ulong, decimal> GetRevenueByPersonIds(IEnumerable<ulong> personIds);

        /// <summary>
        /// Gets purchase invoices where the person with the given identification number is the buyer.
        /// </summary>
        IEnumerable<InvoiceDto> GetPurchasesByIdentificationNumber(string identificationNumber);
    }
}