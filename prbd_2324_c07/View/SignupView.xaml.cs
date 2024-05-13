using PRBD_Framework;
using System.Windows;

namespace prbd_2324_c07.View
{
    public partial class SignupView : WindowBase
    {
        public SignupView() {
            InitializeComponent();
        }

        private void BtnCancel(object sender, RoutedEventArgs e) {

            NotifyColleagues(App.Messages.MSG_SIGNUP_CANCELED);
        }
    }
}
