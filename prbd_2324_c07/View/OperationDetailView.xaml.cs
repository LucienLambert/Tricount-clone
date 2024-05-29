using prbd_2324_c07.Model;
using prbd_2324_c07.ViewModel;
using PRBD_Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
