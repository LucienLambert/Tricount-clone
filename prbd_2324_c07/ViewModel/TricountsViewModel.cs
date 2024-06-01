using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel;

public class TricountsViewModel : ViewModelCommon {

    private ObservableCollection<TricountCardViewModel> _tricounts;
    public ObservableCollection<TricountCardViewModel> Tricounts {
        get => _tricounts;
        set => SetProperty(ref _tricounts, value);
    }

    private string filter;
    public string Filter { get => filter; set => SetProperty(ref filter, value, OnRefreshData); }

    public ICommand ClearFilter { get; set; }
    public ICommand DisplayTricountDetails { get; set; }
    public ICommand NewTricount { get; set; }

    public TricountsViewModel() {
        Console.WriteLine("TricountViewModel");

        OnRefreshData();

        ClearFilter = new RelayCommand(() => Filter = "");

        NewTricount = new RelayCommand(() => {
            NotifyColleagues(App.Messages.MSG_NEW_TRICOUNT, new Tricount());
        });

        DisplayTricountDetails = new RelayCommand<TricountCardViewModel>(vm => {
            NotifyColleagues(App.Messages.MSG_DISPLAY_TRICOUNT, vm.Tricount);
        });

        Register<Tricount>(App.Messages.MSG_TRICOUNT_CHANGED, tricount => {
            OnRefreshData();
        });
        Register(App.Messages.MSG_OPERATION_CHANGED, OnRefreshData);
        Register(App.Messages.MSG_RELOAD_ASKED, OnRefreshData);
    }

    protected override void OnRefreshData() {

        IQueryable<Tricount> tricounts = string.IsNullOrEmpty(Filter) ? Tricount.GetAllWithUser(CurrentUser) : Tricount.GetFiltered(Filter, CurrentUser);
        Console.WriteLine(tricounts.ToString());
        Tricounts = new ObservableCollection<TricountCardViewModel>(tricounts.Select(tricount => new TricountCardViewModel(tricount)));
    }





}

