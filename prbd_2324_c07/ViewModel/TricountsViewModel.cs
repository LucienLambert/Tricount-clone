using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel;

public class TricountsViewModel : ViewModelBase<User, PridContext> {

    public ICommand NewTricount { get; set; }

    public ObservableCollection<Tricount> Tricounts { get; set; }
    
    public TricountsViewModel() : base() {
        Tricounts = GetTricountsByUser(ViewModelCommon.CurrentUser);
        NewTricount = new RelayCommand(() => {
            NotifyColleagues(App.Messages.MSG_NEW_TRICOUNT, new Tricount());
        });
    }

    private ObservableCollection<Tricount> GetTricountsByUser(User user) {
        if (ViewModelCommon.isAdmin){
            return new ObservableCollection<Tricount>(Context.Tricounts);
        }
        // Tricounts crées par l'utilisateur
        IQueryable<Tricount> createdTricounts = Context.Tricounts.Where(t => t.Creator == user);
        // Tricounts dont l'utilisateur est participant
        IQueryable<Tricount> participantTricounts = Context.Subscriptions
            .Where(sub => sub.User == user)
            .Select(sub => sub.Tricount);
        // Union des deux et tri
        IQueryable<Tricount> allTricounts = createdTricounts.Union(participantTricounts);
        var sortedTricounts = SortTricountsByDate(new ObservableCollection<Tricount>(allTricounts));
        
        return sortedTricounts;
    }

    private ObservableCollection<Tricount> SortTricountsByDate(ObservableCollection<Tricount> tricounts) {
        
        // TODO: ne fonctionne pas totalement, à modifier
        var sortedTricounts = tricounts
            .Select(t => new 
            {
                Tricount = t,
                MostRecentOperationDate = t.Operations.Max(o => (DateTime?)o.Operation_date)
            })
            .OrderByDescending(t => t.MostRecentOperationDate ?? t.Tricount.CreatedAt)
            .Select(t => t.Tricount);

        return new ObservableCollection<Tricount>(sortedTricounts);
    }


    protected override void OnRefreshData() {
        // pour plus tard
    }





}

