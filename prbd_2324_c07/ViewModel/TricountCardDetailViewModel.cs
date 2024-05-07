using prbd_2324_c07.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace prbd_2324_c07.ViewModel
{
    public class TricountCardDetailViewModel : ViewModelCommon
    {

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


        public ICommand EditTricount { get; set; }

        public ICommand DeleteTricount { get; set; }


        public TricountCardDetailViewModel(Tricount tricount, bool isNew) {
            Tricount = tricount;
            IsNew = IsNew;


        }



    }
}
