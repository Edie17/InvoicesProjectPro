using Invoices.Api.Interfaces;
using Invoices.Api.Models;
using Invoices.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Invoices.Api.Controllers
{
    /// <summary>
    /// API controller for managing invoices in the system.
    /// </summary>
    [Route("api")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceManager invoiceManager;

        public InvoicesController(IInvoiceManager invoiceManager)
        {
            this.invoiceManager = invoiceManager;
        }

        /// <summary>
        /// Creates a new invoice.
        /// </summary>
        /// <returns>201 Created response with the created invoice</returns>
        [HttpPost("invoices")]
        public IActionResult AddInvoice([FromBody] InvoiceDto invoiceDto)
        {
            InvoiceDto? createdInvoice = invoiceManager.AddInvoice(invoiceDto);
            return StatusCode(StatusCodes.Status201Created, createdInvoice);
        }

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
        [HttpGet("invoices")]
        public IEnumerable<InvoiceDto> GetAllInvoices(
            [FromQuery] ulong? sellerId = null,
            [FromQuery] ulong? buyerId = null,
            [FromQuery] string? product = null,
            [FromQuery] decimal? minPrice = null,
            [FromQuery] decimal? maxPrice = null,
            [FromQuery] int? limit = null,
            [FromQuery] int? invoiceNumber = null,
            [FromQuery] DateTime? issuedFrom = null,
            [FromQuery] DateTime? issuedTo = null)
        {
            return invoiceManager.GetAllInvoices(
                sellerId,
                buyerId,
                product,
                minPrice,
                maxPrice,
                limit,
                invoiceNumber,
                issuedFrom,
                issuedTo
            );
        }

        /// <summary>
        /// Gets a specific invoice by ID.
        /// </summary>
        /// <returns>The invoice or 404 if not found</returns>
        [HttpGet("invoices/{id}")]
        public IActionResult GetInvoice(ulong id)
        {
            var invoice = invoiceManager.GetInvoice(id);
            if (invoice == null)
                return NotFound();

            return Ok(invoice);
        }

        /// <summary>
        /// Updates an existing invoice.
        /// </summary>
        /// <returns>The updated invoice or 404 if not found</returns>
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

        /// <summary>
        /// Gets sales invoices by company identification number.
        /// </summary>
        [HttpGet("identification/{identificationNumber}/sales")]
        public IActionResult GetSalesByIdentificationNumber(string identificationNumber)
        {
            var invoices = invoiceManager.GetSalesByIdentificationNumber(identificationNumber);
            return Ok(invoices);
        }

        /// <summary>
        /// Gets purchase invoices by company identification number.
        /// </summary>
        [HttpGet("identification/{identificationNumber}/purchases")]
        public IActionResult GetPurchasesByIdentificationNumber(string identificationNumber)
        {
            var invoices = invoiceManager.GetPurchasesByIdentificationNumber(identificationNumber);
            return Ok(invoices);
        }

        /// <summary>
        /// Gets statistical data about invoices in the system.
        /// </summary>
        [HttpGet("invoices/statistics")]
        public IActionResult GetStatistics()
        {
            var statistics = invoiceManager.GetStatistics();
            return Ok(statistics);
        }
    }
}