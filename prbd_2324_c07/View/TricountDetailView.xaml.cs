using prbd_2324_c07.Model;
using prbd_2324_c07.ViewModel;
using PRBD_Framework;

namespace prbd_2324_c07.View;

public partial class TricountDetailView : UserControlBase{

    public readonly TricountDetailViewModel _vm;
    
    public TricountDetailView() {
        InitializeComponent();
    }

    public TricountDetailView(Tricount tricount, bool isNew){
        InitializeComponent();
        DataContext = _vm = new TricountDetailViewModel(tricount, isNew);
    }

    private void ParticipantsView_Loaded(object sender, System.Windows.RoutedEventArgs e) {

    }
}
