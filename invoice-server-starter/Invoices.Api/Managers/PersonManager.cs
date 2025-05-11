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

        if (person is null)
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

    public void DeletePerson(uint personId)
    {
        HidePerson(personId);
    }

    private Person? HidePerson(uint personId)
    {
        Person? person = personRepository.FindById(personId);

        if (person is null)
            return null;

        person.Hidden = true;
        return personRepository.Update(person);
    }

    public PersonDto? UpdatePerson(ulong id, PersonDto personDto)
    {
        // Najdi existující osobu podle ID
        Person? existingPerson = personRepository.Select(id);
        if (existingPerson == null)
        {
            return null; // Osoba nenalezena, vrať null
        }

        // Zkontroluj, zda se IČ v personDto liší od IČ existující osoby
        if (existingPerson.IdentificationNumber != personDto.IdentificationNumber)
        {
            throw new InvalidOperationException("Nelze změnit IČ existující osoby.");
        }

        // Skryj existující osobu
        existingPerson.Hidden = true;
        personRepository.Update(existingPerson);

        // Vytvoř novou osobu s upravenými hodnotami
        Person newPerson = mapper.Map<Person>(personDto);
        // Nastav ID na 0, aby se vygenerovalo nové ID
        newPerson.PersonId = 0;
        Person addedPerson = personRepository.Insert(newPerson);

        return mapper.Map<PersonDto>(addedPerson);
    }

}