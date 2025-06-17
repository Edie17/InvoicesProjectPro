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

            if (addedInvoice.BuyerId is not null)
                addedInvoice.Buyer = personRepository.FindById((ulong)addedInvoice.BuyerId);
            if (addedInvoice.SellerId is not null)
                addedInvoice.Seller = personRepository.FindById((ulong)addedInvoice.SellerId);

            return mapper.Map<InvoiceDto>(addedInvoice);
        }

        /// <summary>
        /// Gets all invoices with optional filtering.
        /// </summary>
        public IEnumerable<InvoiceDto> GetAllInvoices(
            ulong? sellerId = null,
            ulong? buyerId = null,
            string? product = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            int? limit = null,
            int? invoiceNumber = null,
            DateTime? issuedFrom = null,
            DateTime? issuedTo = null)
        {
            IQueryable<Invoice> query = invoiceRepository.GetAll().AsQueryable();

            if (sellerId.HasValue && sellerId.Value > 0)
                query = query.Where(i => i.SellerId == sellerId.Value);

            if (buyerId.HasValue && buyerId.Value > 0)
                query = query.Where(i => i.BuyerId == buyerId.Value);

            if (!string.IsNullOrWhiteSpace(product))
            {
                var productLower = product.Trim().ToLower();
                query = query.Where(i => i.Product.ToLower().Contains(productLower));
            }

            if (minPrice.HasValue && minPrice.Value >= 0)
                query = query.Where(i => i.Price >= minPrice.Value);

            if (maxPrice.HasValue && maxPrice.Value >= 0)
                query = query.Where(i => i.Price <= maxPrice.Value);

            if (invoiceNumber.HasValue && invoiceNumber.Value > 0)
                query = query.Where(i => i.InvoiceNumber == invoiceNumber.Value);

            if (issuedFrom.HasValue)
                query = query.Where(i => i.Issued >= issuedFrom.Value);

            if (issuedTo.HasValue)
                query = query.Where(i => i.Issued <= issuedTo.Value);

            if (limit.HasValue && limit.Value > 0)
                query = query.Take(limit.Value);

            return mapper.Map<IEnumerable<InvoiceDto>>(query.ToList());
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

        public IDictionary<ulong, decimal> GetRevenueByPersonIds(IEnumerable<ulong> personIds)
        {
            var idSet = new HashSet<ulong>(personIds);
            var result = idSet.ToDictionary(id => id, id => 0m);

            foreach (var invoice in invoiceRepository.GetAll())
            {
                if (invoice.SellerId.HasValue && idSet.Contains(invoice.SellerId.Value))
                    result[invoice.SellerId.Value] += invoice.Price;
                if (invoice.BuyerId.HasValue && idSet.Contains(invoice.BuyerId.Value))
                    result[invoice.BuyerId.Value] += invoice.Price;
            }

            return result;
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