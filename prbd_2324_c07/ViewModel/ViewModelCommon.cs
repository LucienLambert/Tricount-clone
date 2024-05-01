using prbd_2324_c07.Model;

namespace prbd_2324_c07.ViewModel;

public abstract class ViewModelCommon : PRBD_Framework.ViewModelBase<User, PridContext>{

    //recup le user connecté et return true s'il est Admin
    public static bool isAdmin => IsLoggedIn && CurrentUser is Administrator;

    //recup le user connecté et return true s'il n'est pas Admin
    public static bool isNotAdmin => !isAdmin;
}

