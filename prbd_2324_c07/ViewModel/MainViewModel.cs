using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel;

public class MainViewModel : ViewModelBase<User, PridContext> {

    public ICommand BtnReload {  get; set; }
    public ICommand BtnReset { get; set; }
    public ICommand BtnLogout { get; set; }

    public MainViewModel() {
        BtnLogout = new RelayCommand(BtnLogoutActoin);
        BtnReload = new RelayCommand(BtnReloadAction);
        BtnReset = new RelayCommand(BtnResetAction);
    }
    
    public static string Title {
        get => $"My Tricount ({CurrentUser?.Mail})";
    }

    public void BtnLogoutActoin() {
        NotifyColleagues(App.Messages.MSG_LOGOUT);
    }

    private void BtnReloadAction() {
        NotifyColleagues(App.Messages.MSG_RELOAD_ASKED);
    }

    private void BtnResetAction() {
        NotifyColleagues(App.Messages.MSG_RESET_ASKED);
    }
    //MAJ
}

