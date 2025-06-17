using Invoices.Data;
using Invoices.Data.Models;

namespace Invoices.Api;

/// <summary>
/// Seeds the database with sample data if it is empty.
/// </summary>
public static class DatabaseSeeder
{
    public static void Seed(InvoicesDbContext context)
    {
        bool hasPersons = context.Persons != null && context.Persons.Any();
        bool hasInvoices = context.Set<Invoice>().Any();

        if (hasPersons && hasInvoices)
            return;

        var persons = new List<Person>
        {
            new Person
            {
                Name = "Alfa s.r.o.",
                IdentificationNumber = "12345678",
                TaxNumber = "CZ12345678",
                AccountNumber = "123456789",
                BankCode = "0800",
                Iban = "CZ6508000000001234567890",
                Telephone = "+420 601 111 111",
                Mail = "info@alfa.cz",
                Street = "Václavské náměstí 1",
                Zip = "11000",
                City = "Praha",
                Note = "",
                Country = Country.CZECHIA,
                Hidden = false
            },
            new Person
            {
                Name = "Beta Technologies a.s.",
                IdentificationNumber = "23456789",
                TaxNumber = "CZ23456789",
                AccountNumber = "987654321",
                BankCode = "0100",
                Iban = "CZ6501000000009876543210",
                Telephone = "+420 602 222 222",
                Mail = "kontakt@beta.cz",
                Street = "Náměstí Svobody 5",
                Zip = "60200",
                City = "Brno",
                Note = "",
                Country = Country.CZECHIA,
                Hidden = false
            },
            new Person
            {
                Name = "Gamma Consulting s.r.o.",
                IdentificationNumber = "34567890",
                TaxNumber = "CZ34567890",
                AccountNumber = "111222333",
                BankCode = "2010",
                Iban = "CZ6520100000001112223330",
                Telephone = "+420 603 333 333",
                Mail = "office@gamma.cz",
                Street = "Hlavní třída 10",
                Zip = "70200",
                City = "Ostrava",
                Note = "VIP klient",
                Country = Country.CZECHIA,
                Hidden = false
            },
            new Person
            {
                Name = "Delta Solutions s.r.o.",
                IdentificationNumber = "45678901",
                TaxNumber = "CZ45678901",
                AccountNumber = "444555666",
                BankCode = "0300",
                Iban = "CZ6503000000004445556660",
                Telephone = "+420 604 444 444",
                Mail = "info@delta.cz",
                Street = "Masarykova 22",
                Zip = "37001",
                City = "České Budějovice",
                Note = "",
                Country = Country.CZECHIA,
                Hidden = false
            },
            new Person
            {
                Name = "Omega SK s.r.o.",
                IdentificationNumber = "56789012",
                TaxNumber = "SK56789012",
                AccountNumber = "777888999",
                BankCode = "0900",
                Iban = "SK3112000000198742637541",
                Telephone = "+421 905 555 555",
                Mail = "info@omegask.sk",
                Street = "Obchodná 15",
                Zip = "81106",
                City = "Bratislava",
                Note = "Slovenský partner",
                Country = Country.SLOVAKIA,
                Hidden = false
            }
        };

        context.Persons!.AddRange(persons);
        context.SaveChanges();

        var invoices = new List<Invoice>
        {
            new Invoice
            {
                InvoiceNumber = 2024001,
                Issued = new DateTime(2024, 1, 10),
                DueDate = new DateTime(2024, 1, 24),
                Product = "Vývoj webové aplikace",
                Price = 85000,
                Vat = 21,
                Note = "Dle smlouvy č. 2024/001",
                SellerId = persons[0].PersonId,
                BuyerId = persons[1].PersonId
            },
            new Invoice
            {
                InvoiceNumber = 2024002,
                Issued = new DateTime(2024, 2, 5),
                DueDate = new DateTime(2024, 2, 19),
                Product = "IT konzultace",
                Price = 32000,
                Vat = 21,
                Note = "",
                SellerId = persons[2].PersonId,
                BuyerId = persons[0].PersonId
            },
            new Invoice
            {
                InvoiceNumber = 2024003,
                Issued = new DateTime(2024, 3, 1),
                DueDate = new DateTime(2024, 3, 15),
                Product = "Správa serverů – Q1",
                Price = 18500,
                Vat = 21,
                Note = "Měsíční paušál",
                SellerId = persons[1].PersonId,
                BuyerId = persons[3].PersonId
            },
            new Invoice
            {
                InvoiceNumber = 2024004,
                Issued = new DateTime(2024, 4, 12),
                DueDate = new DateTime(2024, 4, 26),
                Product = "Dodávka hardware",
                Price = 142000,
                Vat = 21,
                Note = "Faktura za notebooky a periférie",
                SellerId = persons[3].PersonId,
                BuyerId = persons[2].PersonId
            },
            new Invoice
            {
                InvoiceNumber = 2024005,
                Issued = new DateTime(2024, 5, 20),
                DueDate = new DateTime(2024, 6, 3),
                Product = "Marketingová kampaň",
                Price = 55000,
                Vat = 21,
                Note = "",
                SellerId = persons[4].PersonId,
                BuyerId = persons[0].PersonId
            },
            new Invoice
            {
                InvoiceNumber = 2024006,
                Issued = new DateTime(2024, 6, 8),
                DueDate = new DateTime(2024, 6, 22),
                Product = "SEO optimalizace",
                Price = 28000,
                Vat = 21,
                Note = "3 měsíce práce",
                SellerId = persons[2].PersonId,
                BuyerId = persons[4].PersonId
            },
            new Invoice
            {
                InvoiceNumber = 2024007,
                Issued = new DateTime(2024, 8, 14),
                DueDate = new DateTime(2024, 8, 28),
                Product = "Vývoj mobilní aplikace",
                Price = 210000,
                Vat = 21,
                Note = "Milestone 1 - design a prototyp",
                SellerId = persons[1].PersonId,
                BuyerId = persons[0].PersonId
            },
            new Invoice
            {
                InvoiceNumber = 2024008,
                Issued = new DateTime(2024, 10, 3),
                DueDate = new DateTime(2024, 10, 17),
                Product = "Cloudové služby – Q3",
                Price = 9900,
                Vat = 21,
                Note = "AWS provoz",
                SellerId = persons[0].PersonId,
                BuyerId = persons[3].PersonId
            },
            new Invoice
            {
                InvoiceNumber = 2025001,
                Issued = new DateTime(2025, 1, 7),
                DueDate = new DateTime(2025, 1, 21),
                Product = "Roční podpora a maintenance",
                Price = 48000,
                Vat = 21,
                Note = "SLA smlouva 2025",
                SellerId = persons[2].PersonId,
                BuyerId = persons[1].PersonId
            },
            new Invoice
            {
                InvoiceNumber = 2025002,
                Issued = new DateTime(2025, 3, 15),
                DueDate = new DateTime(2025, 3, 29),
                Product = "Školení zaměstnanců",
                Price = 22000,
                Vat = 21,
                Note = "2 dny, 10 účastníků",
                SellerId = persons[4].PersonId,
                BuyerId = persons[2].PersonId
            }
        };

        context.Set<Invoice>().AddRange(invoices);
        context.SaveChanges();
    }
}
