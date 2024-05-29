using prbd_2324_c07.Model;
using PRBD_Framework;

namespace prbd_2324_c07.ViewModel
{
    public class ParticipantsOperationViewModel : ViewModelBase<User,PridContext>
    {
        private Tricount _tricount;
        public Tricount Tricount {
            get => _tricount;
            set => SetProperty(ref _tricount, value);
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

        public ParticipantsOperationViewModel(Tricount tricount) {
            Tricount = tricount;
            ListUsers = new ObservableCollectionFast<User>();
            foreach (var s in Tricount.Subscriptions) {
                ListUsers.Add(s.User);
            }

            OnRefreshData();
        }

        protected override void OnRefreshData() {

            OperationParticipantCardVMs = new ObservableCollectionFast<OperationParticipantCardViewModel>(ListUsers.Select(user => new OperationParticipantCardViewModel(user)));
            
        }



    }
}
