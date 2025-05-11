using AutoMapper;
using Invoices.Api.Interfaces;
using Invoices.Api.Models;
using Invoices.Data;
using Invoices.Data.Interfaces;
using Invoices.Data.Models;
using Invoices.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;

namespace Invoices.Api.Managers
{
    public class InvoiceManager : IInvoiceManager
    {
        private readonly IInvoiceRepository invoiceRepository;
        private readonly IPersonRepository personRepository;
        private readonly IMapper mapper;
        private readonly InvoicesDbContext invoicesDbContext;

        public InvoiceManager(IInvoiceRepository invoiceRepository, IMapper mapper, IPersonRepository personRepository, InvoicesDbContext invoicesDbContext)
        {
            this.invoiceRepository = invoiceRepository;
            this.personRepository = personRepository;
            this.mapper = mapper;
            this.invoicesDbContext = invoicesDbContext;

        }

        public InvoiceDto AddInvoice(InvoiceDto invoiceDto)
        {
            Invoice invoice = mapper.Map<Invoice>(invoiceDto);
            invoice.BuyerId = invoice.Buyer?.PersonId;
            invoice.SellerId = invoice.Seller?.PersonId;
            invoice.Buyer = null;
            invoice.Seller = null;
            Invoice addedInvoice = invoiceRepository.Insert(invoice);

            if (invoice.BuyerId is not null) 
             invoice.Buyer = personRepository.FindById((ulong)invoice.BuyerId);
            if (invoice.SellerId is not null)
                invoice.Seller = personRepository.FindById((ulong)invoice.SellerId);

            InvoiceDto addedInvoiceDto = mapper.Map<InvoiceDto>(addedInvoice);

            return addedInvoiceDto;
        }

        public IEnumerable<InvoiceDto> GetAllInvoices()
        {
            IList<Invoice> invoice = invoiceRepository.GetAll();
            return mapper.Map<IList<InvoiceDto>>(invoice);

        }

        public InvoiceDto? GetInvoice(ulong id)
        {
            Invoice? invoice = invoiceRepository.FindById(id);
            return invoice != null ? mapper.Map<InvoiceDto>(invoice) : null;
        }

        public InvoiceDto? UpdateInvoice(ulong id, InvoiceDto invoiceDto)
        {
            var existingInvoice = invoiceRepository.FindById(id);
            if (existingInvoice == null)
                return null;

            // Odpojíme existující entitu z kontextu
            invoicesDbContext.Entry(existingInvoice).State = EntityState.Detached;

            // Vytvoříme novou entitu s aktualizovanými daty
            Invoice invoice = mapper.Map<Invoice>(invoiceDto);
            invoice.InvoiceId = id;

            // Nastavíme vazby na Buyer/Seller
            invoice.BuyerId = invoiceDto.Buyer?.PersonId;
            invoice.SellerId = invoiceDto.Seller?.PersonId;
            invoice.Buyer = null;
            invoice.Seller = null;

            // Aktualizujeme
            Invoice updatedInvoice = invoiceRepository.Update(invoice);

            // Načteme čerstvá data o kupujícím a prodávajícím
            if (updatedInvoice.BuyerId.HasValue)
                updatedInvoice.Buyer = personRepository.FindById((ulong)updatedInvoice.BuyerId);
            if (updatedInvoice.SellerId.HasValue)
                updatedInvoice.Seller = personRepository.FindById((ulong)updatedInvoice.SellerId);

            return mapper.Map<InvoiceDto>(updatedInvoice);
        }

        public IEnumerable<InvoiceDto> GetSalesByIdentificationNumber(string identificationNumber)
        {
            // Najdeme všechny verze osoby se stejným IČO
            var persons = personRepository.GetAll()
                .Where(p => p.IdentificationNumber == identificationNumber)
                .Select(p => p.PersonId);

            // Najdeme všechny faktury, kde jsou tyto osoby jako prodejci
            var invoices = invoiceRepository.GetAll()
                .Where(i => i.SellerId.HasValue && persons.Contains((ulong)i.SellerId));

            // Mapujeme na DTO
            return mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        }

        public IEnumerable<InvoiceDto> GetPurchasesByIdentificationNumber(string identificationNumber)
        {
            // Najdeme všechny verze osoby se stejným IČO
            var persons = personRepository.GetAll()
                .Where(p => p.IdentificationNumber == identificationNumber)
                .Select(p => p.PersonId);

            // Najdeme všechny faktury, kde jsou tyto osoby jako kupující
            var invoices = invoiceRepository.GetAll()
                .Where(i => i.BuyerId.HasValue && persons.Contains((ulong)i.BuyerId));

            // Mapujeme na DTO
            return mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        }

        public void DeleteInvoice(ulong id)
        {
            invoiceRepository.Delete(id);
        }

        public IEnumerable<InvoiceDto> GetInvoicesByPerson(ulong personId)
        {
            var person = personRepository.FindById(personId);
            if (person == null)
                return Enumerable.Empty<InvoiceDto>();

            var allInvoices = invoiceRepository.GetAll();
            var personInvoices = allInvoices.Where(i => i.BuyerId == personId || i.SellerId == personId);

            return mapper.Map<IEnumerable<InvoiceDto>>(personInvoices);
        }

        public StatisticsDto GetStatistics()
        {
            var allInvoices = invoiceRepository.GetAll();
            var currentYear = DateTime.Now.Year;

            return new StatisticsDto
            {
                CurrentYearSum = allInvoices
                                  .Where(i => i.Issued.Year == currentYear)
                                  .Sum(i => i.Price),
                AllTimeSum = allInvoices.Sum(i => i.Price),
                InvoicesCount = allInvoices.Count()
            };
        }

        public PersonStatisticsDto GetPersonStatistics(ulong personId)
        {
            var person = personRepository.FindById(personId);
            if (person == null)
                return new PersonStatisticsDto();

            var allInvoices = invoiceRepository.GetAll();

            // Faktury kde je osoba jako prodejce nebo kupující
            var personInvoices = allInvoices.Where(i => i.SellerId == personId || i.BuyerId == personId);

            return new PersonStatisticsDto
            {
                CompanyName = person.Name,
                TotalInvoices = personInvoices.Count(),
                TotalAmount = personInvoices.Sum(i => i.Price)
            };
        }
    }
}
