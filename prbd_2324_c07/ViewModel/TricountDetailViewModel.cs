using Microsoft.IdentityModel.Tokens;
using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Configuration;
using System.Windows.Controls;
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
            //NotifyColleagues(App.Messages.MSG_TRICOUNT_CHANGED, Tricount);
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

    public ICommand BtnCancel { get; set; }
    public ICommand BtnSave { get; set; }

    // --------- Gestion Participant ---------

    public ICommand AddUserCommand { get; set; }
    public ICommand AddMySelfCommand { get; set; }
    public ICommand AddEveryBodyCommand { get; set; }
    public ICommand DelUserCommand { get; set; }

    private User _userSelected;
    public User UserSelected {
        get => _userSelected;
        set => SetProperty(ref _userSelected, value);
    }

    private ObservableCollectionFast<User> _non_Participant;
    public ObservableCollectionFast<User> Non_Participant {
        get => _non_Participant;
        set => SetProperty(ref _non_Participant, value);
    }

    private ObservableCollectionFast<User> _participant;
    public ObservableCollectionFast<User> Participant {
        get => _participant;
        set => SetProperty(ref _participant, value);
    }

    

    //public TricountDetailViewModel() {
    //    //Console.WriteLine("constructeur vide");
    //    //Tricount = new Tricount();
    //    //IsNew = true;
    //    //IgnitializeDataView();
    //}

    public TricountDetailViewModel(Tricount tricount, bool isNew) {
        Tricount = tricount;
        IsNew = isNew;
        InitializeDataView();
        RaisePropertyChanged();
    }

    private void InitializeDataView() {
        BtnCancel = new RelayCommand(CancelAction, CanCancelAction);
        BtnSave = new RelayCommand(SaveAction, CanSaveAction);
        AddUserCommand = new RelayCommand(AddAction);
        AddMySelfCommand = new RelayCommand(AddMySelfAction);
        AddEveryBodyCommand = new RelayCommand(AddEveryBodyAction);
        DelUserCommand = new RelayCommand<User>(DelAction);

        Participant = new ObservableCollectionFast<User>();
        Non_Participant = new ObservableCollectionFast<User>();

        CreatedAt = DateTime.Now;
        HeaderDefaultSet();
        
        ListNon_Participant();
        ListParticipant();
    }

    private void HeaderDefaultSet() {
        if (IsNew) {
            DefaultHeader = $"<New Tricount> - No Description\nCreated by {CurrentUser.FullName} on {DateTime.Now.Date.ToString("dd/MM/yyyy")}";
        } else {
            DefaultHeader = $"{Tricount.Title} - {Tricount.Description}\nCreated by {Tricount.Creator.FullName} on {Tricount.CreatedAt.ToString("dd/MM/yyyy")}";
        }
    }

    public override bool Validate() {
        ClearErrors();

        Tricount.Validate();
        
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
            RaisePropertyChanged();
        }
    }

    private bool CanCancelAction() {
        return Tricount != null && (IsNew || Tricount.IsModified);
    }

    public override void SaveAction() {
        if (IsNew) {
            Tricount.CreatorId = CurrentUser.UserId;
            IsNew = false;
        }
        Context.Add(Tricount);
        foreach (var u in Participant) {
            Tricount.AddUserSubTricount(u);
        }

        foreach (var u in Tricount.Subscriptions) {
            Console.WriteLine(u.User.FullName);
        }
        Context.AddRange(Tricount.Subscriptions);
        Context.SaveChanges();
        RaisePropertyChanged();
        NotifyColleagues(App.Messages.MSG_TRICOUNT_CHANGED, Tricount);
    }

    private bool CanSaveAction() {
        if (IsNew)
            return Tricount.Validate() && !HasErrors;
        return Tricount != null && Tricount.IsModified && !HasErrors;
    }

    private void ListNon_Participant() {
        var listUser = Context.Users.OrderBy(u => u.FullName);
        if (IsNew) {
            foreach (var user in listUser) {
                Non_Participant.Add(user);
            }
        } else {
            var nonSubscribedUsers = Context.Users
        .Where(u => !Context.Subscriptions.Any(s => s.UserId == u.UserId && s.TricountId == Tricount.TricountId));
            foreach (var user in nonSubscribedUsers) {
                Non_Participant.Add(user);
            }
        } 
        Non_Participant.Remove(CurrentUser);
    }

    private void ListParticipant() {
        if (IsNew) {
            Participant.Add(CurrentUser);
        } else {
            foreach (Subscription sub in Tricount.Subscriptions) {
                Participant.Add(sub.User);
            }
        }
    }

    private void AddAction() {
        if(UserSelected != null) {
            //Tricount.AddUserSubTricount(UserSelected);
            Participant.Add(UserSelected);
            Non_Participant.Remove(UserSelected);
        }
    }

    private void AddMySelfAction() {
        if (!Participant.Contains(CurrentUser)) {
            //Tricount.AddUserSubTricount(CurrentUser);
            Participant.Add(CurrentUser);
            Non_Participant.Remove(CurrentUser);
        }
    }

    private void AddEveryBodyAction() {
        if(!Non_Participant.IsNullOrEmpty()) {
            foreach(var p in Non_Participant) { 
                //Tricount.AddUserSubTricount(p);
                Participant.Add(p);
            }
            Non_Participant.Clear();
        }
    }

    private void DelAction(User user) {
        if (IsNew) {
            if (!user.Equals(CurrentUser)) {
                Participant.Remove(user);
                Non_Participant.Add(user);
            }
        } else {
            if(user.UserId != Tricount.CreatorId) {
                Participant.Remove(user);
                Non_Participant.Add(user);
                Tricount.RemoveUserSubTricount(user);
            }
        }
    }
}