using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;


namespace prbd_2324_c07.ViewModel
{
    public class OperationsViewModel : ViewModelBase<User, PridContext>
    {

        private Tricount _tricount;
        public Tricount Tricount {
            get => _tricount;
            set => SetProperty(ref _tricount, value);
        }

        private ObservableCollection<OperationCardViewModel> _operations;


        public ObservableCollection<OperationCardViewModel> Operations {
            get => _operations;
            set => SetProperty(ref _operations, value);
        }

        public ICommand EditOperation { get; set; }

        public ICommand NewOperation { get; set; }


        public OperationsViewModel(Tricount tricount) : base() {
            Tricount= tricount;
            OnRefreshData();

        }

        protected override void OnRefreshData() {

            Console.WriteLine(Tricount.Title);
            //a modifier
            var operations = Context.Operations;

            Operations = new ObservableCollection<OperationCardViewModel>(operations.Select(operation => new OperationCardViewModel(operation)));

        }




    }
}
