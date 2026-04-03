using System;

namespace Invoices.Api.Models
{
    /// <summary>
    /// Data transfer object for invoice filtering parameters.
    /// All properties are optional; only provided values are applied as filters.
    /// </summary>
    public class InvoiceFilterDto
    {
        /// <summary>Filter by exact invoice number.</summary>
        public int? InvoiceNumber { get; set; }

        /// <summary>Filter by product name (partial, case-insensitive match).</summary>
        public string? Product { get; set; }

        /// <summary>Filter invoices with price greater than or equal to this value.</summary>
        public decimal? MinPrice { get; set; }

        /// <summary>Filter invoices with price less than or equal to this value.</summary>
        public decimal? MaxPrice { get; set; }

        /// <summary>Filter invoices issued on or after this date.</summary>
        public DateTime? IssuedFrom { get; set; }

        /// <summary>Filter invoices issued on or before this date.</summary>
        public DateTime? IssuedTo { get; set; }

        /// <summary>Filter by seller person ID.</summary>
        public ulong? SellerId { get; set; }

        /// <summary>Filter by buyer person ID.</summary>
        public ulong? BuyerId { get; set; }

        /// <summary>Maximum number of results to return.</summary>
        public int? Limit { get; set; }
    }
}