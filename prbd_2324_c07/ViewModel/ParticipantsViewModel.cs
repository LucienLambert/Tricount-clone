using Microsoft.IdentityModel.Tokens;
using prbd_2324_c07.Model;
using PRBD_Framework;

namespace prbd_2324_c07.ViewModel;

class ParticipantsViewModel : ViewModelBase<User, PridContext> {

    private ObservableCollectionFast<string> _listUsersFullName;

    public ObservableCollectionFast<string> ListUserFullName {
        get { return _listUsersFullName; }
        private set => SetProperty(ref _listUsersFullName, value);
    }

    private ObservableCollectionFast<string> _participantsUsers;

    public ObservableCollectionFast<string> ParticipantsUsers {
        get { return _participantsUsers; }
        private set => SetProperty(ref _participantsUsers, value);
    }

    private bool _conditionButtonAdd;

    public bool ConditionButtonAdd { 
        get  => _conditionButtonAdd;
        set => SetProperty(ref _conditionButtonAdd, value);
    }

    public ParticipantsViewModel() {
        ListAllUsers();
    }

    private void ListAllUsers() {
        //créer une liste de tout les users sauf CurrentUser.
        ListUserFullName = new ObservableCollectionFast<string>(Context.Users.Where(u => CurrentUser.FullName != u.FullName).Select(u => u.FullName));
        if (_listUsersFullName.IsNullOrEmpty()) {
            ConditionButtonAdd = false;
        } else {
            ConditionButtonAdd = true;
        }
    }

}

