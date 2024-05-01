using prbd_2324_c07.Model;
using prbd_2324_c07.ViewModel;
using System.Windows;
using System.Globalization;
using PRBD_Framework;

namespace prbd_2324_c07;

public partial class App : ApplicationBase<User,PridContext>{
    public enum Messages{
        MSG_NEW_USER,
        MSG_PSEUDO_CHANGED,
        MSG_USER_CHANGED,
        MSG_DISPLAY_USER,
        MSG_CLOSE_TAB,
        MSG_LOGIN,
        MSG_LOGOUT
    }

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
        base.OnStartup(e);

        PrepareDatabase();
        TestQueries();

        // par défaut au start on passera par ce NavigateTo
        NavigateTo<LoginViewModel, User, PridContext>();

        Register(this, Messages.MSG_LOGOUT, () => {
            Logout();
            NavigateTo<LoginViewModel, User, PridContext>();
        });

        // si un user est logé alors on passe par ce NavigateTo
        Register<User>(this, Messages.MSG_LOGIN, user => {
            Login(user);
            NavigateTo<MainViewModel, User, PridContext>();
        });
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
        //Test Balances
        var tricountsList = Context.Tricounts.ToList();
        tricountsList.ForEach(tri => {
            tri.RefreshBalance();
        });

        Context.SaveChanges();
        // Un endroit pour tester vos requêtes LINQ
    }
}