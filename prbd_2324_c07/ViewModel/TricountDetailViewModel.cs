using prbd_2324_c07.Model;
using PRBD_Framework;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel;

public class TricountDetailViewModel : ViewModelBase<User, PridContext>
{

    private string _defaultHeader;

    public string DefaultHeader {
        get => _defaultHeader;
        set => SetProperty(ref _defaultHeader, value);
    }

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

    public string Title {
        get => Tricount?.Title;
        set => SetProperty(Tricount.Title, value, Tricount, (t, v) => {
            t.Title = v;
            Validate();
            NotifyColleagues(App.Messages.MSG_TRICOUNT_CHANGED, Tricount);
        });
    }

    public string Description {
        get => Tricount?.Description;
        set => SetProperty(Tricount?.Description, value, Tricount, (t, d) => {
            t.Description = d;
            Validate();
        });
    }

    public DateTime CreatedAt {
        get => (DateTime)Tricount?.CreatedAt;
        set => SetProperty(Tricount?.CreatedAt, value, Tricount, (t, c) => {
            t.CreatedAt = (DateTime)c;
            Validate();
        });
    }

    public ICommand BtnCancel { get; set; }

    public TricountDetailViewModel() {

    }

    public TricountDetailViewModel(Tricount tricount, bool isNew) {
        Tricount = tricount;
        IsNew = isNew;
        CreatedAt = DateTime.Now;
        HeaderDefaultSet();
        BtnCancel = new RelayCommand(CancelAction, CanCancelAction);
    }

    private void HeaderDefaultSet() {
        if (IsNew) {
            DefaultHeader = $"<New Tricount> - No Description\nCreated by {CurrentUser.FullName} on {DateTime.Now.Date.ToString("dd/MM/yyyy")}";
        } else {
            DefaultHeader = $"{Tricount.Title} - {Tricount.Description}\nCreated by {Tricount.Creator} on {Tricount.CreatedAt.ToString("dd/MM/yyyy")}";
        }
    }

    public override bool Validate() {
        ClearErrors();

        Tricount.Validate();

        AddErrors(Tricount.Errors);

        return !HasErrors;
    }

    public override void CancelAction() {
        ClearErrors();
        if (IsNew) {
            IsNew = false;
            NotifyColleagues(App.Messages.MSG_CLOSE_TAB, Tricount);
        } else {
            Tricount.Reload();
            RaisePropertyChanged();
        }
    }

    private bool CanCancelAction() {
        return Tricount != null && (IsNew || Tricount.IsModified);
    }
}