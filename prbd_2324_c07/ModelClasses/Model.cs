//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Internal;
//using Microsoft.Extensions.Logging;
//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//public class Model : DbContext {



//    public DbSet<User> Users { get; set; }
//    public DbSet<Tricount> Tricounts { get; set; }
//    public DbSet<Subscription> Subscriptions { get; set; }
//    public DbSet<Operation> Operations { get; set; }
//    public DbSet<Repartition> Repartitions { get; set; }
//    public DbSet<Template> Templates { get; set; }
//    public DbSet<TemplateItem> TemplateItems { get; set; }



//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
//        base.OnConfiguring(optionsBuilder);
//        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=project")
//            .LogTo(Console.WriteLine, LogLevel.Information)
//            .EnableSensitiveDataLogging();

//        //optionsBuilder.UseSqlite(@"Data Source=PRBDproject")
//        //    //.LogTo(Console.WriteLine, LogLevel.Information)
//        //    .EnableSensitiveDataLogging()
//        //    ;
//    }

//    protected override void OnModelCreating(ModelBuilder modelBuilder) {
//        base.OnModelCreating(modelBuilder);



//        // l'entité Member ...
//        modelBuilder.Entity<User>()
//            // doit utiliser la propriété Role comme discriminateur ...
//            .HasDiscriminator(u => u.Role)
//            // en mappant la valeur Role.Member sur le type Member ...
//            .HasValue<User>(Role.User)
//            // et en mappant la valeur Role.Administator sur le type Administrator ...
//            .HasValue<Administrator>(Role.Administrator);

//        modelBuilder.Entity<User>()
//            .HasIndex(us => us.Email)
//            .IsUnique();

//        modelBuilder.Entity<User>()
//            .Property(u => u.Id)
//            .HasColumnType("int");

//        modelBuilder.Entity<User>()
//            .Property(u => u.Email)
//            .HasMaxLength(256)
//            .HasColumnType("varchar(256)");

//        modelBuilder.Entity<User>()
//            .Property(u => u.Password)
//            .HasMaxLength(512)
//            .HasColumnType("varchar(512)");

//        modelBuilder.Entity<User>()
//            .Property(u => u.FullName)
//            .HasMaxLength(256)
//            .HasColumnType("varchar(256)");

//        modelBuilder.Entity<User>()
//            .Property(u => u.Role)
//            .HasColumnType("int");



//        modelBuilder.Entity<User>()
//            .HasMany(us => us.Subscriptions)
//            .WithOne(sub => sub.User)
//            .HasForeignKey(sub => sub.UserId)
//            .OnDelete(DeleteBehavior.NoAction);


//        modelBuilder.Entity<User>()
//            .HasMany(us => us.Tricounts)
//            .WithOne(tri => tri.Creator)
//            .HasForeignKey(tri => tri.CreatorId)
//            .OnDelete(DeleteBehavior.NoAction);


//        modelBuilder.Entity<User>()
//            .HasMany(us => us.Operations)
//            .WithOne(ope => ope.Initiator)
//            .HasForeignKey(ope => ope.InitiatorId)
//            .OnDelete(DeleteBehavior.NoAction);


//        modelBuilder.Entity<User>()
//            .HasMany(us => us.Repartitions)
//            .WithOne(rep => rep.User)
//            .HasForeignKey(rep => rep.UserId)
//            .OnDelete(DeleteBehavior.NoAction);

//        modelBuilder.Entity<User>()
//            .HasMany(us => us.TemplateItems)
//            .WithOne(tmpi => tmpi.User)
//            .HasForeignKey(tmpi => tmpi.UserId)
//            .OnDelete(DeleteBehavior.NoAction);





//        modelBuilder.Entity<Subscription>()
//            .HasKey(sub => new { sub.UserId, sub.TricountId });

//        modelBuilder.Entity<Subscription>()
//            .Property(s => s.TricountId)
//            .HasColumnType("int");

//        modelBuilder.Entity<Subscription>()
//            .Property(s => s.UserId)
//            .HasColumnType("int");



//        modelBuilder.Entity<Subscription>()
//            .HasOne(sub => sub.User)
//            .WithMany(us => us.Subscriptions)
//            .HasForeignKey(sub => sub.UserId)
//            .OnDelete(DeleteBehavior.NoAction);

