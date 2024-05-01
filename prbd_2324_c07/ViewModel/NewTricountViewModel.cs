using prbd_2324_c07.Model;
using PRBD_Framework;

namespace prbd_2324_c07.ViewModel;

public class NewTricountViewModel : ViewModelBase<User, PridContext> {

    private string _title;

    public string Title {
        get => _title;
        set => SetProperty(ref _title, value, () => validation());
    }

    private string _description;

    public string Description {
        get => _description;
        set => SetProperty(ref _description, value, () => validation());
    }

    public NewTricountViewModel() { 
        
    }

    private bool validation() {
        ClearErrors();

        var tricount = Context.Tricounts.FirstOrDefault(Tricount => Tricount.Title == Title);

        if (Title == null) {
            AddError(nameof(Title), "required");
        } else if (Title.Length < 3) {
            AddError(nameof(Title), "Min 3 characters");
        } else if (tricount != null) {
            AddError(nameof(Title), "this tricount's title is already in use");
        } else {
            if (Description != null && Description.Length < 3) {
                AddError(nameof(Title), "Min 3 characters or null");
            }
        }

        return !HasErrors;
    }

}