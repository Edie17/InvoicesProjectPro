using Invoices.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Interfaces
{
    public interface IInvoiceRepository : IBaseRepository<Invoice>
    {
        
    }
}