//        modelBuilder.Entity<Subscription>()
//            .HasOne(sub => sub.Tricount)
//            .WithMany(tri => tri.Subscriptions)
//            .HasForeignKey(sub => sub.TricountId)
//            .OnDelete(DeleteBehavior.NoAction);






//        modelBuilder.Entity<Tricount>()
//            .Property(t => t.Id)
//            .HasColumnType("int");

//        modelBuilder.Entity<Tricount>()
//            .Property(t => t.Title)
//            .HasMaxLength(256)
//            .HasColumnType("varchar(256)");

//        modelBuilder.Entity<Tricount>()
//            .Property(t => t.Description)
//            .HasMaxLength(1024)
//            .HasColumnType("varchar(1024)");

//        modelBuilder.Entity<Tricount>()
//            .Property(t => t.CreationDate)
//            .HasColumnType("datetime");

//        modelBuilder.Entity<Tricount>()
//            .Property(t => t.CreatorId)
//            .HasColumnType("int");



//        //making sure it's nullable
//        modelBuilder.Entity<Tricount>()
//            .Property(tri => tri.Description)
//            .IsRequired(false);

//        modelBuilder.Entity<Tricount>()
//            .HasMany(tri => tri.Subscriptions)
//            .WithOne(sub => sub.Tricount)
//            .HasForeignKey(sub => sub.TricountId)
//            .OnDelete(DeleteBehavior.NoAction);

//        modelBuilder.Entity<Tricount>()
//            .HasMany(tri => tri.Operations)
//            .WithOne(ope => ope.Tricount)
//            .HasForeignKey(ope => ope.TricountId)
//            .OnDelete(DeleteBehavior.NoAction);

//        modelBuilder.Entity<Tricount>()
//            .HasMany(tri => tri.Templates)
//            .WithOne(temp => temp.Tricount)
//            .HasForeignKey(temp => temp.TricountId)
//            .OnDelete(DeleteBehavior.NoAction);

//        modelBuilder.Entity<Tricount>()
//            .HasOne(tri=>tri.Creator)
//            .WithMany(us=>us.Tricounts)
//            .HasForeignKey(tri=>tri.CreatorId)
//            .OnDelete(DeleteBehavior.Cascade);




//        modelBuilder.Entity<Operation>()
//            .Property(o => o.Id)
//            .HasColumnType("int");

//        modelBuilder.Entity<Operation>()
//            .Property(o => o.Title)
//            .HasMaxLength(256)
//            .HasColumnType("varchar(256)");

//        modelBuilder.Entity<Operation>()
//            .Property(o => o.TricountId)
//            .HasColumnType("int");

//        modelBuilder.Entity<Operation>()
//            .Property(o => o.Amount)
//            .HasColumnType("float");

//        modelBuilder.Entity<Operation>()
//            .Property(o => o.Date)
//            .HasColumnType("date");

//        modelBuilder.Entity<Operation>()
//            .Property(o => o.InitiatorId)
//            .HasColumnType("int");





//        modelBuilder.Entity<Operation>()
//            .HasOne(ope => ope.Tricount)
//            .WithMany(tri => tri.Operations)
//            .HasForeignKey(ope => ope.TricountId)
//            .OnDelete(DeleteBehavior.Cascade);

//        modelBuilder.Entity<Operation>()
//            .HasMany(ope => ope.Repartitions)
//            .WithOne(rep => rep.Operation)
//            .HasForeignKey(rep => rep.OperationId)
//            .OnDelete(DeleteBehavior.NoAction);

//        modelBuilder.Entity<Operation>()
//            .HasOne(ope => ope.Initiator)
//            .WithMany(u=>u.Operations)
//            .HasForeignKey(ope => ope.InitiatorId)
//            .OnDelete(DeleteBehavior.NoAction);



//        modelBuilder.Entity<Repartition>()
//            .Property(r => r.Weight)
//            .HasColumnType("int");

//        modelBuilder.Entity<Repartition>()
//            .Property(r => r.OperationId)
//            .HasColumnType("int");

//        modelBuilder.Entity<Repartition>()
//            .Property(r => r.UserId)
//            .HasColumnType("int");


//        modelBuilder.Entity<Repartition>()
//            .HasKey(rep => new { rep.OperationId, rep.UserId });

