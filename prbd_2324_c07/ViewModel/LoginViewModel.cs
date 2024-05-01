using Microsoft.IdentityModel.Tokens;
using prbd_2324_c07.Model;
using System.Windows.Controls;
using Newtonsoft.Json;

namespace prbd_2324_c07.ViewModel;

public class LoginViewModel : PRBD_Framework.ViewModelBase<User, PridContext> {

    private string _pseudo;
    private string _password;

    public LoginViewModel() {

    }

    public string Pseudo {
        get => _pseudo;
        set => SetProperty(ref _pseudo, value, () => Validation());
    }

    public string Password {
        get => _password;
        set => SetProperty(ref _password, value, () => Validation());
    }

    public bool Validation() {
        ClearErrors();

        var user = Context.Users.FirstOrDefault(user => user.FullName == Pseudo);
        Console.WriteLine(user);

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
            } else if (user.Password != Password) {
                AddError(nameof(Password), "Wrong password");
            }
        }

            return !HasErrors;
        }

    protected override void OnRefreshData() {
        // pour plus tard
    }
}