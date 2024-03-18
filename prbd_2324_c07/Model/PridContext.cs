
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Logging;
using PRBD_Framework;
using System.Configuration;

namespace prbd_2324_c07.Model;

public class PridContext : DbContextBase
{

    public DbSet<User> Users => Set<User>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Tricount> Tricounts => Set<Tricount>();
    public DbSet<Operation> Operations => Set<Operation>();
    public DbSet<Repartition> Repartitions => Set<Repartition>();
    public DbSet<Template> Templates => Set<Template>();
    public DbSet<TemplateItem> TemplateItems => Set<TemplateItem>();

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
        Discriminator_Admin(modelBuilder);
        // Configuration des relations entre les entités
        Link_Tricounts(modelBuilder);
        Link_Subscriptions(modelBuilder);
        Link_Operations(modelBuilder);
        Link_Repartitions(modelBuilder);
        Link_TemplateItems(modelBuilder);
        Link_Templates(modelBuilder);

        //function Setup_Attributes
        SetupAttributesUsers(modelBuilder);
        SetupAttributesTricounts(modelBuilder);
        SetupAttributesOperations(modelBuilder);

        SeedData(modelBuilder);
    }

    private void Discriminator_Admin(ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>()
            .HasDiscriminator(user => user.Role)
            .HasValue<User>(Role.User)
            .HasValue<Administrator>(Role.Administrator);
    }

    private void Link_Tricounts(ModelBuilder modelBuilder) {
        // Link Tricount into User
        modelBuilder.Entity<Tricount>()
            .HasOne(t => t.Creator)
            .WithMany(u => u.Tricounts)
            .HasForeignKey(t => t.CreatorId)
            .OnDelete(DeleteBehavior.Cascade); // Si je supprime un utilisateur je veux qu'il me supprime tous les tricounts qu'un user à créé
    }

    private void Link_Subscriptions(ModelBuilder modelBuilder) {
        // Relation Subscription => User And Subscription => Tricount
        modelBuilder.Entity<Subscription>()
            .HasKey(s => new { s.UserId, s.TricountId });

        // Link Subscritpion into User
        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.User)
            .WithMany(u => u.Subscriptions)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Si je supprime un utilisateur je veux qu'il supprime les subscriptions lié au user

        // Link Subscription into Tricount
        modelBuilder.Entity<Subscription>()
            .HasOne(s => s.Tricount)
            .WithMany(t => t.Subscriptions)
            .HasForeignKey(s => s.TricountId)
            .OnDelete(DeleteBehavior.NoAction); // Si je supprime un tricount je veux qu'il supprime les subscriptions également

    }

    private void Link_Operations(ModelBuilder modelBuilder) {
        // Link Operation into User
        modelBuilder.Entity<Operation>()
            .HasOne(o => o.Initiator)
            .WithMany(u => u.Operations)
            .HasForeignKey(o => o.InitiatorId)
            .OnDelete(DeleteBehavior.NoAction); // si je supprime une Operation je veux qu'il conserve le user

        // Link Operation into Tricount
        modelBuilder.Entity<Operation>()
            .HasOne(o => o.Tricount)
            .WithMany(t => t.Operations)
            .HasForeignKey(o => o.TricountId)
            .OnDelete(DeleteBehavior.NoAction); // si je supprime une Operation je veux qu'il conserve le tricount
    }

    private void Link_Repartitions(ModelBuilder modelBuilder) {
        // Link Repartition => Operation And Operation => User
        modelBuilder.Entity<Repartition>()
            .HasKey(rep => new { rep.OperationId, rep.UserId });

        // Link Repartition into Operation
        modelBuilder.Entity<Repartition>()
            .HasOne(rep => rep.Operation)
            .WithMany(o => o.Repartitions)
            .HasForeignKey(rep => rep.OperationId)
            .OnDelete(DeleteBehavior.NoAction); // si je supprime une Repartition je veux qu'il conserve l'opération

        // Link Repartition  into User
        modelBuilder.Entity<Repartition>()
            .HasOne(rep => rep.User)
            .WithMany(u => u.Repartitions)
            .HasForeignKey(rep => rep.UserId)
            .OnDelete(DeleteBehavior.NoAction); // si je supprime une Repartition je veux qu'il conserve le user
    }

    private void Link_TemplateItems(ModelBuilder modelBuilder) {
        // Link TemplateItem => User And TemplateItem => Template
        modelBuilder.Entity<TemplateItem>()
            .HasKey(tempItem => new { tempItem.UserId, tempItem.TemplateId });

        // Link TemplateItem into User
        modelBuilder.Entity<TemplateItem>()
            .HasOne(tempItem => tempItem.User)
            .WithMany(u => u.TemplatesItems)
            .HasForeignKey(tempItem => tempItem.UserId)
            .OnDelete(DeleteBehavior.NoAction); // si je supprime une TemplateItem je veux qu'il conserve le user

        // Link TemplateItem into Template
        modelBuilder.Entity<TemplateItem>()
            .HasOne(tempItem => tempItem.Template)
            .WithMany(t => t.TemplatesItems)
            .HasForeignKey(tempItem => tempItem.TemplateId)
            .OnDelete(DeleteBehavior.NoAction); // si je supprime une TemplateItem je veux qu'il conserve le Temple
    }

    private void Link_Templates(ModelBuilder modelBuilder) {
        // Link Template into Tricount
        modelBuilder.Entity<Template>()
            .HasOne(t => t.Tricount)
            .WithMany(tc => tc.Templates)
            .HasForeignKey(t => t.TricountId)
            .OnDelete(DeleteBehavior.Cascade); // si je supprime un tricount je veux qu'il supprime les Temples lié.
    }

    private void SetupAttributesUsers(ModelBuilder modelBuilder) {
        modelBuilder.Entity<User>().HasIndex(user => user.Mail).IsUnique();
        modelBuilder.Entity<User>().Property(user => user.Mail).HasMaxLength(256);
        modelBuilder.Entity<User>().Property(user => user.FullName).HasMaxLength(256);
    }
     
    private void SetupAttributesTricounts(ModelBuilder modelBuilder) { 
        modelBuilder.Entity<Tricount>().Property(tri => tri.Title).HasMaxLength(256);
        modelBuilder.Entity<Tricount>().Property(tri => tri.Description).HasMaxLength(1024);
        modelBuilder.Entity<Tricount>().Property(tri => tri.CreatedAt).HasColumnType("datetime");
    }

    private void SetupAttributesOperations(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Operation>().Property(ope => ope.Title).HasMaxLength(256);
        modelBuilder.Entity<Operation>().Property(ope => ope.Operation_date).HasColumnType("date");
    }

    private static void ConfigureOptions(DbContextOptionsBuilder optionsBuilder) {
        optionsBuilder.UseLazyLoadingProxies()
            //.LogTo(Console.WriteLine, LogLevel.Information) // permet de visualiser les requêtes SQL générées par LINQ
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors(); // attention : ralentit les requêtes
    }

    private static void SeedData(ModelBuilder modelBuilder) {

        var a = new User { UserId = 1, Mail = "boverhaegen@epfc.eu", FullName = "Boris", Password = "3D4AEC0A9B43782133B8120B2FDD8C6104ABB513FE0CDCD0D1D4D791AA42E338: C217604FDAEA7291C7BA5D1D525815E4:100000:SHA256" };
        var b = new User { UserId = 2, Mail = "bepenelle@epfc.eu", FullName = "Benoît", Password = "9E58D87797C6795D294E6762B6C05116D075BC18445AD4078C25674809DB57EF:C91E0B85B7264877C0424D52494D6296:100000:SHA256" };
        var c = new User { UserId = 3, Mail = "xapigeolet@epfc.eu", FullName = "Xavier", Password = "5B979AB86EC73B0996F439D0BC3947ECCFA0A41310C77533EA36CB409DBB1243:0CF43009110DE4B4AA6D4E749F622755:100000:SHA256" };
        var d = new User { UserId = 4, Mail = "mamichel@epfc.eu", FullName = "Marc", Password = "955F147CE3473774E35EE58F4233AA84AE9118C6ECD4699DD788B8D588238034:5514D1DD0A97E9BA7FE4C0B5A4E89351:100000:SHA256" };
        var e = new Administrator { UserId = 5, Mail = "admin@epfc.eu", FullName = "Administrator", Password = "C9949A02A5DFBE50F1DA289DC162E3C97443AB09CE6F6EB1FD0C9D51B5241BBD:5533437973C5BC6459DB687CA5BDE76C:100000:SHA256" };

        modelBuilder.Entity<User>().HasData(a, b, c, d);
        modelBuilder.Entity<Administrator>().HasData(e);

        modelBuilder.Entity<Tricount>().HasData(
            new Tricount { TricountId = 1, Title = "Gers 2023", CreatedAt = DateTime.Parse("2023/10/10 18:42:24"), CreatorId = 1 },
            new Tricount { TricountId = 2, Title = "Resto badminton", CreatedAt = DateTime.Parse("2023/10/10 19:25:10"), CreatorId = 1 },
            new Tricount { TricountId = 4, Title = "Vacances", Description = "A la mer du nord", CreatedAt = DateTime.Parse("2023/10/10 19:31:09"), CreatorId = 1 },
            new Tricount { TricountId = 5, Title = "Grosse virée", Description = "A Torremolinos", CreatedAt = DateTime.Parse("2023/08/15 10:00:00"), CreatorId = 2 },
            new Tricount { TricountId = 6, Title = "Torhout Werchter", Description = "Memorable", CreatedAt = DateTime.Parse("2023/07/02 18:30:12"), CreatorId = 3 }
        );

        modelBuilder.Entity<Subscription>().HasData(
            new Subscription { UserId = 1, TricountId = 1 },
            new Subscription { UserId = 1, TricountId = 2 },
            new Subscription { UserId = 1, TricountId = 4 },
            new Subscription { UserId = 1, TricountId = 6 },
            new Subscription { UserId = 2, TricountId = 2 },
            new Subscription { UserId = 2, TricountId = 4 },
            new Subscription { UserId = 2, TricountId = 5 },
            new Subscription { UserId = 2, TricountId = 6 },
            new Subscription { UserId = 3, TricountId = 4 },
            new Subscription { UserId = 3, TricountId = 5 },
            new Subscription { UserId = 3, TricountId = 6 },
            new Subscription { UserId = 4, TricountId = 4 },
            new Subscription { UserId = 4, TricountId = 5 },
            new Subscription { UserId = 4, TricountId = 6 }
        );

        modelBuilder.Entity<Operation>().HasData(
            new Operation { OperationId = 1, Title = "Colruyt", TricountId = 4, Amount = (decimal)100f, Operation_date = DateTime.Parse("2023/10/13"), InitiatorId = 2 },
            new Operation { OperationId = 2, Title = "Plein essence", TricountId = 4, Amount = (decimal)75f, Operation_date = DateTime.Parse("2023/10/13"), InitiatorId = 1 },
            new Operation { OperationId = 3, Title = "Grosses courses LIDL", TricountId = 4, Amount = (decimal)212.47f, Operation_date = DateTime.Parse("2023/10/13"), InitiatorId = 3 },
            new Operation { OperationId = 4, Title = "Apéros", TricountId = 4, Amount = (decimal)31.897456217f, Operation_date = DateTime.Parse("2023/10/13"), InitiatorId = 1 },
            new Operation { OperationId = 5, Title = "Boucherie", TricountId = 4, Amount = (decimal)25.5f, Operation_date = DateTime.Parse("2023/10/26"), InitiatorId = 2 },
            new Operation { OperationId = 6, Title = "Loterie", TricountId = 4, Amount = (decimal)35f, Operation_date = DateTime.Parse("2023/10/26"), InitiatorId = 1 },
            new Operation { OperationId = 7, Title = "Sangria", TricountId = 5, Amount = (decimal)42f, Operation_date = DateTime.Parse("2023/08/16"), InitiatorId = 2 },
            new Operation { OperationId = 8, Title = "Jet Ski", TricountId = 5, Amount = (decimal)250f, Operation_date = DateTime.Parse("2023/08/17"), InitiatorId = 3 },
            new Operation { OperationId = 9, Title = "PV parking", TricountId = 5, Amount = (decimal)15.5f, Operation_date = DateTime.Parse("2023/08/16"), InitiatorId = 3 },
            new Operation { OperationId = 10, Title = "Tickets", TricountId = 6, Amount = (decimal)220f, Operation_date = DateTime.Parse("2023/06/08"), InitiatorId = 1 },
            new Operation { OperationId = 11, Title = "Décathlon", TricountId = 6, Amount = (decimal)199.99f, Operation_date = DateTime.Parse("2023/07/01"), InitiatorId = 2 }
        );

        modelBuilder.Entity<Repartition>().HasData(
            new Repartition { OperationId = 1, UserId = 1, Weight = 1 },
            new Repartition { OperationId = 1, UserId = 2, Weight = 1 },
            new Repartition { OperationId = 2, UserId = 1, Weight = 1 },
            new Repartition { OperationId = 2, UserId = 2, Weight = 1 },
            new Repartition { OperationId = 3, UserId = 1, Weight = 2 },
            new Repartition { OperationId = 3, UserId = 2, Weight = 1 },
            new Repartition { OperationId = 3, UserId = 3, Weight = 1 },
            new Repartition { OperationId = 4, UserId = 1, Weight = 1 },
            new Repartition { OperationId = 4, UserId = 2, Weight = 2 },
            new Repartition { OperationId = 4, UserId = 3, Weight = 3 },
            new Repartition { OperationId = 5, UserId = 1, Weight = 2 },
            new Repartition { OperationId = 5, UserId = 2, Weight = 1 },
            new Repartition { OperationId = 5, UserId = 3, Weight = 1 },
            new Repartition { OperationId = 6, UserId = 1, Weight = 1 },
            new Repartition { OperationId = 6, UserId = 3, Weight = 1 },
            new Repartition { OperationId = 7, UserId = 2, Weight = 1 },
            new Repartition { OperationId = 7, UserId = 3, Weight = 2 },
            new Repartition { OperationId = 7, UserId = 4, Weight = 3 },
            new Repartition { OperationId = 8, UserId = 3, Weight = 2 },
            new Repartition { OperationId = 8, UserId = 4, Weight = 1 },
            new Repartition { OperationId = 9, UserId = 2, Weight = 1 },
            new Repartition { OperationId = 9, UserId = 4, Weight = 5 },
            new Repartition { OperationId = 10, UserId = 1, Weight = 1 },
            new Repartition { OperationId = 10, UserId = 3, Weight = 1 },
            new Repartition { OperationId = 11, UserId = 2, Weight = 2 },
            new Repartition { OperationId = 11, UserId = 4, Weight = 2 }
        );

        modelBuilder.Entity<Template>().HasData(
            new Template { TemplateId = 1, Title = "Boris paye double", TricountId = 4 },
            new Template { TemplateId = 2, Title = "Boris ne paye rien", TricountId = 4 }
        );

        modelBuilder.Entity<TemplateItem>().HasData(
            new TemplateItem { TemplateId = 1, UserId = 1, Weight = 2 },
            new TemplateItem { TemplateId = 1, UserId = 2, Weight = 1 },
            new TemplateItem { TemplateId = 1, UserId = 3, Weight = 1 },
            new TemplateItem { TemplateId = 2, UserId = 1, Weight = 1 },
            new TemplateItem { TemplateId = 2, UserId = 3, Weight = 1 }
        );


    }



}