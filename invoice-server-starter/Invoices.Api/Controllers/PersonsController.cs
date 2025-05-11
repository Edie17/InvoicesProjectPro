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

[Route("api")]
[ApiController]
public class PersonsController : ControllerBase
{
    private readonly IPersonManager personManager;
    private readonly IInvoiceManager invoiceManager; // Přidat tuto instanci

    // Upravit konstruktor pro přijetí obou závislostí
    public PersonsController(IPersonManager personManager, IInvoiceManager invoiceManager)
    {
        this.personManager = personManager;
        this.invoiceManager = invoiceManager; // Přiřadit injektovanou instanci
    }


    [HttpGet("persons")]
    public IEnumerable<PersonDto> GetPersons()
    {
        return personManager.GetAllPersons();
    }

    [HttpGet("persons/{personId}")]
    public IActionResult GetPerson(ulong personId)
    {
        PersonDto? person = personManager.GetPerson(personId);

        if (person is null)
            return NotFound();

        return Ok(person);
    }

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

    [HttpGet("persons/statistics")]
    public IActionResult GetPersonsStatistics()
    {
        // Získat všechny osoby
        var persons = personManager.GetAllPersons();

        // Výsledný seznam pro statistiky osob
        var personsStatistics = new List<object>();

        // Pro každou osobu získat statistiky
        foreach (var person in persons)
        {
            // Získat statistiky pro danou osobu
            var statistics = invoiceManager.GetPersonStatistics(person.PersonId);

            // Přidat do výsledku ve formátu dle dokumentace
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