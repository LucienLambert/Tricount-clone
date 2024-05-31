using prbd_2324_c07.Model;
using PRBD_Framework;
using System.ComponentModel;

namespace prbd_2324_c07.View;

public partial class MainView : WindowBase {
    
    public MainView() {
        InitializeComponent();
        Register<Tricount>(App.Messages.MSG_NEW_TRICOUNT, tricount => DoDisplayTricount(tricount, true));
        Register<string>(App.Messages.MSG_CLOSE_TAB, DoCloseTab);
        Register<Tricount>(App.Messages.MSG_DISPLAY_TRICOUNT, tricount => DoDisplayTricount(tricount, false));}

    private void DoDisplayTricount(Tricount tricount, bool isNew) {
        if (tricount != null) {
            OpenTab(isNew ? "<New Tricount>" : tricount.Title, tricount.Title, () => 
            isNew ? new TricountDetailView(tricount, isNew) : new TricountCardDetailView(tricount, isNew));
        }
    }

    private void OpenTab(string header, string tag, Func<UserControlBase> createView) {
        var tab = tabControl.FindByTag(tag);
        if (tab == null)
            tabControl.Add(createView(), header, tag);
        else
            tabControl.SetFocus(tab);
    }

    private void DoCloseTab(string title) {
        Console.WriteLine(title);
        tabControl.CloseByTag(title);
    }

    protected override void OnClosing(CancelEventArgs e) {
        base.OnClosing(e);
        tabControl.Dispose();
    }
}

