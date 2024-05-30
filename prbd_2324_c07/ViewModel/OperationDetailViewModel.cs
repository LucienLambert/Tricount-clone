using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel;

public class OperationDetailViewModel : ViewModelBase<User, PridContext>
{

    private Tricount _tricount;
    public Tricount Tricount {
        get => _tricount;
        set => SetProperty(ref _tricount, value);
    }

    // Pour bien faire, devrait peut-être être dans un convertisseur
    public string SaveBtnLabel => IsNewOperation ? "Add" : "Save";

    public string HeaderLabel => IsNewOperation ? "Add Operation" : "Edit Operation";

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

    public string Title {
        get => Operation?.Title;
        set => SetProperty(Operation?.Title, value, Operation, (o, t) => {
            o.Title = t;
            Validate();
        });
    }

    private User _initiator;
    public User Initiator {
        get => _initiator;
        set => SetProperty(ref _initiator, value);
    }

    public DateTime CreatedAt {
        get => (DateTime)Operation?.Operation_date;
        set => SetProperty(Operation?.Operation_date, value, Operation, (o, c) => {
            o.Operation_date = (DateTime)c;
            Validate();
        });
    }

    private string _amount;
    public string Amount {
        get => _amount;
        set => SetProperty(ref _amount, value, () => {
            Validate();
        });
    }

    private DateTime _datePicker;
    public DateTime DatePicker {
        get => _datePicker;
        set => SetProperty(ref _datePicker, value);
    }

    //création d'une liste pour gerer les Participants au tricount
    private ObservableCollectionFast<User> _participantsOperation;
    public ObservableCollectionFast<User> ParticipantsOperation {
        get => _participantsOperation;
        set => SetProperty(ref _participantsOperation, value);
    }

    //liste utiliser pour instancer une OperationParticipantViewModel pour chaques users (Participant)
    private OperationParticipantViewModel _participantsOperationVM;
    public OperationParticipantViewModel ParticipantsOperationVM {
        get => _participantsOperationVM;
        set => SetProperty(ref _participantsOperationVM, value);
    }

    // Dictionnaire pour stocker les poids des utilisateurs en attente d'etre modifié ou sauvé
    private Dictionary<User, double> _temporaryRepartition = new();
    public Dictionary<User, double> TemporaryRepartition {
        get => _temporaryRepartition;
        set => SetProperty(ref _temporaryRepartition, value);
    }

    public ICommand BtnCancel { get; set; }
    public ICommand BtnSave { get; set; }

    // Constructeur Add Operation
    public OperationDetailViewModel(Tricount tricount) {
        Tricount = tricount;
        ParticipantsOperationVM = new OperationParticipantViewModel(Tricount);
        IsNewOperation = true;
        InitializedAddOperation();

        // Reçoit le participant et son poid lorsque celui-ci a été changé et actualise la Répartition temporaire, puis la renvoie à OperationParticipantCard
        Register<Dictionary<User, double>>(App.Messages.MSG_OPERATION_USER_WEIGHT_CHANGED, dic => {
            var usr = dic.Keys.ElementAt(0);
            var wght = dic.Values.ElementAt(0);
            if (wght > 0) {
                TemporaryRepartition[usr] = wght;
            } else if (wght == 0 && TemporaryRepartition.ContainsKey(usr)) {
                TemporaryRepartition.Remove(usr);
            }
            NotifyColleagues(App.Messages.MSG_OPERATION_TEMPORARY_REPARTITION_CHANGED, TemporaryRepartition);
        });
    }

