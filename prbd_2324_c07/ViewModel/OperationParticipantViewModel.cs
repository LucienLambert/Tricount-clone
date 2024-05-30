using prbd_2324_c07.Model;
using PRBD_Framework;

namespace prbd_2324_c07.ViewModel
{
    public class OperationParticipantViewModel : ViewModelBase<User,PridContext>
    {
        private Tricount _tricount;
        public Tricount Tricount {
            get => _tricount;
            set => SetProperty(ref _tricount, value);
        }

        private Operation _operation;
        public Operation Operation {
            get => _operation;
            set => SetProperty(ref _operation, value);
        }

        private bool _isNewOperation;

        public bool IsNewOperation {
            get => _isNewOperation;
            set => SetProperty(ref _isNewOperation, value);
        }

        private ObservableCollectionFast<OperationParticipantCardViewModel> _operationParticipantsVMs;

        public ObservableCollectionFast<OperationParticipantCardViewModel> OperationParticipantCardVMs {
            get => _operationParticipantsVMs;
            set => SetProperty(ref _operationParticipantsVMs, value);
        }

        private ObservableCollectionFast<User> _listUsers;

        public ObservableCollectionFast<User> ListUsers {
            get => _listUsers;
            set => SetProperty(ref _listUsers, value);
        }

        // Constructeur en venant de Add Operation
        public OperationParticipantViewModel(Tricount tricount) {
            IsNewOperation = true;
            Tricount = tricount;
            ListUsers = new ObservableCollectionFast<User>();
            foreach (var s in Tricount.Subscriptions) {
                ListUsers.Add(s.User);
            }

            OnRefreshData();
        }

        // Constructeur en venant de Edit Operation
        public OperationParticipantViewModel(Operation operation) {
            IsNewOperation = false;
            Tricount = operation.Tricount;
            Operation = operation;
            OnRefreshData();
        }

        protected override void OnRefreshData() {
            if (IsNewOperation) {  
                OperationParticipantCardVMs = new ObservableCollectionFast<OperationParticipantCardViewModel>(ListUsers.Select(user => new OperationParticipantCardViewModel(user, ListUsers.Count())));
            } else {
                var repartition = Operation.GetRepartitions();
                OperationParticipantCardVMs = new ObservableCollectionFast<OperationParticipantCardViewModel>(repartition.Select(kvp => new OperationParticipantCardViewModel(kvp.Key, Operation)));
            }
        }



    }
}
