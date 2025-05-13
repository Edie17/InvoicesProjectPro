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

namespace Invoices.Data.Interfaces;

/// <summary>
/// Generic interface for basic repository operations.
/// </summary>
/// <typeparam name="TEntity">Entity type the repository works with</typeparam>
public interface IBaseRepository<TEntity> where TEntity : class
{
    /// <summary>
    /// Gets all entities from the repository.
    /// </summary>
    IList<TEntity> GetAll();

    /// <summary>
    /// Finds an entity by its ID.
    /// </summary>
    /// <returns>The entity or null if not found</returns>
    TEntity? FindById(ulong id);

    /// <summary>
    /// Inserts a new entity into the repository.
    /// </summary>
    /// <returns>The inserted entity with generated ID</returns>
    TEntity Insert(TEntity entity);

    /// <summary>
    /// Updates an existing entity.
    /// </summary>
    /// <returns>The updated entity</returns>
    TEntity Update(TEntity entity);

    /// <summary>
    /// Deletes an entity by its ID.
    /// </summary>
    void Delete(ulong id);

    /// <summary>
    /// Checks if an entity with the specified ID exists.
    /// </summary>
    /// <returns>True if entity exists, otherwise false</returns>
    bool ExistsWithId(ulong id);
}