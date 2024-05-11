using prbd_2324_c07.Model;
using prbd_2324_c07.View;
using PRBD_Framework;
namespace prbd_2324_c07.ViewModel
{
    public class OperationCardViewModel : ViewModelBase<User, PridContext>
    {
        private readonly Operation _operation;

        public Operation Operation {
            get => _operation;
            private init => SetProperty(ref _operation, value);
        }


        public double OperationAmount => Math.Round(Operation.Amount,2);


        public OperationCardViewModel(Operation operation) : base() {
            Operation = operation;
        }


    }
}