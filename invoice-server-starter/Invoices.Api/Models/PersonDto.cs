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

using Invoices.Data.Models;
using System.Text.Json.Serialization;

namespace Invoices.Api.Models;

/// <summary>
/// Data transfer object for persons.
/// </summary>
public class PersonDto
{
    /// <summary>Unique person identifier serialized as <c>_id</c>.</summary>
    [JsonPropertyName("_id")]
    public ulong PersonId { get; set; }

    /// <summary>Full name of the person or company.</summary>
    public string Name { get; set; } = "";

    /// <summary>Company identification number (IČO).</summary>
    public string IdentificationNumber { get; set; } = "";

    /// <summary>Tax identification number (DIČ).</summary>
    public string TaxNumber { get; set; } = "";

    /// <summary>Bank account number.</summary>
    public string AccountNumber { get; set; } = "";

    /// <summary>Bank code.</summary>
    public string BankCode { get; set; } = "";

    /// <summary>International Bank Account Number (IBAN).</summary>
    public string Iban { get; set; } = "";

    /// <summary>Contact telephone number.</summary>
    public string Telephone { get; set; } = "";

    /// <summary>Contact e-mail address.</summary>
    public string Mail { get; set; } = "";

    /// <summary>Street address.</summary>
    public string Street { get; set; } = "";

    /// <summary>Postal (ZIP) code.</summary>
    public string Zip { get; set; } = "";

    /// <summary>City.</summary>
    public string City { get; set; } = "";

    /// <summary>Optional note or additional information.</summary>
    public string Note { get; set; } = "";

    /// <summary>Country of the person or company.</summary>
    public Country Country { get; set; }
}