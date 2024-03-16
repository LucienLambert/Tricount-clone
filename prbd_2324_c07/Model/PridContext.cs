
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PRBD_Framework;
using System.Configuration;
using System.Reflection.Emit;

namespace prbd_2324_c07.Model;

public class PridContext : DbContextBase
{

    public DbSet<User> Users => Set<User>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Tricount> Tricounts => Set<Tricount>();
    public DbSet<Operation> Operations => Set<Operation>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        base.OnConfiguring(optionsBuilder);

        /*
         * SQLite
         */

        // var connectionString = ConfigurationManager.ConnectionStrings["SqliteConnectionString"].ConnectionString;
        // optionsBuilder.UseSqlite(connectionString);

        /*
         * SQL Server
         */

        var connectionString = ConfigurationManager.ConnectionStrings["MsSqlConnectionString"].ConnectionString;
        optionsBuilder.UseSqlServer(connectionString);

        ConfigureOptions(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        Link_Subscriptions_Users_Tricounts(modelBuilder);
        SetupAttributesUsers(modelBuilder);
        SetupAttributesTricounts(modelBuilder);
        Link_Operations_Users_Tricounts(modelBuilder);
    }

    // mise en relation des tables + modification en relation.
    private void Link_Subscriptions_Users_Tricounts(ModelBuilder modelBuilder) {
        //Liaison Subscriptions => Users / Subscriptions => Tricount 
        modelBuilder.Entity<Subscription>()
            .HasKey(sub => new { sub.UserId, sub.TricountId });

        //Liaison Subscriptions => Users
        modelBuilder.Entity<Subscription>()
            .HasOne(sub => sub.User)
            .WithMany(user => user.Subscriptions)
            .HasForeignKey(sub => sub.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        //Liaison Subscriptions => Tricounts
        modelBuilder.Entity<Subscription>()
            .HasOne(sub => sub.Tricount)
            .WithMany(tri => tri.Subscriptions)
            .HasForeignKey(sub => sub.TricountId);

        //Liaison Users => Tricounts
        modelBuilder.Entity<User>()
            .HasMany(user => user.Tricounts)
            .WithOne(tri => tri.Creator)
            .HasForeignKey(tri => tri.CreatorId);

        //
        modelBuilder.Entity<User>()
            .HasMany(user => user.Subscriptions)
            .WithOne(sub => sub.User)
            .HasForeignKey(sub => sub.UserId)
            .OnDelete(DeleteBehavior.NoAction);


        //test commit feat_Lucien

    }

    private void Link_Operations_Users_Tricounts(ModelBuilder modelBuilder) {
        //Link Operations => Tricounts
        modelBuilder.Entity<Operation>().HasOne(ope => ope.Tricount)
            .WithMany(tri => tri.Operations)
            .HasForeignKey(ope => ope.TricountId)
            .OnDelete(DeleteBehavior.NoAction);

        //Link Operations => Users
        modelBuilder.Entity<Operation>().HasOne(ope => ope.Initiator)
            .WithMany(user => user.Operations)
            .HasForeignKey(ope => ope.InitiatorId)
            .OnDelete(DeleteBehavior.NoAction);

        //Link Operations => repatitions
    }

        private void SetupAttributesUsers(ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>().HasIndex(user => user.Mail).IsUnique();
        modelBuilder.Entity<User>().Property(user => user.Mail).HasMaxLength(256);
        modelBuilder.Entity<User>().Property(user => user.FullName).HasMaxLength(256);
    }
     
    private void SetupAttributesTricounts(ModelBuilder modelBuilder) { 
        modelBuilder.Entity<Tricount>().Property(tricount =>  tricount.Title).HasMaxLength(256);
        modelBuilder.Entity<Tricount>().Property(tricount => tricount.Description).HasMaxLength(1024);
        modelBuilder.Entity<Tricount>().Property(tricount => tricount.CreatedAt).HasColumnType("datetime");
    }

    private static void ConfigureOptions(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseLazyLoadingProxies()
            //.LogTo(Console.WriteLine, LogLevel.Information) // permet de visualiser les requêtes SQL générées par LINQ
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors() // attention : ralentit les requêtes
            ;
    }





}