using System;
using PRBD_Framework;
using System.Windows.Controls;
using prbd_2324_c07.Model;
using prbd_2324_c07.ViewModel;

namespace prbd_2324_c07.View
{
    public partial class TricountCardDetailView : UserControlBase
    {
        private readonly TricountCardDetailViewModel _vm;

        public TricountCardDetailView(Tricount tricount, bool isNew) {
            InitializeComponent();

            // Nécessaire pour pouvoir fermer l'onglet
            DataContext = _vm = new TricountCardDetailViewModel(tricount, isNew);



            InitializeComponent();

            Register<Tricount>(App.Messages.MSG_NEW_OPERATION, tricount => {
                OperationDetailView OperationDetailWindow = new OperationDetailView(tricount);
                OperationDetailWindow.ShowDialog();
            });


        }
    }
}
