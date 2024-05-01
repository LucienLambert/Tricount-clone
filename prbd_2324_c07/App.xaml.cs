using prbd_2324_c07.Model;
using prbd_2324_c07.ViewModel;
using System.Windows;
using System.Globalization;
using PRBD_Framework;
using Microsoft.EntityFrameworkCore;
using System;

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

        NavigateTo<LoginViewModel, User, PridContext>();
    }

    private static void PrepareDatabase() {
        // Clear database and seed data
        Context.Database.EnsureDeleted();
        Context.Database.EnsureCreated();

        // Cold start
        Console.Write("Cold starting database... ");
        Context.Users.Find(1);
        Console.WriteLine("done");
    }

    protected override void OnRefreshData() {
        // TODO
    }

    private static void TestQueries() {

        Console.WriteLine("Befor Modification");
        foreach (var user in Context.Users) {
            Console.WriteLine("USER : " + user.FullName);
            Console.WriteLine("SUBSCRIPTION : " + user.Subscriptions.Count);
            Console.WriteLine("TRICOUNT : " + user.Tricounts.Count);
            Console.WriteLine("OPERATION : " + user.Operations.Count);
            Console.WriteLine("REPARTITION : " + user.Repartitions.Count);
            foreach (var tri in Context.Tricounts) {
                if (tri.CreatorId == user.UserId) {
                    Console.WriteLine("TEMPLE : " + tri.Templates.Count);
                }
            }
            Console.WriteLine("TEMPLE_ITEMS : " + user.TemplatesItems.Count);
            Console.WriteLine("********************************");
        }

        var test = Context.Users.FirstOrDefault(r => r.FullName == "Boris");
        if (test != null) {
            Context.Users.Remove(test);
        }

        //Test Balances
        var tricountsList = Context.Tricounts.ToList();
        tricountsList.ForEach(tri => {
            tri.RefreshBalance();
        });

        Context.SaveChanges();
        // Un endroit pour tester vos requêtes LINQ
    }
}