//        modelBuilder.Entity<Repartition>()
//            .HasOne(rep => rep.User)
//            .WithMany(us => us.Repartitions)
//            .HasForeignKey(rep => rep.UserId)
//            .OnDelete(DeleteBehavior.NoAction);

//        modelBuilder.Entity<Repartition>()
//            .HasOne(rep => rep.Operation)
//            .WithMany(ope => ope.Repartitions)
//            .HasForeignKey(rep => rep.OperationId)
//            .OnDelete(DeleteBehavior.Cascade);



//        modelBuilder.Entity<Template>()
//            .Property(t => t.Title)
//            .HasMaxLength(256)
//            .HasColumnType("varchar(256)");

//        modelBuilder.Entity<Template>()
//            .Property(t => t.TricountId)
//            .HasColumnType("int");

//        modelBuilder.Entity<Template>()
//            .Property(t => t.Id)
//            .HasColumnType("int");


//        modelBuilder.Entity<Template>()
//            .HasOne(tri => tri.Tricount)
//            .WithMany(tri => tri.Templates)
//            .HasForeignKey(temp => temp.TricountId)
//            .OnDelete(DeleteBehavior.Cascade);

//        modelBuilder.Entity<Template>()
//            .HasMany(temp => temp.TemplateItems)
//            .WithOne(tmpi => tmpi.Template)
//            .HasForeignKey(tmpi => tmpi.TemplateId)
//            .OnDelete(DeleteBehavior.NoAction);


//        modelBuilder.Entity<TemplateItem>()
//             .HasKey(tmpi => new { tmpi.UserId, tmpi.TemplateId, tmpi.Weight });

//        modelBuilder.Entity<TemplateItem>()
//            .Property(t => t.Weight)
//            .HasColumnType("int");

//        modelBuilder.Entity<TemplateItem>()
//            .Property(t => t.UserId)
//            .HasColumnType("int");

//        modelBuilder.Entity<TemplateItem>()
//            .Property(t => t.TemplateId)
//            .HasColumnType("int");


//        modelBuilder.Entity<TemplateItem>()
//            .HasOne(tmpi => tmpi.Template)
//            .WithMany(temp => temp.TemplateItems)
//            .HasForeignKey(temp => temp.TemplateId)
//            .OnDelete(DeleteBehavior.Cascade);

//        modelBuilder.Entity<TemplateItem>()
//            .HasOne(tmpi => tmpi.User)
//            .WithMany(us => us.TemplateItems)
//            .HasForeignKey(tmpi => tmpi.UserId)
//            .OnDelete(DeleteBehavior.NoAction);


//        SeedData(modelBuilder);
//    }


//    private static void SeedData(ModelBuilder modelBuilder) {

//        var a = new User { Id = 1, Email = "boverhaegen@epfc.eu", FullName = "Boris", Password = "3D4AEC0A9B43782133B8120B2FDD8C6104ABB513FE0CDCD0D1D4D791AA42E338: C217604FDAEA7291C7BA5D1D525815E4:100000:SHA256" };
//        var b = new User { Id = 2, Email = "bepenelle@epfc.eu", FullName = "Benoît", Password = "9E58D87797C6795D294E6762B6C05116D075BC18445AD4078C25674809DB57EF:C91E0B85B7264877C0424D52494D6296:100000:SHA256" };
//        var c = new User { Id = 3, Email = "xapigeolet@epfc.eu", FullName = "Xavier", Password = "5B979AB86EC73B0996F439D0BC3947ECCFA0A41310C77533EA36CB409DBB1243:0CF43009110DE4B4AA6D4E749F622755:100000:SHA256" };
//        var d = new User { Id = 4, Email = "mamichel@epfc.eu", FullName = "Marc", Password = "955F147CE3473774E35EE58F4233AA84AE9118C6ECD4699DD788B8D588238034:5514D1DD0A97E9BA7FE4C0B5A4E89351:100000:SHA256" };
//        var e = new Administrator { Id = 5, Email = "admin@epfc.eu", FullName = "Administrator", Password = "C9949A02A5DFBE50F1DA289DC162E3C97443AB09CE6F6EB1FD0C9D51B5241BBD:5533437973C5BC6459DB687CA5BDE76C:100000:SHA256" };

