using Microsoft.EntityFrameworkCore;
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

    private string filter;

    public string Filter { get => filter; set => SetProperty(ref filter, value, OnRefreshData); }
    public ICommand ClearFilter { get; set; }

    public TricountsViewModel() : base() {

        OnRefreshData();

        ClearFilter = new RelayCommand(() => Filter = "");

        NewTricount = new RelayCommand(() => {
            NotifyColleagues(App.Messages.MSG_NEW_TRICOUNT, new Tricount());
        });
    }

    protected override void OnRefreshData() {

        IQueryable<Tricount> tricounts = string.IsNullOrEmpty(Filter) ? Tricount.GetAllWithUser(CurrentUser) : Tricount.GetFiltered(Filter);

        Tricounts = new ObservableCollection<TricountCardViewModel>(tricounts.Select(tricount => new TricountCardViewModel(tricount)));
    }





}

