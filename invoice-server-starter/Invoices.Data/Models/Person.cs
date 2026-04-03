/*  _____ _______         _                      _
 * |_   _|__   __|       | |                    | |
 *   | |    | |_ __   ___| |___      _____  _ __| | __  ___ ____
 *   | |    | | '_ \ / _ \ __\ \ /\ / / _ \| '__| |/ / / __|_  /
 *  _| |_   | | | | |  __/ |_ \ V  V / (_) | |  |   < | (__ / /
 * |_____|  |_|_| |_|\___|\__| \_/\_/ \___/|_|  |_|\_(_)___/___|
 *
 *                      ___ ___ ___
 *                     | . |  _| . |  LICENCE
 *                     |  _|_| |___|
 *                     |_|
 *
 *    REKVALIFIKAČNÍ KURZY  <>  PROGRAMOVÁNÍ  <>  IT KARIÉRA
 *
 * Tento zdrojový kód je součástí profesionálních IT kurzů na
 * WWW.ITNETWORK.CZ
 *
 * Kód spadá pod licenci PRO obsahu a vznikl díky podpoře
 * našich členů. Je určen pouze pro osobní užití a nesmí být šířen.
 * Více informací na http://www.itnetwork.cz/licence
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Invoices.Data.Models;

/// <summary>
/// Represents a person (company or individual) stored in the database.
/// Persons are soft-deleted by setting <see cref="Hidden"/> to true.
/// </summary>
public class Person
{
    /// <summary>Auto-generated primary key.</summary>
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public ulong PersonId { get; set; }

    /// <summary>Full name of the person or company.</summary>
    [Required]
    public string Name { get; set; } = "";

    /// <summary>Company identification number (IČO).</summary>
    [Required]
    public string IdentificationNumber { get; set; } = "";

    /// <summary>Tax identification number (DIČ).</summary>
    [Required]
    public string TaxNumber { get; set; } = "";

    /// <summary>Bank account number.</summary>
    [Required]
    public string AccountNumber { get; set; } = "";

    /// <summary>Bank code.</summary>
    [Required]
    public string BankCode { get; set; } = "";

    /// <summary>International Bank Account Number (IBAN).</summary>
    [Required]
    public string Iban { get; set; } = "";

    /// <summary>Contact telephone number.</summary>
    [Required]
    public string Telephone { get; set; } = "";

    /// <summary>Contact e-mail address.</summary>
    [Required]
    public string Mail { get; set; } = "";

    /// <summary>Street address.</summary>
    [Required]
    public string Street { get; set; } = "";

    /// <summary>Postal (ZIP) code.</summary>
    [Required]
    public string Zip { get; set; } = "";

    /// <summary>City.</summary>
    [Required]
    public string City { get; set; } = "";

    /// <summary>Optional note or additional information.</summary>
    [Required]
    public string Note { get; set; } = "";

    /// <summary>Country of the person or company.</summary>
    [Required]
    public Country Country { get; set; }

    /// <summary>
    /// Soft-delete flag. When true the person is considered deleted
    /// but remains in the database for referential integrity.
    /// </summary>
    [Required]
    public bool Hidden { get; set; } = false;

    /// <summary>Invoices in which this person is the buyer.</summary>
    public virtual List<Invoice> Purchases { get; set; } = new List<Invoice>();

    /// <summary>Invoices in which this person is the seller.</summary>
    public virtual List<Invoice> Sales { get; set; } = new List<Invoice> { };
}