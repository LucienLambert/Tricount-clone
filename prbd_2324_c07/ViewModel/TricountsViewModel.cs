using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel;

public class TricountsViewModel : ViewModelBase<User, PridContext> {

    private ObservableCollection<TricountCardViewModel> _tricounts;


    public ObservableCollection<TricountCardViewModel> Tricounts {
        get => _tricounts;
        set => SetProperty(ref _tricounts, value);
    }

    public ICommand NewTricount { get; set; }

 
    public TricountsViewModel() : base() {

        OnRefreshData();

        NewTricount = new RelayCommand(() => {
            NotifyColleagues(App.Messages.MSG_NEW_TRICOUNT, new Tricount());
        });
    }

    protected override void OnRefreshData() {
        var tricounts = Tricount.GetAllWithUser(CurrentUser);
        Tricounts = new ObservableCollection<TricountCardViewModel>(tricounts.Select(tricount => new TricountCardViewModel(tricount)));
    }





}

