using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel;

public class TricountsViewModel : ViewModelBase<User, PridContext>
{

    public ObservableCollection<Tricount> Tricounts { get; set; }


    public static string Title {
        get => $"My Tricount ({CurrentUser?.Mail})";
    }

    public TricountsViewModel() : base() {
        Tricounts = new ObservableCollection<Tricount>(Context.Tricounts);

    }

    protected override void OnRefreshData() {
        // pour plus tard
    }





}

