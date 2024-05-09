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

        //public string Title => Tricount.Title;
        //public string Description => Tricount.Description;
        //public DateTime CreatedAt => Tricount.CreatedAt;
        //public DateTime? LastOpDate => Tricount.GetLastOperationDate();
        //public User Creator => Tricount.Creator;


        public OperationCardViewModel(Operation operation) : base() {
            Operation = operation;
        }


    }
}