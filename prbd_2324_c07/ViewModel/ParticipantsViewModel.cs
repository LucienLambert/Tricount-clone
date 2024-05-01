using Microsoft.IdentityModel.Tokens;
using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;

namespace prbd_2324_c07.ViewModel;

class ParticipantsViewModel : ViewModelBase<User, PridContext> {

    private ObservableCollection<string> _usersFullName;
    public ObservableCollection<string> UserFullName {
        get { return _usersFullName; }
        private set => SetProperty(ref _usersFullName, value);
    }

    public ParticipantsViewModel() {
        _usersFullName = new ObservableCollection<string>(Context.Users.Select(u => u.FullName));
}

}

