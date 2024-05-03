using prbd_2324_c07.Model;
using System.Windows.Input;
using PRBD_Framework;
using System.Windows;

namespace prbd_2324_c07.ViewModel;

public class LoginViewModel : ViewModelBase<User, PridContext> {

    private string _pseudo;
    private string _password;

    public string Pseudo {
        get => _pseudo;
        set => SetProperty(ref _pseudo, value, () => Validate());
    }

    public string Password {
        get => _password;
        set => SetProperty(ref _password, value, () => Validate());
    }

    public ICommand LoginButton {  get; set; }

    public LoginViewModel() {
        LoginButton = new RelayCommand(LoginAction);
    }

    public override bool Validate() {
        ClearErrors();

        var user = Context.Users.FirstOrDefault(user => user.FullName == Pseudo);

        if (string.IsNullOrEmpty(Pseudo)) {
            AddError(nameof(Pseudo), "required");
        } else if (Pseudo.Length < 3) {
            AddError(nameof(Pseudo), "length must be >= 3");
        } else if (user == null) {
            AddError(nameof(Pseudo), "does not exist");
        } else {
            if (string.IsNullOrEmpty(Password)) {
                AddError(nameof(Password), "required");
            } else if (Password.Length < 8) {
                AddError(nameof(Password), "lenght must be >= 8");
            } else if (!SecretHasher.Verify(Password, user.Password)) {
                AddError(nameof(Password), "Wrong password");
            }
        }
            return !HasErrors;
    }

    private void LoginAction() {
        if (Validate()) { 
            var user = Context.Users.FirstOrDefault(u => u.FullName == Pseudo);
            NotifyColleagues(App.Messages.MSG_LOGIN, user);
            Console.Write("Connexion réussi");
        }
    }

    protected override void OnRefreshData() {
        // pour plus tard
    }
}