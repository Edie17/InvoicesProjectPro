using Invoices.Data.Interfaces;
using Invoices.Data.Models;

namespace Invoices.Data.Repositories;

/// <summary>
/// Repository implementation for invoice-related database operations.
/// </summary>
public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(InvoicesDbContext invoicesDbContext) : base(invoicesDbContext)
    {
    }
}