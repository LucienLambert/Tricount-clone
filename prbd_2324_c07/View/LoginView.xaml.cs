using prbd_2324_c07;
using PRBD_Framework;
using System.Windows;

namespace prbd_2324_c07.View;

public partial class LoginView : WindowBase {

    public LoginView() {
        InitializeComponent();
    }

    private void BtnCancel(object sender, RoutedEventArgs e) {
        Close();
    }

}