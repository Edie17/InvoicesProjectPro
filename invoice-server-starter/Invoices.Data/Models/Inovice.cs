using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.Data.Models
{
    /// <summary>
    /// Represents an invoice entity stored in the database.
    /// </summary>
    public class Invoice
    {
        /// <summary>Auto-generated primary key.</summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong InvoiceId { get; set; }

        /// <summary>Human-readable invoice number.</summary>
        [Required]
        public int InvoiceNumber { get; set; }

        /// <summary>Date the invoice was issued.</summary>
        [Required]
        public DateTime Issued { get; set; }

        /// <summary>Payment due date.</summary>
        [Required]
        public DateTime DueDate { get; set; }

        /// <summary>Name of the product or service being invoiced.</summary>
        [Required]
        public string Product { get; set; } = "";

        /// <summary>Invoice amount (excluding VAT).</summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>VAT rate in percent.</summary>
        [Required]
        public int Vat { get; set; }

        /// <summary>Optional note or description.</summary>
        [Required]
        public string Note { get; set; } = "";

        /// <summary>Foreign key referencing the seller.</summary>
        public ulong? SellerId { get; set; }

        /// <summary>Foreign key referencing the buyer.</summary>
        public ulong? BuyerId { get; set; }

        /// <summary>Navigation property to the seller person.</summary>
        [ForeignKey(nameof(SellerId))]
        public virtual Person? Seller { get; set; }

        /// <summary>Navigation property to the buyer person.</summary>
        [ForeignKey(nameof(BuyerId))]
        public virtual Person? Buyer { get; set; }
    }
}