    // Constructeur Edit Operation
    public OperationDetailViewModel(Operation operation) {
        Operation = operation;
        Tricount = Operation.Tricount;
        ParticipantsOperationVM = new OperationParticipantViewModel(Operation);
        IsNewOperation = false;
        InitializeEditOperation();

        // Reçoit le participant et son poid lorsque celui-ci a été changé et actualise la Répartition temporaire, puis la renvoie à OperationParticipantCard
        Register<Dictionary<User, double>>(App.Messages.MSG_OPERATION_USER_WEIGHT_CHANGED, dic => {
            var usr = dic.Keys.ElementAt(0);
            var wght = dic.Values.ElementAt(0);
            if (wght > 0) {
                TemporaryRepartition[usr] = wght;
            } else if (wght == 0 && TemporaryRepartition.ContainsKey(usr)) {
                TemporaryRepartition.Remove(usr);
            }
            NotifyColleagues(App.Messages.MSG_OPERATION_TEMPORARY_REPARTITION_CHANGED, TemporaryRepartition);
        });

    }


    private void InitializedAddOperation() {
        BtnCancel = new RelayCommand(CancelAction);
        BtnSave = new RelayCommand(SaveAction, CanSaveAction);
        Operation = new Operation();
        ParticipantsOperation = new ObservableCollectionFast<User>();
        foreach (var participant in Tricount.Subscriptions) {
            ParticipantsOperation.Add(participant.User);
            // chaque utilisateur a un poid de 1 à l'initialisation
            TemporaryRepartition.Add(participant.User, 1);
        }

        CreatedAt = DateTime.Now;
        Initiator = CurrentUser;
        DatePicker = DateTime.Now;
        Amount = "0,00";
    }

    private void InitializeEditOperation() {
        BtnCancel = new RelayCommand(CancelAction);
        BtnSave = new RelayCommand(SaveAction, CanSaveAction);
        ParticipantsOperation = new ObservableCollectionFast<User>();
        foreach (var participant in Tricount.Subscriptions) {
            ParticipantsOperation.Add(participant.User);
        }
        Initiator = Operation.Initiator;
        CreatedAt = Operation.Operation_date;
        Amount = Operation.Amount.ToString();
        TemporaryRepartition = Operation.GetRepartitions();
    }


    public override bool Validate() {
        ClearErrors();
        Operation.Validate(Amount);
        AddErrors(Operation.Errors);

        if (!Operation.ValidateAmount(Amount)) {
            NotifyColleagues(App.Messages.MSG_OPERATION_AMOUNT_CHANGED, double.Parse(Amount));
        }
        return !HasErrors;
    }

    public override void CancelAction() {
        ClearErrors();
        if (IsNewOperation) {
            Operation.Title = null;
            IsNewOperation = false;
        } else {
            Operation.Reload();
            //notife qui changement à été fait et déclanche une MAJ de l'interface.
            RaisePropertyChanged();
        }
        ParticipantsOperation.Clear();
        NotifyColleagues(App.Messages.MSG_CLOSE_OPERATION);
    }
    public bool CanSaveAction() {

        return Validate() && !HasErrors;
    }

    public override void SaveAction() {

        if (IsNewOperation) {
            Operation.Amount = double.Parse(Amount);
            Operation.Tricount = Tricount;
            Operation.Initiator = Initiator;
            Context.Add(Operation);

            foreach (var entry in TemporaryRepartition) {
                // Value = double (poid), Key = User
                Context.Add(new Repartition((int)entry.Value, entry.Key, Operation));
            }

        } else {
            Operation.Amount = double.Parse(Amount);
            Operation.Initiator = Initiator;
            var usersToUpdate = TemporaryRepartition.Keys;

            // Si le user de Repartitions est dans le dictionnaire, modifie son poid, sinon le supprime
            foreach (var repartition in Context.Repartitions.Where(rep=>rep.Operation == Operation).ToList()) {
                if (usersToUpdate.Contains(repartition.User)) {
                    repartition.Weight = (int)TemporaryRepartition[repartition.User];
                } else {
                    Context.Repartitions.Remove(repartition);
                }

            }

            Context.SaveChanges();

            NotifyColleagues(App.Messages.MSG_CLOSE_OPERATION);
            NotifyColleagues(App.Messages.MSG_OPERATION_CHANGED);

        }
    }
}
