using Invoices.Data.Interfaces;
using Invoices.Data.Models;

namespace Invoices.Data.Repositories;

public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(InvoicesDbContext invoicesDbContext) : base(invoicesDbContext)
    {
    }
  
}