//        modelBuilder.Entity<User>().HasData(a, b, c, d);
//        modelBuilder.Entity<Administrator>().HasData(e);

//        modelBuilder.Entity<Tricount>().HasData(
//            new Tricount { Id = 1, Title = "Gers 2023", CreationDate = DateTime.Parse("2023/10/10 18:42:24"), CreatorId = 1 },
//            new Tricount { Id = 2, Title = "Resto badminton", CreationDate = DateTime.Parse("2023/10/10 19:25:10"), CreatorId = 1 },
//            new Tricount { Id = 4, Title = "Vacances", Description = "A la mer du nord", CreationDate = DateTime.Parse("2023/10/10 19:31:09"), CreatorId = 1 },
//            new Tricount { Id = 5, Title = "Grosse virée", Description = "A Torremolinos", CreationDate = DateTime.Parse("2023/08/15 10:00:00"), CreatorId = 2 },
//            new Tricount { Id = 6, Title = "Torhout Werchter", Description = "Memorable", CreationDate = DateTime.Parse("2023/07/02 18:30:12"), CreatorId = 3 }
//        );

//        modelBuilder.Entity<Subscription>().HasData(
//            new Subscription { UserId = 1, TricountId = 1 },
//            new Subscription { UserId = 1, TricountId = 2 },
//            new Subscription { UserId = 1, TricountId = 4 },
//            new Subscription { UserId = 1, TricountId = 6 },
//            new Subscription { UserId = 2, TricountId = 2 },
//            new Subscription { UserId = 2, TricountId = 4 },
//            new Subscription { UserId = 2, TricountId = 5 },
//            new Subscription { UserId = 2, TricountId = 6 },
//            new Subscription { UserId = 3, TricountId = 4 },
//            new Subscription { UserId = 3, TricountId = 5 },
//            new Subscription { UserId = 3, TricountId = 6 },
//            new Subscription { UserId = 4, TricountId = 4 },
//            new Subscription { UserId = 4, TricountId = 5 },
//            new Subscription { UserId = 4, TricountId = 6 }
//        );

//        modelBuilder.Entity<Operation>().HasData(
//            new Operation { Id = 1, Title = "Colruyt", TricountId = 4, Amount = 100f, Date = DateTime.Parse("2023/10/13"), InitiatorId = 2 },
//            new Operation { Id = 2, Title = "Plein essence", TricountId = 4, Amount = 75f, Date = DateTime.Parse("2023/10/13"), InitiatorId = 1 },
//            new Operation { Id = 3, Title = "Grosses courses LIDL", TricountId = 4, Amount = 212.47f, Date = DateTime.Parse("2023/10/13"), InitiatorId = 3 },
//            new Operation { Id = 4, Title = "Apéros", TricountId = 4, Amount = 31.897456217f, Date = DateTime.Parse("2023/10/13"), InitiatorId = 1 },
//            new Operation { Id = 5, Title = "Boucherie", TricountId = 4, Amount = 25.5f, Date = DateTime.Parse("2023/10/26"), InitiatorId = 2 },
//            new Operation { Id = 6, Title = "Loterie", TricountId = 4, Amount = 35f, Date = DateTime.Parse("2023/10/26"), InitiatorId = 1 },
//            new Operation { Id = 7, Title = "Sangria", TricountId = 5, Amount = 42f, Date = DateTime.Parse("2023/08/16"), InitiatorId = 2 },
//            new Operation { Id = 8, Title = "Jet Ski", TricountId = 5, Amount = 250f, Date = DateTime.Parse("2023/08/17"), InitiatorId = 3 },
//            new Operation { Id = 9, Title = "PV parking", TricountId = 5, Amount = 15.5f, Date = DateTime.Parse("2023/08/16"), InitiatorId = 3 },
//            new Operation { Id = 10, Title = "Tickets", TricountId = 6, Amount = 220f, Date = DateTime.Parse("2023/06/08"), InitiatorId = 1 },
//            new Operation { Id = 11, Title = "Décathlon", TricountId = 6, Amount = 199.99f, Date = DateTime.Parse("2023/07/01"), InitiatorId = 2 }
//        );

