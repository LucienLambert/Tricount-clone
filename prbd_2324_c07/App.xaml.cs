using prbd_2324_c07.Model;
using prbd_2324_c07.ViewModel;
using System.Windows;
using System.Globalization;
using PRBD_Framework;
using Microsoft.EntityFrameworkCore;

namespace prbd_2324_c07;

public partial class App : ApplicationBase<User,PridContext>{
    public App() {
        var ci = new CultureInfo("fr-BE") {
            DateTimeFormat = {
                ShortDatePattern = "dd/MM/yyyy",
                DateSeparator = "/"
            }
        };
        CultureInfo.DefaultThreadCurrentCulture = ci;
        CultureInfo.DefaultThreadCurrentUICulture = ci;
        CultureInfo.CurrentCulture = ci;
        CultureInfo.CurrentUICulture = ci;
    }

    protected override void OnStartup(StartupEventArgs e) {
        PrepareDatabase();
        TestQueries();

        NavigateTo<MainViewModel, User, PridContext>();
    }

    private static void PrepareDatabase() {
        // Clear database and seed data
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();

        var userTest = new User("lucien@gmail.com", "password", "Lucien Lambert");
        var userAdmin = new Administrator("admin", "passwordAdmin", "admin administrator");

        

        var tricTest = new Tricount("Tricount test", "test test", userTest);

        var subTest = new Subscription(userTest, tricTest);

        var opeTest = new Operation("operation faite pare UserTest 100 euro", 100, tricTest, userTest);

        Context.Users.AddRange(userTest, userAdmin);
        Context.Tricounts.Add(tricTest);
        Context.Subscriptions.Add(subTest);
        // Context.Operations.Add(opeTest);

        Context.SaveChanges();

        foreach (var user in Context.Users) {
            Console.WriteLine("*****************Befor del***************");
            Console.WriteLine("USER : " + user);
        }

        foreach (var tric in Context.Tricounts) {
            Console.WriteLine("TRICOUNT : " + tric);
        }

        Context.Users.Remove(userTest);
        Context.SaveChanges();

        foreach (var user in Context.Users) {
            Console.WriteLine("*****************After del***************");
            Console.WriteLine("USER : " + user);
        }
        foreach (var tric in Context.Tricounts) {
            Console.WriteLine("TRICOUNT : " + tric);
        }

        // Cold start
        Console.Write("Cold starting database... ");
        Context.Users.Find(1);
        Console.WriteLine("done");
        Context.SaveChanges();
    }

    protected override void OnRefreshData() {
        // TODO
    }

    private static void TestQueries() {
        // Un endroit pour tester vos requêtes LINQ
    }
}