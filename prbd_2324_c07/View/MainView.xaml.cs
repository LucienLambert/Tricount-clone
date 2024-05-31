using prbd_2324_c07.Model;
using PRBD_Framework;
using System.ComponentModel;
using System.Windows.Controls;

namespace prbd_2324_c07.View;

public partial class MainView : WindowBase {
    
    public MainView() {
        InitializeComponent();
        Register<Tricount>(App.Messages.MSG_NEW_TRICOUNT, tricount => DoDisplayTricount(tricount, true));
        Register<Tricount>(App.Messages.MSG_CLOSE_TAB, DoCloseTab);
        Register<Tricount>(App.Messages.MSG_DISPLAY_TRICOUNT, tricount => DoDisplayTricount(tricount, false));
        Register<Tricount>(App.Messages.MSG_TRICOUNT_CHANGED,
                tricount => DoRenameTab(string.IsNullOrEmpty(tricount.Title) ? "<New Tricount>" : tricount.Title));
    }

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

    private void DoCloseTab(Tricount tricount) {
        tabControl.CloseByTag(string.IsNullOrEmpty(tricount.Title) ? "<New Tricount>" : tricount.Title);
    }

    protected override void OnClosing(CancelEventArgs e) {
        base.OnClosing(e);
        tabControl.Dispose();
    }

    private void DoRenameTab(string header) {
        if (tabControl.SelectedItem is TabItem tab) {
            MyTabControl.RenameTab(tab, header);
            tab.Tag = header;
        }
    }
}

