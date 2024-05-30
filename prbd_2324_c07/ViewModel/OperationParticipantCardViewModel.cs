using prbd_2324_c07.Model;
using PRBD_Framework;

namespace prbd_2324_c07.ViewModel
{
    public class OperationParticipantCardViewModel : ViewModelBase<User, PridContext>
    {

        //private Tricount _tricount;
        //public Tricount Tricount {
        //    get => _tricount;
        //    set => SetProperty(ref _tricount, value);
        //}

        private User _participant;
        public User Participant {
            get => _participant;
            set => SetProperty(ref _participant, value);
        }

        private Dictionary<User, double> _repartition = new();
        public Dictionary<User, double> Repartition {
            get => _repartition;
            set => SetProperty(ref _repartition, value, RefreshBaseAmount);
        }

        private double _amount;
        public double Amount {
            get => _amount;
            set => SetProperty(ref _amount, value, RefreshBaseAmount);
        }

        private double _baseAmount;
        public double BaseAmount {
            get => _baseAmount;
            set => SetProperty(ref _baseAmount, value, RefreshUserAmount);
        }

        private double _userAmount;
        public double UserAmount {
            get => _userAmount;
            set => SetProperty(ref _userAmount, value);
        }

        private double _userWeight;
        public double UserWeight {
            get => _userWeight;
            set => SetProperty(ref _userWeight, value, UserWeightChanged);
        }

        private bool _checkedBox;
        public bool CheckedBox {
            get => _checkedBox;
            set => SetProperty(ref _checkedBox, value, CheckedChanged);
        }

        private void UserWeightChanged() {
            if (UserWeight == 0) {
                CheckedBox = false;
            } else {
                CheckedBox = true;
            }
            // Notifie OperationDetailViewModel et lui passe un Dictionnaire contenant le Participant et son Poid pour qu'il mette à jour la Répartition temporaire
            NotifyColleagues(App.Messages.MSG_OPERATION_USER_WEIGHT_CHANGED, new Dictionary<User, double> { { Participant, UserWeight } });
            RefreshUserAmount();
        }


        private void CheckedChanged() {
            if (!CheckedBox) {
                UserWeight = 0;
                RefreshUserAmount();
            } else {
                UserWeight = 1;
                RefreshUserAmount();
            }
        }

        private void RefreshBaseAmount() {
            // Montant de base, calculé en divisant Amount de la somme de tous les poids dans la répartition
            BaseAmount = Amount / Repartition.Values.Sum();
        }

        private void RefreshUserAmount() {
            // BaseAmount * le poid du participant
            UserAmount = Math.Round(BaseAmount * UserWeight, 2);
        }




        public OperationParticipantCardViewModel(User user, double participantCount) {
            Participant = user;
            InitializeAddOperation(participantCount);

            Register<double>(App.Messages.MSG_OPERATION_AMOUNT_CHANGED, amount => {
                Amount = amount;
            });

            Register<Dictionary<User, double>>(App.Messages.MSG_OPERATION_TEMPORARY_REPARTITION_CHANGED, temprep => {
                Repartition = temprep;
                // Je ne comprends pas pourquoi la méthode dans le setter de Repartition ne proc pas, du coup j'ai du la remettre ici
                RefreshBaseAmount();
            });
        }

        private void InitializeAddOperation(double participantCount) {

            BaseAmount = 0;
            UserWeight = 1;
            CheckedBox = true;
            // Pour l'initialisation, il faut le nombre de participants pour pouvoir calculer les UserAmount de chacun
            for(int i = 0; i < participantCount; ++i) {
                Repartition.Add(new User(), 1);
            }


        }

    }
}
