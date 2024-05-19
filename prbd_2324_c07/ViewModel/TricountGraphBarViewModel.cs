using Newtonsoft.Json.Linq;
using prbd_2324_c07.Model;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prbd_2324_c07.ViewModel
{
    public class TricountGraphBarViewModel : ViewModelBase<User, PridContext>
    {
        public User Participant;

        private double _amount;

        public double Amount { 
            get => _amount;
            set => SetProperty(ref _amount, value);
        }
        public string UserName => IsCreator ? Participant.FullName + " (me)" : Participant.FullName;

        public bool IsCreator;

        public TricountGraphBarViewModel(User user, double amount, bool isCreator) {
            Participant = user;
            Amount = amount;
            IsCreator = isCreator;
        }
    }
}
