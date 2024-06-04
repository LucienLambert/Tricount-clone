using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel;

public class TricountDetailViewModel : ViewModelBase<User, PridContext> {

    // --------- Gestion TricountView ---------

    private string _defaultHeader;

    public string DefaultHeader {
        get => _defaultHeader;
        set => SetProperty(ref _defaultHeader, value);
    }

    private Tricount _tricount;
    public Tricount Tricount {
        get => _tricount;
        set => SetProperty(ref _tricount, value);
    }

    private bool _isNew;
    public bool IsNew {
        get => _isNew;
        set => SetProperty(ref _isNew, value);
    }
   
    public string Title {
        get => Tricount?.Title;
        set => SetProperty(Tricount.Title, value, Tricount, (t, v) => {
            t.Title = v;
            Validate();
        });
    }

    public string Description {
        get => Tricount?.Description;
        set => SetProperty(Tricount?.Description, value, Tricount, (t, d) => {
            t.Description = d;
            Validate();
        });
    }

    public DateTime CreatedAt {
        get => (DateTime)Tricount?.CreatedAt;
        set => SetProperty(Tricount?.CreatedAt, value, Tricount, (t, c) => {
            t.CreatedAt = (DateTime)c;
            Validate();
        });
    }

    private DateTime _DatePicker;
    public DateTime DatePicker {
        get => _DatePicker;
        set => SetProperty(ref _DatePicker, value);
    }

    //ajout
    private ParticipantsViewModel _participantVM;
    public ParticipantsViewModel ParticipantVM {
        get => _participantVM;
        set => SetProperty(ref _participantVM, value);
    }

    public ICommand BtnCancel { get; set; }
    public ICommand BtnSave { get; set; }

    private ObservableCollectionFast<User> listParticipant;

  

    public TricountDetailViewModel(Tricount tricount, bool isNew) {
        Tricount = tricount;
        ParticipantVM = new ParticipantsViewModel(Tricount, isNew);
        IsNew = isNew;
        InitializeDataView();
    }

    private void InitializeDataView() {
        BtnCancel = new RelayCommand(CancelAction);
        BtnSave = new RelayCommand(SaveAction, CanSaveAction);

        if (IsNew) {
            CreatedAt = DateTime.Now;
        }
        DatePicker = DateTime.Now;
        listParticipant = new ObservableCollectionFast<User>();

        HeaderDefaultSet();
    }

    private void HeaderDefaultSet() {       
        if (IsNew) {
            DefaultHeader = $" <New Tricount> - No Description\nCreated by {CurrentUser.FullName} on {DateTime.Now.Date.ToString("dd/MM/yyyy")}";
        } else {
            DefaultHeader = $"{Tricount.Title} - {Tricount.Description}\nCreated by {Tricount.Creator.FullName} on {Tricount.CreatedAt.ToString("dd/MM/yyyy")}";
        }
    }

    public override bool Validate() {
        ClearErrors();
        Tricount.Validate(CurrentUser); 
        AddErrors(Tricount.Errors);
        return !HasErrors;
    }

    public override void CancelAction() {
        ClearErrors();
        if (IsNew) {
            IsNew = false;
            NotifyColleagues(App.Messages.MSG_CLOSE_TAB, Tricount);
        } else {
            Tricount.Reload();
            NotifyColleagues(App.Messages.MSG_CLOSE_TAB, Tricount);
            NotifyColleagues(App.Messages.MSG_DISPLAY_TRICOUNT, Tricount);
        }
        listParticipant.Clear(); 
    }

    public override void SaveAction() {
        foreach (var p in ParticipantVM.Participant) {
            //check si l'utilisateur est déjà présent dans la liste des Subscriptions sinon add user dans subscription
            if (!Tricount.Subscriptions.Any(sub => sub.UserId == p.UserId)) {
                Tricount.AddUserSubTricount(p);
            }
        }
        if (IsNew) {
            Tricount.CreatorId = CurrentUser.UserId;
            Context.Add(Tricount);
            IsNew = false;
        } else {
            foreach (var p in ParticipantVM.TempoDelParticipants){
                if (Tricount.Subscriptions.Any(sub => sub.UserId == p.UserId)) {
                    Tricount.RemoveUserSubTricount(p);
                }
            }
        }
        Context.SaveChanges();
        //listParticipant.Clear();
        // Ajouter tous les participants qui ne sont pas déjà dans la liste des Subscription

        NotifyColleagues(App.Messages.MSG_TRICOUNT_CHANGED, Tricount);
        NotifyColleagues(App.Messages.MSG_CLOSE_TAB, Tricount);
        NotifyColleagues(App.Messages.MSG_DISPLAY_TRICOUNT, Tricount);
    }

    private bool CanSaveAction() {
        if (IsNew) {
            return Tricount.Validate() && !HasErrors;
        }
        return Tricount != null && !HasErrors && (Tricount.IsModified || ParticipantVM.IsChanged);
    }

    protected override void OnRefreshData() {
       
    }
}