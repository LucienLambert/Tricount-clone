using prbd_2324_c07.Model;
using PRBD_Framework;

namespace prbd_2324_c07.ViewModel;

public class TricountDetailViewModel : ViewModelBase<User, PridContext> {

    private string _defaultHeader;

    public string DefaultHeader {
        get => $"<New Tricount> - No Description\nCreated by {CurrentUser.FullName} on {DateTime.Now.Date.ToString("dd/MM/yyyy")}";
    }

    private DateTime _date;

    public DateTime Date {
        get => _date;
        set => SetProperty(ref _date, value, () => Validate());
    }

    private string _title;

    public string Title {
        get => _title;
        set => SetProperty(ref _title, value, () => Validate());
    }

    private string _description;

    public string Description {
        get => _description;
        set => SetProperty(ref _description, value ,() => Validate());
    }

    private bool _conditionButtonSave;

    public bool ConditionButtonSave { 
        get => _conditionButtonSave;
        set => SetProperty(ref _conditionButtonSave, value);
    }

    public TricountDetailViewModel() {

    }

    public TricountDetailViewModel(Tricount tricount, bool isNew) { 
        Date = DateTime.Now;
    }

    public override bool Validate() {
        ClearErrors();

        var tricount = Context.Tricounts.FirstOrDefault(Tricount => Tricount.Title == Title);

        if (Title == null) {
            AddError(nameof(Title), "required");
        } else if (Title.Length < 3) {
            AddError(nameof(Title), "Min 3 characters");
        } else if (tricount != null) {
            AddError(nameof(Title), "this tricount's title is already in use");
        } 

        if(!string.IsNullOrEmpty(Description)) {
            if (Description.Length < 3) {
                    AddError(nameof(Description), "Min 3 characters");
            }
        }

        if (Date.CompareTo(DateTime.Now) > 0) {
            AddError(nameof(Date), "the date cannot be greater than the current date");
        }
        ConditionButtonSave = !HasErrors;
        return !HasErrors;
    }
}