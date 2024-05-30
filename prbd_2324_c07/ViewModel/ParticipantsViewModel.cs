using Microsoft.IdentityModel.Tokens;
using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Windows.Input;
using static prbd_2324_c07.App;

namespace prbd_2324_c07.ViewModel;

public class ParticipantsViewModel : ViewModelBase<User, PridContext> {

    public ICommand AddUserCommand { get; set; }
    public ICommand AddMySelfCommand { get; set; }
    public ICommand AddEveryBodyCommand { get; set; }
    //public ICommand DelUserCommand { get; set; }

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

    private ObservableCollectionFast<User> _tempoDelParticipants;
    public ObservableCollectionFast<User> TempoDelParticipants {
        get => _tempoDelParticipants;
        set => SetProperty(ref _tempoDelParticipants, value);
    }

    //cette liste permet de passé la liste des participant dans la ParticipantCardViewModel
    private ObservableCollectionFast<ParticipantsCardViewModel> _participantCards;
    public ObservableCollectionFast<ParticipantsCardViewModel> ParticipantCards {
        get => _participantCards;
        set => SetProperty(ref _participantCards, value);
    }

    public ParticipantsViewModel(Tricount tricount, bool isNew) : base() {
        Tricount = tricount;
        IsNew = isNew;
        AddUserCommand = new RelayCommand(AddAction, () => !Non_Participant.IsNullOrEmpty());
        AddMySelfCommand = new RelayCommand(AddMySelfAction, CanAddMySelf);
        AddEveryBodyCommand = new RelayCommand(AddEveryBodyAction, () => !Non_Participant.IsNullOrEmpty());

        Participant = new ObservableCollectionFast<User>();
        Non_Participant = new ObservableCollectionFast<User>();
        TempoDelParticipants = new ObservableCollectionFast<User>();

        ListNon_Participant();
        ListParticipant();

        Register<User>(App.Messages.MSG_REVOME_PARTICIPANT, DelAction);

        OnRefreshData();
        // Notifier du changement de la liste des participants
        //RaisePropertyChanged(nameof(Participant));
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
            //Tricount.AddUserSubTricount(CurrentUser);
        } else {
            foreach (Subscription sub in Tricount.Subscriptions) {
                Participant.Add(sub.User);
            }
        }
        OnRefreshData();
        //RaisePropertyChanged(nameof(Participant));
    }

    private void AddAction() {
        if (UserSelected != null) {
            User user = UserSelected;
            Participant.Add(UserSelected);
            if (TempoDelParticipants.Contains(user)) {
                TempoDelParticipants.Remove(user);
            }
            Non_Participant.Remove(UserSelected);
            //if (IsNew) {
            //    Tricount.AddUserSubTricount(user);
            //}
        }
        OnRefreshData();
        // Notifier du changement de la liste des participants
        //RaisePropertyChanged(nameof(Participant));
    }

    private void AddMySelfAction() {
        Participant.Add(CurrentUser);
        Non_Participant.Remove(CurrentUser);
        OnRefreshData();
        // Notifier du changement de la liste des participants
        //RaisePropertyChanged(nameof(Participant));
    }

    private bool CanAddMySelf() {
        return !Participant.Contains(CurrentUser);
    }

    private void AddEveryBodyAction() {
        foreach (var p in Non_Participant) {
            Participant.Add(p);
            //if (IsNew) {
            //    Tricount.AddUserSubTricount(p);
            // }
        }
        Non_Participant.Clear();
        OnRefreshData();
        // Notifier du changement de la liste des participants
        //RaisePropertyChanged(nameof(Participant));
    }

    private void DelAction(User user) {
        if (IsNew) {
            if (!user.Equals(CurrentUser)) {
                //Tricount.RemoveUserSubTricount(user);
                Non_Participant.Add(user);
                Participant.Remove(user);
            }
        } else {
            if (user.UserId != Tricount.CreatorId) {
                Non_Participant.Add(user);
                Participant.Remove(user);
                //permet de stocker provisoirement les users remove de sub le temps de save ou cancel.
                TempoDelParticipants.Add(user);
                //Tricount.RemoveUserSubTricount(user);
            }
        }
        OnRefreshData();
        // Notifier du changement de la liste des participants
        //RaisePropertyChanged(nameof(Participant));
    }

    protected override void OnRefreshData() {
        ParticipantCards = new ObservableCollectionFast<ParticipantsCardViewModel>(Participant.Select(user => new ParticipantsCardViewModel(Tricount, IsNew, user)));
    }
}

