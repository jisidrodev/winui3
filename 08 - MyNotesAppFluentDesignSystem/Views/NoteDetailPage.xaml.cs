using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MyNotesApp.ViewsModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyNotesApp.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NoteDetailPage : Page
    {
        public NoteDetailViewModel? ViewModel;

        public NoteDetailPage()
        {
            ViewModel = App.HostContainer?.Services.GetRequiredService<NoteDetailViewModel>();
            InitializeComponent();
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

            // Load the user setting
            string? haveExplainedSaveSetting = localSettings.Values[nameof(SavingTip)] as string;

            // If the user has not seen the save tip, display it

            if (!bool.TryParse(haveExplainedSaveSetting, out bool result) || !result)
            {
                SavingTip.IsOpen = true;

                // Save the teaching tip setting
                localSettings.Values[nameof(SavingTip)] = "true";
            }

            Loaded += Page_Loaded;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var noteId = (int)e.Parameter;
            if (noteId > 0)
                ViewModel?.InitializeNoteDetailDataAsync(noteId);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var mainWindow = (Application.Current as App)?.Window as MainWindow;
            if (mainWindow != null)
                mainWindow.SetPageTitle("Note Detail");
        }

    }
}
