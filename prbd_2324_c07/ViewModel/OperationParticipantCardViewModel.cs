using prbd_2324_c07.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_c07.ViewModel
{
    public class OperationParticipantCardViewModel : ViewModelBase<User, PridContext>
    {

        //private Tricount _tricount;
        //public Tricount Tricount {
        //    get => _tricount;
        //    set => SetProperty(ref _tricount, value);
        //}

        private User _participant;
        public User Participant {
            get => _participant;
            set => SetProperty(ref _participant, value);
        }

        private double _amout;
        public double Double {
            get => _amout;
            set => SetProperty(ref _amout, value);
        }

        private double _repartition;
        public double repartition {
            get => _repartition;
            set => SetProperty(ref _repartition, value);
        }

        public OperationParticipantCardViewModel(User user) { 
            Participant = user;
        }
    
    }
}
