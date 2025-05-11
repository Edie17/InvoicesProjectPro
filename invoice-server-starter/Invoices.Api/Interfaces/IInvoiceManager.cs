using Invoices.Api.Models;

namespace Invoices.Api.Interfaces
{
    public interface IInvoiceManager
    {
        InvoiceDto AddInvoice(InvoiceDto invoiceDto);
        IEnumerable<InvoiceDto> GetAllInvoices();
        InvoiceDto? GetInvoice(ulong id);
        InvoiceDto? UpdateInvoice(ulong id, InvoiceDto invoiceDto);
        void DeleteInvoice(ulong id);
        IEnumerable<InvoiceDto> GetInvoicesByPerson(ulong personId);
        StatisticsDto GetStatistics();
        PersonStatisticsDto GetPersonStatistics(ulong personId);
        IEnumerable<InvoiceDto> GetSalesByIdentificationNumber(string identificationNumber);
        IEnumerable<InvoiceDto> GetPurchasesByIdentificationNumber(string identificationNumber);

    }
}
