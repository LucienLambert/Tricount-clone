using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Net.Mail;
using System.Security.Policy;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel
{
    public class SignupViewModel : ViewModelBase<User, PridContext>
    {


        private string _name;
        private string _mail;
        private string _password;
        private string _confirmPassword;

        public string Name {
            get => _name;
            set => SetProperty(ref _name, value, () => ValidateName());
        }

        public string Mail {
            get => _mail;
            set => SetProperty(ref _mail, value, () => ValidateMail());
        }

        public string Password {
            get => _password;
            set => SetProperty(ref _password, value, () => ValidatePassword());
        }

        public string ConfirmPassword {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value, () => ValidateConfirmPassword());
        }

        public ICommand SignupButton { get; set; }


        public SignupViewModel() {
            SignupButton = new RelayCommand(SignupAction);
        }

        public override bool Validate() {
            ClearErrors();
            return ValidateName()
                && ValidateMail()
                && ValidatePassword()
                && ValidateConfirmPassword();
        }

        private bool ValidateName() {

            ClearErrors();

            var user = Context.Users.FirstOrDefault(user => user.FullName == Name);

            if (string.IsNullOrEmpty(Name)) {
                AddError(nameof(Name), "required");
            } else if (Name.Length < 3) {
                AddError(nameof(Name), "length must be >= 3");
            } else if (user != null) {
                AddError(nameof(Name), "name already exists");
            } else if (Name.Length > 50) {
                AddError(nameof(Name), "name way too long");
            }
            return !HasErrors;
        }

        private bool ValidateMail() {

            ClearErrors();

            var emailRegex = new System.Text.RegularExpressions.Regex(@"^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$");
            var user = Context.Users.FirstOrDefault(user => user.Mail == Mail);

            if (string.IsNullOrEmpty(Mail)) {
                AddError(nameof(Mail), "required");
            } else if (!emailRegex.IsMatch(Mail)) {
                AddError(nameof(Mail), "invalid format");
            } else if (user != null){
                AddError(nameof(Mail), "email address already exists");
            }

            return !HasErrors;
        }


        private bool ValidatePassword() {

            ClearErrors();

            var passwordRegex = new System.Text.RegularExpressions.Regex(@"^(?=.*[0-9])(?=.*[A-Z])(?=.*\W).+$");

            if (string.IsNullOrEmpty(Password)) {
                AddError(nameof(Password), "required");
            } else if (!passwordRegex.IsMatch(Password)) {
                AddError(nameof(Password), "requires at least 8 char, 1 number, 1 uppercase");
            }


            return !HasErrors;
        }

        private bool ValidateConfirmPassword() {

            ClearErrors();

            if (Password != ConfirmPassword) {
                AddError(nameof(ConfirmPassword), "passwords do not match");
            }
            return !HasErrors;
        }
        private void SignupAction() {
            if (Validate()) {
                var hashedPass = SecretHasher.Hash(Password);

                User newuser = new User(Mail, hashedPass, Name);

                Context.Users.Add(newuser);
                Context.SaveChanges();

                NotifyColleagues(App.Messages.MSG_LOGIN, newuser);

            }

        }
    }
}
