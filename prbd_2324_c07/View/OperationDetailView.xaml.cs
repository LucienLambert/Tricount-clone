using prbd_2324_c07.Model;
using prbd_2324_c07.ViewModel;
using PRBD_Framework;

namespace prbd_2324_c07.View
{
    public partial class OperationDetailView : WindowBase
    {

        public readonly OperationDetailViewModel _vm;

        public OperationDetailView(Tricount tricount)
        {
            InitializeComponent();
            DataContext = _vm = new OperationDetailViewModel(tricount);
        }
    }
}
