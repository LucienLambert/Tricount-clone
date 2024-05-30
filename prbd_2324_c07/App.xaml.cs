using prbd_2324_c07.Model;
using prbd_2324_c07.ViewModel;
using System.Windows;
using System.Globalization;
using PRBD_Framework;
using prbd_2324_c07.View;

namespace prbd_2324_c07;

public partial class App : ApplicationBase<User,PridContext>{
    public enum Messages{
        MSG_NEW_USER,
        MSG_SIGNUP_CANCELED,
        MSG_NEW_TRICOUNT,
        MSG_TRICOUNT_CHANGED,
        MSG_OPERATION_CHANGED,
        MSG_FULLNAME_CHANGED,
        MSG_USER_CHANGED,
        MSG_DISPLAY_USER,
        MSG_DISPLAY_TRICOUNT,
        MSG_CLOSE_TAB,
        MSG_LOGIN,
        MSG_LOGOUT,
        MSG_RELOAD_ASKED,
        MSG_RESET_ASKED,
        MSG_REVOME_PARTICIPANT,
        MSG_ADD_PARTICIPANT,
        MSG_NEW_OPERATION,
        MSG_EDIT_OPERATION,
        MSG_CLOSE_OPERATION,
        MSG_OPERATION_AMOUNT_CHANGED,
        MSG_OPERATION_USER_WEIGHT_CHANGED,
        MSG_OPERATION_TEMPORARY_REPARTITION_CHANGED
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
        //TestQueries();

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

        Register(this, Messages.MSG_NEW_USER, () => {
            NavigateTo<SignupViewModel, User, PridContext>();
        });

        Register(this, Messages.MSG_SIGNUP_CANCELED, () => {
            NavigateTo<LoginViewModel, User, PridContext>();
        });

        Register(this, Messages.MSG_RESET_ASKED, () => {
            App.ClearContext();
            PrepareDatabase();
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
        if(CurrentUser?.FullName != null) {
            CurrentUser = User.GetByFullName(CurrentUser.FullName);
        } else {
            CurrentUser = null;
        }
        // TODO
    }

    private static void TestQueries() {

        var tricountsList = Context.Tricounts.ToList();
        tricountsList.ForEach(tri => {
            tri.RefreshBalance();
        });

        Context.SaveChanges();
        // Un endroit pour tester vos requêtes LINQ
    }
}