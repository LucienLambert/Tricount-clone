using prbd_2324_c07.Model;
using PRBD_Framework;

namespace prbd_2324_c07.ViewModel
{
    public class TricountGraphBarViewModel : ViewModelBase<User, PridContext>
    {
        public User Participant;

        private double _amount;

        public double Amount {
            get => _amount;
            set => SetProperty(ref _amount, value);
        }

        public Dictionary<int, float> Balance;

        public string UserName => Participant == CurrentUser ? Participant.FullName + " (me)" : Participant.FullName;

        private double _rectangleWidth;

        public double RectangleWidth {
            get => _rectangleWidth;
            set => SetProperty(ref _rectangleWidth, value);
        }

        private double CalculateRectangleWidth() {

            if (Amount < 0) {
                var total = Balance
                    .Where(kvp => kvp.Value < 0)
                    .Select(kvp => Math.Abs(kvp.Value))
                    .Sum();
                return (Math.Abs(Amount) / total) * 200;


            } else if (Amount > 0) {

                var total = Balance
                    .Where(kvp => kvp.Value > 0)
                    .Select(kvp => Math.Abs(kvp.Value))
                    .Sum();
                return (Amount / total) * 200;

            }
            return 0;
        }

        public TricountGraphBarViewModel(User user, double amount, Dictionary<int, float> balance) {
            Participant = user;
            Amount = amount;
            Balance = balance;
            RectangleWidth = CalculateRectangleWidth();
            OnRefreshData();
        }
    }
}
