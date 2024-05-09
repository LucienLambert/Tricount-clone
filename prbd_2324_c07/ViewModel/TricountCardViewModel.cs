using prbd_2324_c07.Model;
using prbd_2324_c07.View;
using PRBD_Framework;
namespace prbd_2324_c07.ViewModel
{
    public class TricountCardViewModel : ViewModelBase<User, PridContext> {
        private readonly Tricount _tricount;

        public Tricount Tricount {
            get => _tricount;
            private init => SetProperty(ref _tricount, value);
        }

        public string Title => Tricount.Title;
        public string Description => Tricount.Description;
        public DateTime CreatedAt => Tricount.CreatedAt;
        public DateTime? LastOpDate => Tricount.GetLastOperationDate();
        public User Creator => Tricount.Creator;
        public string NumberOfFriends => GenerateNumberOfFriends();
        public string NumberOfOperations => GenerateNumberOfOperations();
        public double TotalExpenses => Tricount.TotalExpenses();
        public double UserExpenses => Tricount.GetUserExpenses(CurrentUser);
        public double UserBalance => Tricount.GetUserBalance(CurrentUser);


        public TricountCardViewModel(Tricount tricount) : base() {
            Tricount = tricount;
        }


        public string GenerateNumberOfFriends() {

            var number = Tricount.NumberOfFriends();

            return number switch {
                0 => "With no friend",
                1 => "With 1 friend",
                > 1 => "With " + number + " friends",
                _ => "error",
            };
        }

        public string GenerateNumberOfOperations() {
            var number = Tricount.NumberOfOperations();
            return number switch {
                0 => "no operation",
                1 => "1 operation",
                > 1 => number + " operations",
                _ => "error",
            };
        }

    }
}