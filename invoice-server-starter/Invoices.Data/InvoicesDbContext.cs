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



public class InvoicesDbContext : DbContext
{
    public DbSet<Person>? Persons { get; set; } = null!;
   


    public InvoicesDbContext(DbContextOptions<InvoicesDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<Invoice>()
            .Property(x => x.Price)
            .HasColumnType("decimal(10,2)");


        modelBuilder
            .Entity<Invoice>()
          .HasOne(i => i.Buyer)
          .WithMany(p => p.Purchases)
          .HasForeignKey(i => i.BuyerId);

        modelBuilder
            .Entity<Invoice>()
          .HasOne(i => i.Seller)
          .WithMany(p => p.Sales)
          .HasForeignKey(i => i.SellerId);
    }
}

