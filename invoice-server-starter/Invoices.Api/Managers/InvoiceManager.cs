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
    /// <summary>
    /// Implementation of invoice management operations.
    /// </summary>
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

            // Detach existing entity from context
            invoicesDbContext.Entry(existingInvoice).State = EntityState.Detached;

            // Create new entity with updated data
            Invoice invoice = mapper.Map<Invoice>(invoiceDto);
            invoice.InvoiceId = id;

            // Set Buyer/Seller relations
            invoice.BuyerId = invoiceDto.Buyer?.PersonId;
            invoice.SellerId = invoiceDto.Seller?.PersonId;
            invoice.Buyer = null;
            invoice.Seller = null;

            // Update
            Invoice updatedInvoice = invoiceRepository.Update(invoice);

            // Load fresh data about buyer and seller
            if (updatedInvoice.BuyerId.HasValue)
                updatedInvoice.Buyer = personRepository.FindById((ulong)updatedInvoice.BuyerId);
            if (updatedInvoice.SellerId.HasValue)
                updatedInvoice.Seller = personRepository.FindById((ulong)updatedInvoice.SellerId);

            return mapper.Map<InvoiceDto>(updatedInvoice);
        }

        /// <summary>
        /// Gets sales invoices where the person with the given identification number is the seller.
        /// </summary>
        public IEnumerable<InvoiceDto> GetSalesByIdentificationNumber(string identificationNumber)
        {
            // Find all versions of the person with the same identification number
            var persons = personRepository.GetAll()
                .Where(p => p.IdentificationNumber == identificationNumber)
                .Select(p => p.PersonId);

            // Find all invoices where these persons are sellers
            var invoices = invoiceRepository.GetAll()
                .Where(i => i.SellerId.HasValue && persons.Contains((ulong)i.SellerId));

            // Map to DTOs
            return mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        }

        /// <summary>
        /// Gets purchase invoices where the person with the given identification number is the buyer.
        /// </summary>
        public IEnumerable<InvoiceDto> GetPurchasesByIdentificationNumber(string identificationNumber)
        {
            // Find all versions of the person with the same identification number
            var persons = personRepository.GetAll()
                .Where(p => p.IdentificationNumber == identificationNumber)
                .Select(p => p.PersonId);

            // Find all invoices where these persons are buyers
            var invoices = invoiceRepository.GetAll()
                .Where(i => i.BuyerId.HasValue && persons.Contains((ulong)i.BuyerId));

            // Map to DTOs
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

        /// <summary>
        /// Gets overall invoice statistics.
        /// </summary>
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

            // Invoices where person is seller or buyer
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