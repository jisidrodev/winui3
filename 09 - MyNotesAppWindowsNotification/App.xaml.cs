using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.Windows.AppLifecycle;
using Microsoft.Windows.AppNotifications;
using MyNotesApp.Helpers;
using MyNotesApp.Interfaces;
using MyNotesApp.Services;
using MyNotesApp.Views;
using MyNotesApp.ViewsModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyNotesApp
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        private static Window? _window;

        public static IHost? HostContainer { get; private set; } = null;
        internal Window? Window => _window;

        [DllImport("user32.dll", SetLastError = true)]
        static extern void SwitchToThisWindow(IntPtr hWnd, bool turnOn);

        private NotificationManager notificationManager;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        /// 
        public App()
        {
            InitializeComponent();
            notificationManager = new NotificationManager();
            notificationManager.Init();
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;
        }

        /// <summary>
        /// Invoked when the application is launched.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected async override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            var rootFrame = new Frame();
            await this.RegisterComponentsAsync(rootFrame);
            rootFrame.NavigationFailed += RootFrame_NavigationFailed;
            rootFrame.Navigate(typeof(MainPage), args,null);
            _window.Content = rootFrame;

            var currentInstance = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent();
            if (currentInstance.IsCurrent)
            {
                AppActivationArguments activationArgs = currentInstance.GetActivatedEventArgs();
                if (activationArgs != null)
                {
                    ExtendedActivationKind extendedKind = activationArgs.Kind;
                    if (extendedKind == ExtendedActivationKind.AppNotification)
                    {
                        var notificationActivatedEventArgs = (AppNotificationActivatedEventArgs)activationArgs.Data;
                        notificationManager.ProcessLaunchActivationArgs(notificationActivatedEventArgs);
                    }
                }
            }

            _window.Activate();
        }

        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception($"Error loading page {e.SourcePageType.FullName}");
        }

        /// <summary>
        /// Initializes the DI container
        /// </summary>
        /// 
        private async Task RegisterComponentsAsync(Frame frame)
        {
            var navigationService = new NavigationService(frame);
            navigationService.Configure(nameof(MainPage), typeof(MainPage));
            navigationService.Configure(nameof(NoteDetailPage), typeof(NoteDetailPage));
            var dataService = new SqliteDataService();
            await dataService.InitializeDataAsync();
            HostContainer = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<INavigationService>(navigationService);
                    services.AddSingleton<IDataService>(dataService);
                    services.AddTransient<MainViewModel>();
                    services.AddTransient<NoteDetailViewModel>();
                }).Build();
        }

        private void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            notificationManager.Unregister();
        }

        public static void ToForeground()
        {
            if (_window != null)
            {
                IntPtr handle = WindowNative.GetWindowHandle(_window);
                if (handle != IntPtr.Zero)
                {
                    SwitchToThisWindow(handle, true);
                }
            }
        }

        public static string GetFullPathToExe()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var pos = path.LastIndexOf("\\");
            return path.Substring(0, pos);
        }

        public static string GetFullPathToAsset(string assetName)
        {
            return $"{GetFullPathToExe()}\\Assets\\{assetName}";
        }
    }
}
