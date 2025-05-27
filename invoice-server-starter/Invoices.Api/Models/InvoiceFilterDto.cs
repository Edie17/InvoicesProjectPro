using System;

namespace Invoices.Api.Models
{
    /// <summary>
    /// Data transfer object for invoice filtering parameters.
    /// </summary>
    public class InvoiceFilterDto
    {
        
        public int? InvoiceNumber { get; set; }

        
        public string? Product { get; set; }

        public decimal? MinPrice { get; set; }

        
        public decimal? MaxPrice { get; set; }

       
        public DateTime? IssuedFrom { get; set; }

    
        public DateTime? IssuedTo { get; set; }

       
        public ulong? SellerId { get; set; }

       
        public ulong? BuyerId { get; set; }

       
        public int? Limit { get; set; }
    }
}