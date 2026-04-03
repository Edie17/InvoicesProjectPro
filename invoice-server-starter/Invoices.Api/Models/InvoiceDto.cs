using Invoices.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Invoices.Api.Models
{
    /// <summary>
    /// Data transfer object for invoices.
    /// </summary>
    public class InvoiceDto
    {
        /// <summary>Unique invoice identifier serialized as <c>_id</c>.</summary>
        [JsonPropertyName("_id")]
        public ulong InvoiceId { get; set; }

        /// <summary>Human-readable invoice number.</summary>
        public int InvoiceNumber { get; set; }

        /// <summary>Date the invoice was issued.</summary>
        public DateTime Issued { get; set; }

        /// <summary>Payment due date.</summary>
        public DateTime DueDate { get; set; }

        /// <summary>Name of the product or service being invoiced.</summary>
        public string Product { get; set; } = "";

        /// <summary>Invoice amount (excluding VAT).</summary>
        public decimal Price { get; set; }

        /// <summary>VAT rate in percent.</summary>
        public int Vat { get; set; }

        /// <summary>Optional note or description.</summary>
        public string Note { get; set; } = "";

        /// <summary>Seller of the invoice.</summary>
        public PersonDto? Seller { get; set; }

        /// <summary>Buyer of the invoice.</summary>
        public PersonDto? Buyer { get; set; }
    }
}