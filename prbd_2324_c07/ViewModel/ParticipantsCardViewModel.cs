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

    private bool _delVisibility;
    public bool DelVisibility {
        get => _delVisibility;
        set => SetProperty(ref _delVisibility, value);
    }

    private int NbExpense => NbExpenseByUser();
    //si expense > 0 && le user est le créateur sinon on affiche que le NbExpense
    public string CreatorDisplay => 
        NbExpense == 0 && Participant.UserId != Tricount.CreatorId ? string.Empty :
        NbExpense > 0  && Participant.UserId == Tricount.CreatorId ? "(Creator) - ("+ NbExpense + " Expenses)" : "(" + NbExpense + " Expenses)";

    public ICommand DelUserCommand { get; set; }

    public ParticipantsCardViewModel(Tricount tricount, bool isNew, User user) {
        Tricount = tricount;
        IsNew = isNew;
        Participant = user;
        DelVisibility = user.UserId != tricount.CreatorId && NbExpense == 0;
        DelUserCommand = new RelayCommand(DelAction);
        NbExpenseByUser();
        //RaisePropertyChanged();
    }

    private void DelAction() {
        NotifyColleagues(App.Messages.MSG_REVOME_PARTICIPANT, Participant);
    }

    private int NbExpenseByUser() {
        var NbExpenseByUser = Context.Repartitions
            //pour chaque répartition ou le Participant est présent && opération du tricount on compte combien de fois il est présent.
                .Where(r => r.User.UserId == Participant.UserId && r.Operation.Tricount.TricountId == Tricount.TricountId)
                .GroupBy(r => r.Operation.OperationId)
                .Count();
        return NbExpenseByUser;
    }
}
