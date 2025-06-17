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

using AutoMapper;
using Invoices.Api.Interfaces;
using Invoices.Api.Models;
using Invoices.Data.Interfaces;
using Invoices.Data.Models;

namespace Invoices.Api.Managers;

/// <summary>
/// Implementation of person management operations.
/// </summary>
public class PersonManager : IPersonManager
{
    private readonly IPersonRepository personRepository;
    private readonly IMapper mapper;

    public PersonManager(IPersonRepository personRepository, IMapper mapper)
    {
        this.personRepository = personRepository;
        this.mapper = mapper;
    }

    public IList<PersonDto> GetAllPersons()
    {
        IList<Person> persons = personRepository.GetAllByHidden(false);
        return mapper.Map<IList<PersonDto>>(persons);
    }

    public PersonDto? GetPerson(ulong personId)
    {
        Person? person = personRepository.FindById(personId);

        if (person is null || person.Hidden)
            return null;

        return mapper.Map<PersonDto>(person);
    }

    public PersonDto AddPerson(PersonDto personDto)
    {
        Person person = mapper.Map<Person>(personDto);
        person.PersonId = default;
        Person addedPerson = personRepository.Insert(person);

        return mapper.Map<PersonDto>(addedPerson);
    }

    public void DeletePerson(ulong personId)
    {
        HidePerson(personId);
    }

    /// <summary>
    /// Hides a person rather than deleting it to maintain referential integrity.
    /// </summary>
    /// <returns>The hidden person or null if not found</returns>
    private Person? HidePerson(ulong personId)
    {
        Person? person = personRepository.FindById(personId);

        if (person is null)
            return null;

        person.Hidden = true;
        return personRepository.Update(person);
    }

    /// <summary>
    /// Updates a person by creating a new version with the updated data and hiding the old one.
    /// This approach preserves history of changes.
    /// </summary>
    public PersonDto? UpdatePerson(ulong id, PersonDto personDto)
    {
        // Find existing person by ID
        Person? existingPerson = personRepository.Select(id);
        if (existingPerson == null)
        {
            return null; // Person not found
        }

        // Check if identification number in personDto is different from existing person
        if (existingPerson.IdentificationNumber != personDto.IdentificationNumber)
        {
            throw new InvalidOperationException("Cannot change identification number of an existing person.");
        }

        // Hide existing person
        existingPerson.Hidden = true;
        personRepository.Update(existingPerson);

        // Create new person with updated values
        Person newPerson = mapper.Map<Person>(personDto);
        // Set ID to 0 to generate a new ID
        newPerson.PersonId = 0;
        Person addedPerson = personRepository.Insert(newPerson);

        return mapper.Map<PersonDto>(addedPerson);
    }
}