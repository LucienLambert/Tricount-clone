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
        public double TotalExpenses => Math.Round(Tricount.TotalExpenses(), 2);

        public TricountCardViewModel(Tricount tricount) {
            Tricount = tricount;
        }
        public string GenerateNumberOfFriends() {

            var number = CalculateNumberOfFriends();
            
            switch(number) {
                case 0:
                    return "With no friend";
                case 1:
                    return "With 1 friend";
                case > 1:
                    return "With " + number + " friends";
            }
            return "error";
        }

        // Nombre d'amis = participants moins celui qui regarde le Tricount
        public int CalculateNumberOfFriends() {
            if (ViewModelCommon.isAdmin && Tricount.CreatorId != CurrentUser.UserId)
                return Tricount.NumberOfParticipants();

            return Tricount.NumberOfParticipants() - 1;
        }

        public string GenerateNumberOfOperations() {
            var number = Tricount.NumberOfOperations();
            switch(number) {
                case 0:
                    return "no operation";
                case 1:
                    return "1 operation";
                case > 1:
                    return number + " operations";
            }
            return "error";
        }

    }
}