//        modelBuilder.Entity<Repartition>().HasData(
//            new Repartition { OperationId = 1, UserId = 1, Weight = 1 },
//            new Repartition { OperationId = 1, UserId = 2, Weight = 1 },
//            new Repartition { OperationId = 2, UserId = 1, Weight = 1 },
//            new Repartition { OperationId = 2, UserId = 2, Weight = 1 },
//            new Repartition { OperationId = 3, UserId = 1, Weight = 2 },
//            new Repartition { OperationId = 3, UserId = 2, Weight = 1 },
//            new Repartition { OperationId = 3, UserId = 3, Weight = 1 },
//            new Repartition { OperationId = 4, UserId = 1, Weight = 1 },
//            new Repartition { OperationId = 4, UserId = 2, Weight = 2 },
//            new Repartition { OperationId = 4, UserId = 3, Weight = 3 },
//            new Repartition { OperationId = 5, UserId = 1, Weight = 2 },
//            new Repartition { OperationId = 5, UserId = 2, Weight = 1 },
//            new Repartition { OperationId = 5, UserId = 3, Weight = 1 },
//            new Repartition { OperationId = 6, UserId = 1, Weight = 1 },
//            new Repartition { OperationId = 6, UserId = 3, Weight = 1 },
//            new Repartition { OperationId = 7, UserId = 2, Weight = 1 },
//            new Repartition { OperationId = 7, UserId = 3, Weight = 2 },
//            new Repartition { OperationId = 7, UserId = 4, Weight = 3 },
//            new Repartition { OperationId = 8, UserId = 3, Weight = 2 },
//            new Repartition { OperationId = 8, UserId = 4, Weight = 1 },
//            new Repartition { OperationId = 9, UserId = 2, Weight = 1 },
//            new Repartition { OperationId = 9, UserId = 4, Weight = 5 },
//            new Repartition { OperationId = 10, UserId = 1, Weight = 1 },
//            new Repartition { OperationId = 10, UserId = 3, Weight = 1 },
//            new Repartition { OperationId = 11, UserId = 2, Weight = 2 },
//            new Repartition { OperationId = 11, UserId = 4, Weight = 2 }
//        );

//        modelBuilder.Entity<Template>().HasData(
//            new Template { Id = 1, Title = "Boris paye double", TricountId = 4 },
//            new Template { Id = 2, Title = "Boris ne paye rien", TricountId = 4 }
//        );

//        modelBuilder.Entity<TemplateItem>().HasData(
//            new TemplateItem { TemplateId = 1, UserId = 1, Weight = 2 },
//            new TemplateItem { TemplateId = 1, UserId = 2, Weight = 1 },
//            new TemplateItem { TemplateId = 1, UserId = 3, Weight = 1 },
//            new TemplateItem { TemplateId = 2, UserId = 1, Weight = 1 },
//            new TemplateItem { TemplateId = 2, UserId = 3, Weight = 1 }
//        );


//    }

//}



















//// l'entité Member ...
//modelBuilder.Entity<Member>()
//    // doit utiliser la propriété Role comme discriminateur ...
//    .HasDiscriminator(m => m.Role)
//    // en mappant la valeur Role.Member sur le type Member ...
//    .HasValue<Member>(Role.Member)
//    // et en mappant la valeur Role.Administator sur le type Administrator ...
//    .HasValue<Administrator>(Role.Administrator);

//// l'entité User participe à une relation many-to-many ...
//modelBuilder.Entity<User>()
//    // avec, d'un côté, la propriété Followees ...
//    .HasMany(m => m.Followees)
//    // avec, de l'autre côté, la propriété Followers ...
//    .WithMany(m => m.Followers)
//    // en utilisant l'entité Follow comme entité "association"
//    .UsingEntity<Follow>(
//        // celle-ci étant constituée d'une relation one-to-many à partir de Followee
//        right => right.HasOne(f => f.Followee).WithMany().HasForeignKey(nameof(Follow.FolloweePseudo))
//            .OnDelete(DeleteBehavior.ClientCascade),
//        // et d'une autre relation one-to-many à partir de Follower
//        left => left.HasOne(f => f.Follower).WithMany().HasForeignKey(nameof(Follow.FollowerPseudo))
//            .OnDelete(DeleteBehavior.ClientCascade),
//        joinEntity => {
//            // en n'oubliant pas de spécifier la clé primaire composée de la table association
//            joinEntity.HasKey(f => new { f.FollowerPseudo, f.FolloweePseudo });
//        });



