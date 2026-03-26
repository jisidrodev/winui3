using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MyNotesApp.Helpers;
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
    public sealed partial class MainPage : Page
    {
        public MainViewModel? ViewModel;
        public static MainPage? Current;
        public MainPage()
        {
            ViewModel = App.HostContainer?.Services.GetRequiredService<MainViewModel>();
            InitializeComponent();
            Current = this;
            Loaded += Page_Loaded;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var mainWindow = (Application.Current as App)?.Window as MainWindow;
            if (mainWindow != null)
                mainWindow.SetPageTitle("Home");
        }

        public void NotifyUser(string message, InfoBarSeverity severity, bool isOpen = true)
        {
            if (DispatcherQueue.HasThreadAccess)
            {
                UpdateStatus(message, severity, isOpen);
            }
            else
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    UpdateStatus(message, severity, isOpen);
                });
            }
        }

        private void UpdateStatus(string message, InfoBarSeverity severity, bool isOpen)
        {
            notifyInfoBar.Message = message;
            notifyInfoBar.IsOpen = isOpen;
            notifyInfoBar.Severity = severity;
        }

        public void NotificationReceived(NotificationShared.Notification notification)
        {
            var text = $"{notification.Originator}; Action: {notification.Action}";

            if (notification.HasInput)
            {
                if (string.IsNullOrWhiteSpace(notification.Input))
                    text += "; No input received";
                else
                    text += $"; Input received: {notification.Input}";
            }

            if (DispatcherQueue.HasThreadAccess)
                DisplayMessageDialog(text);
            else
            {
                DispatcherQueue.TryEnqueue(() =>
                {
                    DisplayMessageDialog(text);
                });
            }
        }

        private void DisplayMessageDialog(string message)
        {
            ContentDialog notifyDialog = new()
            {
                XamlRoot = this.XamlRoot,
                Title = "Notification received",
                Content = message,
                CloseButtonText = "Ok"
            };

            notifyDialog.ShowAsync();
        }
    }
}
