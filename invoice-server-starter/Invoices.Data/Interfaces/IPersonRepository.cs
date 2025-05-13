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

namespace Invoices.Data.Interfaces;

/// <summary>
/// Repository interface for person-related database operations.
/// </summary>
public interface IPersonRepository : IBaseRepository<Person>
{
    /// <summary>
    /// Gets all persons based on their hidden status.
    /// </summary>
    /// <param name="hidden">If true, returns hidden persons; otherwise, returns visible persons</param>
    IList<Person> GetAllByHidden(bool hidden);

    /// <summary>
    /// Selects a person by ID only if it's not hidden.
    /// </summary>
    /// <returns>The person if found and not hidden; otherwise, null</returns>
    Person? Select(ulong id);
}