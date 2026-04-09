using MyNotesAppUnoPlatform.ViewsModels;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace MyNotesAppUnoPlatform;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class NoteDetailPage : Page
{
    public NoteDetailViewModel ViewModel;
    public NoteDetailPage()
    {
        ViewModel = App.HostContainer!.Services.GetRequiredService<NoteDetailViewModel>();
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        var itemId = (int)e.Parameter;
        if (itemId > 0)
            ViewModel.InitializeNoteDetailData(itemId);
    }
}
