using prbd_2324_c07.Model;
using PRBD_Framework;

namespace prbd_2324_c07.ViewModel
{
    public class OperationParticipantViewModel : ViewModelBase<User, PridContext>
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

        private Dictionary<User, bool> _listUsers;

        // Pour la validation, le booléen fait référence à la case cochée dans OperationParticipantCard
        public Dictionary<User, bool> ListUsers {
            get => _listUsers;
            set => SetProperty(ref _listUsers, value);
        }


        public OperationParticipantViewModel(Tricount tricount, Operation operation) {
            if (tricount != null) {
                //ADD OPERATION
                IsNewOperation = true;
                Tricount = tricount;
                ListUsers = new Dictionary<User, bool>();

                foreach (var s in Tricount.Subscriptions) {
                    ListUsers.Add(s.User, true);
                }

                OnRefreshData();


            } else {
                //EDIT OPERATION
                IsNewOperation = false;
                Operation = operation;
                Tricount = Operation.Tricount;
                ListUsers = new Dictionary<User, bool>();


                foreach (var s in Tricount.Subscriptions) {
                    ListUsers.Add(s.User, false);
                }

                foreach (var rep in Operation.Repartitions) {
                    ListUsers[rep.User] = true;
                }

                OnRefreshData();
            }

            Register<Dictionary<User, bool>>(App.Messages.MSG_CHECKBOX_CHANGED, dic => {
                foreach (var entry in dic) {
                    ListUsers[entry.Key] = entry.Value;
                }
                Validate();
            });


        }

        public override bool Validate() {
            ClearErrors();

            if (!ListUsers.ContainsValue(true)) {
                AddError(nameof(this.OperationParticipantCardVMs), "You must check at least one participant!");
            } else {
                ClearErrors();
            }

            return !HasErrors;
            //NotifyColleagues(App.Messages.OPERATION_)
        }

        protected override void OnRefreshData() {
            //il faut modifier cette partie, dans tout les cas on doit faire un select sur listUser
            if (IsNewOperation) {
                OperationParticipantCardVMs = new ObservableCollectionFast<OperationParticipantCardViewModel>(ListUsers.Select(kvp => new OperationParticipantCardViewModel(kvp.Key, ListUsers.Count())));
            } else {
                //var repartition = Operation.GetRepartitions();
                OperationParticipantCardVMs = new ObservableCollectionFast<OperationParticipantCardViewModel>(ListUsers.Select(kvp => new OperationParticipantCardViewModel(kvp.Key, Operation)));
            }
        }



    }
}
