using Invoices.Api.Models;

namespace Invoices.Api.Interfaces
{
    /// <summary>
    /// Service interface for invoice management operations.
    /// </summary>
    public interface IInvoiceManager
    {
        InvoiceDto AddInvoice(InvoiceDto invoiceDto);
        IEnumerable<InvoiceDto> GetAllInvoices();

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
        /// Gets purchase invoices where the person with the given identification number is the buyer.
        /// </summary>
        IEnumerable<InvoiceDto> GetPurchasesByIdentificationNumber(string identificationNumber);
    }
}