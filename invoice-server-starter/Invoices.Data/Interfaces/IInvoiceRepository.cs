using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Interfaces
{
    /// <summary>
    /// Repository interface for invoice-related database operations.
    /// </summary>
    public interface IInvoiceRepository : IBaseRepository<Invoice>
    {
        // Currently inherits all standard repository operations from IBaseRepository
    }
}