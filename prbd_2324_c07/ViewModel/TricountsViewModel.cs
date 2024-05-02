using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel;

public class TricountsViewModel : ViewModelBase<User, PridContext>
{

    public ObservableCollection<Tricount> Tricounts { get; set; }
    

    public TricountsViewModel() : base() {
        Tricounts = GetTricountsByUser(ViewModelCommon.CurrentUser);

    }

    public ObservableCollection<Tricount> GetTricountsByUser(User user) {
        if (ViewModelCommon.isAdmin){
            return new ObservableCollection<Tricount>(Context.Tricounts);
        }
        // Tricounts crées par l'utilisateur
        IQueryable<Tricount> createdTricounts = Context.Tricounts.Where(t => t.Creator == user);
        // Tricounts dont l'utilisateur est participant
        IQueryable<Tricount> participantTricounts = Context.Subscriptions
            .Where(sub => sub.User == user)
            .Select(sub => sub.Tricount);
        // Union des deux
        IQueryable<Tricount> allTricounts = createdTricounts.Union(participantTricounts);

        return new ObservableCollection<Tricount>(allTricounts);
    }
    

    protected override void OnRefreshData() {
        // pour plus tard
    }





}

