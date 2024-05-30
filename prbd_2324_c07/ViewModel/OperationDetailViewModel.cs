using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel;

public class OperationDetailViewModel : ViewModelBase<User, PridContext> {

    private Tricount _tricount;
    public Tricount Tricount {
        get => _tricount;
        set => SetProperty(ref _tricount, value);
    }

    private Operation _operation;
    public  Operation Operation {
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
        set => SetProperty(Operation?.Title, value, Operation, (o,t) => {
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

    private DateTime _DatePicker;
    public DateTime DatePicker {
        get => _DatePicker;
        set => SetProperty(ref _DatePicker, value);
    }

    //création d'une liste pour gerer les Participants au tricount
    private ObservableCollectionFast<User> _participantsOperation;
    public ObservableCollectionFast<User> ParticipantsOperation  {
        get => _participantsOperation;
        set => SetProperty(ref _participantsOperation, value);
    }

    //liste utiliser pour instancer une OperationParticipantViewModel pour chaques users (Participant)
    private OperationParticipantViewModel _participantsOperationVM;
    public OperationParticipantViewModel ParticipantsOperationVM {
        get => _participantsOperationVM;
        set => SetProperty(ref _participantsOperationVM, value);
    }

    public ICommand BtnCancel { get; set; }

    // Constructeur Add Operation
    public OperationDetailViewModel(Tricount tricount) {
        Tricount = tricount;
        ParticipantsOperationVM = new OperationParticipantViewModel(Tricount);
        IsNewOperation = true;
        InitializedAddOperation();
        OnRefreshData();

    }

    // Constructeur Edit Operation
    public OperationDetailViewModel(Operation operation) {
        Operation = operation;
        IsNewOperation = false;
        //OnRefreshData();
    }

    private void InitializedAddOperation() {
        BtnCancel = new RelayCommand(CancelAction);
        Operation = new Operation();
        ParticipantsOperation = new ObservableCollectionFast<User>();
        foreach (var participant in Tricount.Subscriptions) {
            ParticipantsOperation.Add(participant.User);
        }

        CreatedAt = DateTime.Now;
        Initiator = CurrentUser;
        DatePicker = DateTime.Now;
        Amount = "0,00";
    }

    public override bool Validate() {
        ClearErrors();
        Operation.Validate(Amount);
        AddErrors(Operation.Errors);
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



}
