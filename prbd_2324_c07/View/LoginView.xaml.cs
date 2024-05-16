using prbd_2324_c07;
//DEBUG, Model à retirer
using prbd_2324_c07.Model;
using prbd_2324_c07.ViewModel;
using PRBD_Framework;
using System.Windows;
using System.Windows.Controls;

namespace prbd_2324_c07.View;

public partial class LoginView : WindowBase {

    public LoginView() {
        InitializeComponent();
    }

    private void BtnCancel(object sender, RoutedEventArgs e) {
        Close();
    }
    //DEBUG
    public void FastLogin(object param, SelectionChangedEventArgs e) {
        var context = this.DataContext as LoginViewModel;
        context.FastLoginAction(param);
    }

}