using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static prbd_2324_c07.App;


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

        public ICommand AddOperation { get; set; }


        public OperationsViewModel(Tricount tricount) : base() {
            Tricount = tricount;
            AddOperation = new RelayCommand(AddOperationAction);

            OnRefreshData();

            Register(Messages.MSG_OPERATION_CHANGED, OnRefreshData);

            Register(Messages.MSG_RELOAD_ASKED, OnRefreshData);

        }

        protected void AddOperationAction() {

            NotifyColleagues(Messages.MSG_NEW_OPERATION, Tricount);

        }

        protected override void OnRefreshData() {


            // Pour les convertisseurs
            if (Tricount.Operations.Count() == 0) {
                Operations = null;
            } else {
                var operations = Tricount.Operations.OrderByDescending(op => op.Operation_date);
                Operations = new ObservableCollection<OperationCardViewModel>(operations.Select(operation => new OperationCardViewModel(operation)));
            }


        }




    }
}
