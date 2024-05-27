using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel;

public class ParticipantsCardViewModel : ViewModelBase<User, PridContext> {

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

    private User _participant;
    public User Participant {
        get => _participant;
        set => SetProperty(ref _participant, value);
    }

    private bool _isCreator;
    public bool IsCreator {
        get => _isCreator;
        set => SetProperty(ref _isCreator, value);
    }

    public ICommand DelUserCommand { get; set; }

    public ParticipantsCardViewModel(Tricount tricount, bool isNew, User user) {
        Tricount = tricount;
        IsNew = isNew;
        Participant = user;
        IsCreator = user.UserId != tricount.CreatorId;
        DelUserCommand = new RelayCommand(DelAction);
        RaisePropertyChanged();
    }

    private void DelAction() {
        NotifyColleagues(App.Messages.MSG_REVOME_PARTICIPANT, Participant);
    }
}
