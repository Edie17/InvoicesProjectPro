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

using Invoices.Api.Models;

namespace Invoices.Api.Interfaces;

/// <summary>
/// Service interface for person management operations.
/// </summary>
public interface IPersonManager
{
    /// <summary>Gets all visible (non-hidden) persons.</summary>
    IList<PersonDto> GetAllPersons();

    /// <summary>Creates and persists a new person.</summary>
    PersonDto AddPerson(PersonDto personDto);

    /// <summary>Soft-deletes a person by hiding them.</summary>
    void DeletePerson(ulong personId);

    /// <summary>
    /// Gets a person by ID.
    /// </summary>
    /// <returns>The person or null if not found</returns>
    PersonDto? GetPerson(ulong personId);

    /// <summary>
    /// Updates an existing person.
    /// </summary>
    /// <returns>The updated person or null if not found</returns>
    PersonDto? UpdatePerson(ulong id, PersonDto personDto);
}