using prbd_2324_c07;
using PRBD_Framework;
using System.Windows;

namespace prbd_2324_c07.View;

public partial class MainView : WindowBase {
    
    public MainView() {
        InitializeComponent();
    }

    private void BtnMenuLogout(object sender, RoutedEventArgs e) {
        NotifyColleagues(App.Messages.MSG_LOGOUT);
    }
}

