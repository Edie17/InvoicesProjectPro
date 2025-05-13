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

using Invoices.Api.Interfaces;
using Invoices.Api.Managers;
using Invoices.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Invoices.Api.Controllers;

/// <summary>
/// API controller for managing persons in the system.
/// </summary>
[Route("api")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly IPersonManager personManager;
    private readonly IInvoiceManager invoiceManager;

    public PersonsController(IPersonManager personManager, IInvoiceManager invoiceManager)
    {
        this.personManager = personManager;
        this.invoiceManager = invoiceManager;
    }

    [HttpGet("persons")]
    public IEnumerable<PersonDto> GetPersons()
    {
        return personManager.GetAllPersons();
    }

    /// <summary>
    /// Gets a specific person by ID.
    /// </summary>
    /// <returns>The person or 404 if not found</returns>
    [HttpGet("persons/{personId}")]
    public IActionResult GetPerson(ulong personId)
    {
        PersonDto? person = personManager.GetPerson(personId);

        if (person is null)
            return NotFound();

        return Ok(person);
    }

    /// <summary>
    /// Creates a new person.
    /// </summary>
    /// <returns>201 Created response with the created person</returns>
    [HttpPost("persons")]
    public IActionResult AddPerson([FromBody] PersonDto personDto)
    {
        PersonDto? createdPerson = personManager.AddPerson(personDto);
        return StatusCode(StatusCodes.Status201Created, createdPerson);
    }

    [HttpDelete("persons/{personId}")]
    public IActionResult DeletePerson(uint personId)
    {
        personManager.DeletePerson(personId);
        return NoContent();
    }

    /// <summary>
    /// Updates an existing person.
    /// </summary>
    /// <returns>The updated person or 404 if not found</returns>
    [HttpPut("persons/{id}")]
    public IActionResult UpdatePerson(ulong id, [FromBody] PersonDto personDto)
    {
        PersonDto? updatedPerson = personManager.UpdatePerson(id, personDto);
        if (updatedPerson == null)
        {
            return NotFound();
        }
        return Ok(updatedPerson);
    }

    /// <summary>
    /// Gets statistical data about persons and their revenues.
    /// </summary>
    [HttpGet("persons/statistics")]
    public IActionResult GetPersonsStatistics()
    {
        // Get all persons
        var persons = personManager.GetAllPersons();

        // Result list for person statistics
        var personsStatistics = new List<object>();

        // For each person, get their statistics
        foreach (var person in persons)
        {
            // Get statistics for the current person
            var statistics = invoiceManager.GetPersonStatistics(person.PersonId);

            // Add to the result in the format specified in documentation
            personsStatistics.Add(new
            {
                personId = person.PersonId,
                personName = person.Name,
                revenue = statistics.TotalAmount
            });
        }

        return Ok(personsStatistics);
    }
}