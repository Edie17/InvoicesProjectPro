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
        [JsonPropertyName("_id")]
        public ulong InvoiceId { get; set; }

        public int InvoiceNumber { get; set; }

        public DateTime Issued { get; set; }

        public DateTime DueDate { get; set; }

        public string Product { get; set; } = "";

        public decimal Price { get; set; }

        public int Vat { get; set; }

        public string Note { get; set; } = "";

        public PersonDto? Seller { get; set; }
        public PersonDto? Buyer { get; set; }
    }
}