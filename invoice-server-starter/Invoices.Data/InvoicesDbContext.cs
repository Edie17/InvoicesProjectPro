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
using Microsoft.EntityFrameworkCore;

namespace Invoices.Data;

/// <summary>
/// Entity Framework DbContext for working with the invoices database.
/// </summary>
public class InvoicesDbContext : DbContext
{
    public DbSet<Person>? Persons { get; set; } = null!;

    public InvoicesDbContext(DbContextOptions<InvoicesDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// Configures entity relationships and properties on model creation.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure price as decimal with specific precision
        modelBuilder
            .Entity<Invoice>()
            .Property(x => x.Price)
            .HasColumnType("decimal(10,2)");

        // Configure invoice-buyer relationship
        modelBuilder
            .Entity<Invoice>()
          .HasOne(i => i.Buyer)
          .WithMany(p => p.Purchases)
          .HasForeignKey(i => i.BuyerId);

        // Configure invoice-seller relationship
        modelBuilder
            .Entity<Invoice>()
          .HasOne(i => i.Seller)
          .WithMany(p => p.Sales)
          .HasForeignKey(i => i.SellerId);
    }
}