using Azure;
using prbd_2324_c07.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_c07.ViewModel
{
    public class TricountGraphViewModel : ViewModelBase<User, PridContext>
    {
        private Tricount _tricount;
        public Tricount Tricount {
            get => _tricount;
            set => SetProperty(ref _tricount, value);
        }

        private ObservableCollection<TricountGraphBarViewModel> _tricountGraphBars;

        public ObservableCollection<TricountGraphBarViewModel> TricountGraphBars {
            get => _tricountGraphBars;
            set => SetProperty(ref _tricountGraphBars, value);
        }


        public TricountGraphViewModel(Tricount tricount) {
            Tricount = tricount;
            OnRefreshData();
        }


        protected override void OnRefreshData() {

            var participants = Tricount.ParticipantTricount()
                 .Select(par => par.User)
                 .Distinct();

            TricountGraphBars = new ObservableCollection<TricountGraphBarViewModel>(participants.Select(user =>
                new TricountGraphBarViewModel(user, Tricount.GetUserBalance(user), user == Tricount.Creator ? true : false)
            ));

        }
    }
}
