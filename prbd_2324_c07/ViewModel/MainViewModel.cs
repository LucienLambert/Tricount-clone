using prbd_2324_c07.Model;

namespace prbd_2324_c07.ViewModel;

public class MainViewModel : PRBD_Framework.ViewModelBase<User, PridContext> {

    public static string Title {
        get => $"My Tricount ({CurrentUser?.Mail})";
    }

    public MainViewModel() {
        
    }

    protected override void OnRefreshData() {
        // pour plus tard
    }

}

