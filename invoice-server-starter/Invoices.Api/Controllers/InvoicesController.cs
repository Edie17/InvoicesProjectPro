using Invoices.Api.Interfaces;
using Invoices.Api.Models;
using Invoices.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Invoices.Api.Controllers
{

    [Route("api")]
    [ApiController]

    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceManager invoiceManager;


        public InvoicesController(IInvoiceManager invoiceManager)
        {
            this.invoiceManager = invoiceManager;
        }



        [HttpPost("invoices")]
        public IActionResult AddInvoice([FromBody] InvoiceDto invoiceDto)
        {
            InvoiceDto? createdInvoice = invoiceManager.AddInvoice(invoiceDto);
            return StatusCode(StatusCodes.Status201Created, createdInvoice);
        }

        [HttpGet("invoices")]
        public IEnumerable<InvoiceDto> GetAllInvoices()
        {
            return invoiceManager.GetAllInvoices();
        }

        [HttpGet("invoices/{id}")]
        public IActionResult GetInvoice(ulong id)
        {
            var invoice = invoiceManager.GetInvoice(id);
            if (invoice == null)
                return NotFound();

            return Ok(invoice);
        }

        [HttpPut("invoices/{id}")]
        public IActionResult UpdateInvoice(ulong id, [FromBody] InvoiceDto invoiceDto)
        {
            var updatedInvoice = invoiceManager.UpdateInvoice(id, invoiceDto);
            if (updatedInvoice == null)
                return NotFound();

            return Ok(updatedInvoice);
        }

        [HttpDelete("invoices/{id}")]
        public IActionResult DeleteInvoice(ulong id)
        {
            invoiceManager.DeleteInvoice(id);
            return NoContent();
        }

        [HttpGet("persons/{personId}/invoices")]
        public IActionResult GetPersonInvoices(ulong personId)
        {
            var invoices = invoiceManager.GetInvoicesByPerson(personId);
            return Ok(invoices);
        }

        [HttpGet("identification/{identificationNumber}/sales")]
        public IActionResult GetSalesByIdentificationNumber(string identificationNumber)
        {
            var invoices = invoiceManager.GetSalesByIdentificationNumber(identificationNumber);
            return Ok(invoices);
        }

        [HttpGet("identification/{identificationNumber}/purchases")]
        public IActionResult GetPurchasesByIdentificationNumber(string identificationNumber)
        {
            var invoices = invoiceManager.GetPurchasesByIdentificationNumber(identificationNumber);
            return Ok(invoices);
        }

        [HttpGet("invoices/statistics")]
        public IActionResult GetStatistics()
        {
            var statistics = invoiceManager.GetStatistics();
            return Ok(statistics);
        }

        
    }
}