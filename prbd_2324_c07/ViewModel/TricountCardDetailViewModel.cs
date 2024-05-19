using prbd_2324_c07.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel
{
    public class TricountCardDetailViewModel : ViewModelBase<User, PridContext>
    {

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


        public TricountCardDetailViewModel() {
            
        }
        public TricountCardDetailViewModel(Tricount tricount, bool isNew) {
            Tricount = tricount;
            IsNew = IsNew;
            OperationsVM = new OperationsViewModel(tricount);
            TricountGraphVM = new TricountGraphViewModel(tricount);
        }



    }
}
