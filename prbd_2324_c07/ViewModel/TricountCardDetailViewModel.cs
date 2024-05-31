using prbd_2324_c07.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel
{
    public class TricountCardDetailViewModel : ViewModelBase<User, PridContext> {

        private OperationsViewModel _operationsVM;
        public OperationsViewModel OperationsVM {
            get => _operationsVM;
            set => SetProperty(ref _operationsVM, value);
        }

        private TricountGraphViewModel _tricountGraphVM;
        public TricountGraphViewModel TricountGraphVM {
            get => _tricountGraphVM;
            set => SetProperty(ref _tricountGraphVM, value);
        }

        private TricountDetailViewModel _tricountDetailVM;
        public TricountDetailViewModel TricountDetailVM {
            get => _tricountDetailVM;
            set => SetProperty(ref _tricountDetailVM, value);
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

        public string TricountTitle => Tricount.Title;

        public string TricountDescription => Tricount.Description;

        public string TricountCreator => Tricount.Creator.FullName;

        public DateTime CreatedAt => Tricount.CreatedAt;

        public ICommand EditTricount { get; set; }

        public ICommand DeleteTricount { get; set; }

        private bool _visibleTricountCard;
        public bool VisibleTricountCard {
            get => _visibleTricountCard;
            set => SetProperty(ref _visibleTricountCard, value);
        }

        private bool _visibleTricountDetail;
        public bool VisibleTricountDetal {
            get => _visibleTricountDetail;
            set => SetProperty(ref _visibleTricountDetail, value);
        }

        public TricountCardDetailViewModel() {

        }
        public TricountCardDetailViewModel(Tricount tricount, bool isNew) {
            Tricount = tricount;
            IsNew = IsNew;
            OperationsVM = new OperationsViewModel(tricount);
            TricountGraphVM = new TricountGraphViewModel(tricount);
            VisibleTricountCard = true;
            VisibleTricountDetal = false;
            EditTricount = new RelayCommand(EditAction, CanEditAction);
            DeleteTricount = new RelayCommand(DeleteTricountAction, CanDeleteAction);
        }

        private void EditAction() {
            VisibleTricountCard = false;
            VisibleTricountDetal = true;
            TricountDetailVM = new TricountDetailViewModel(Tricount, false);
        }

        //Permet d'activer le bouton EditAction si CurrentUser = Creator ou Admin
        private bool CanEditAction() {
            return CurrentUser.Equals(Tricount.Creator) || ViewModelCommon.isAdmin;
        }


        private void DeleteTricountAction() {

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("You're about to delete this Tricount. \nDo you confirm ?", "Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes) {
                Tricount.Delete();
                NotifyColleagues(App.Messages.MSG_TRICOUNT_CHANGED, Tricount);
                NotifyColleagues(App.Messages.MSG_CLOSE_TAB, Tricount.Title);
            }
        }

        private bool CanDeleteAction() {
            return CurrentUser.Equals(Tricount.Creator) || ViewModelCommon.isAdmin;
        }


    }
}
