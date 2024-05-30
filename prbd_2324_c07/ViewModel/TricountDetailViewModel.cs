using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using prbd_2324_c07.Model;
using prbd_2324_c07.View;
using PRBD_Framework;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel;

public class TricountDetailViewModel : ViewModelBase<User, PridContext> {

    // --------- Gestion TricountView ---------

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

    private DateTime _DatePicker;
    public DateTime DatePicker {
        get => _DatePicker;
        set => SetProperty(ref _DatePicker, value);
    }

    //ajout
    private ParticipantsViewModel _participantVM;
    public ParticipantsViewModel ParticipantVM {
        get => _participantVM;
        set => SetProperty(ref _participantVM, value);
    }

    public ICommand BtnCancel { get; set; }
    public ICommand BtnSave { get; set; }

    private ObservableCollectionFast<User> listParticipant;

    string titleTemp;

    public TricountDetailViewModel(Tricount tricount, bool isNew) {
        ParticipantVM = new ParticipantsViewModel(tricount, isNew);
        Tricount = tricount;
        IsNew = isNew;
        if(IsNew) {
            titleTemp = "<New Tricount>";
        } else {
            titleTemp = tricount.Title;
        }
        InitializeDataView();

        RaisePropertyChanged();
    }

    private void InitializeDataView() {
        BtnCancel = new RelayCommand(CancelAction);
        BtnSave = new RelayCommand(SaveAction, CanSaveAction);

        if(IsNew) {
            CreatedAt = DateTime.Now;
        }
        DatePicker = DateTime.Now;
        listParticipant = new ObservableCollectionFast<User>();

        HeaderDefaultSet();
    }

    private void HeaderDefaultSet() {       
        if (IsNew) {
            DefaultHeader = $"<New Tricount> - No Description\nCreated by {CurrentUser.FullName} on {DateTime.Now.Date.ToString("dd/MM/yyyy")}";
        } else {
            DefaultHeader = $"{Tricount.Title} - {Tricount.Description}\nCreated by {Tricount.Creator.FullName} on {Tricount.CreatedAt.ToString("dd/MM/yyyy")}";
        }
    }

    public override bool Validate() {
        ClearErrors();
        Tricount.Validate(CurrentUser); 
        AddErrors(Tricount.Errors);
        return !HasErrors;
    }

    public override void CancelAction() {
        ClearErrors();
        if (IsNew) {
            Tricount.Title = null;
            IsNew = false;
        } else {
            Tricount.Reload();
            //notife qui changement à été fait et déclanche une MAJ de l'interface.
            RaisePropertyChanged();
        }
        listParticipant.Clear();
        NotifyColleagues(App.Messages.MSG_CLOSE_TAB, titleTemp);
    }

    public override void SaveAction() {
        if (IsNew) {
            //add idUser -> creatorID
            Tricount.CreatorId = CurrentUser.UserId;
            //add Tricount à la DB
            Context.Add(Tricount);
            //add les sub à la DB
            foreach (var sub in Tricount.Subscriptions) {
                if (!Context.Subscriptions.Any(s => s.UserId == sub.UserId && s.TricountId == sub.TricountId)) {
                    Context.Add(sub);
                }
            }
            //Context.AddRange(Tricount.Subscriptions);
            IsNew = false;
        } else {
            //parcours la liste de Participant
            foreach (var p in ParticipantVM.Participant) {
                //check si l'utilisateur est déjà présent dans la liste des Subscriptions sinon add user dans subscription
                if (!Tricount.Subscriptions.Any(sub => sub.UserId == p.UserId)) {
                    Console.WriteLine("add : "+p.FullName+" into sub");
                    Tricount.AddUserSubTricount(p);
                }
            }
            //parcours la liste de participant à remove de Sub
            foreach (var p in ParticipantVM.TempoDelParticipants){
                //check si l'utilisateur est présent dans la liste des Subscriptions si oui remove user dans subscription
                if (Tricount.Subscriptions.Any(sub => sub.UserId == p.UserId)) {
                    Tricount.RemoveUserSubTricount(p);
                }
            }
        }

        Context.SaveChanges();
        RaisePropertyChanged(nameof(Title));
        listParticipant.Clear();
        
        NotifyColleagues(App.Messages.MSG_CLOSE_TAB, titleTemp);
        NotifyColleagues(App.Messages.MSG_TRICOUNT_CHANGED, Tricount);
        NotifyColleagues(App.Messages.MSG_DISPLAY_TRICOUNT, Tricount);
        

    }

    private bool CanSaveAction() {
        if (IsNew) {
            return Tricount.Validate() && !HasErrors;
        }
        return Tricount != null && !HasErrors && Tricount.IsModified;
    }

    protected override void OnRefreshData() {
       
    }
}