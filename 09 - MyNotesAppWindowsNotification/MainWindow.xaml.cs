using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MyNotesApp.Enums;
using MyNotesApp.Model;
using MyNotesApp.ViewsModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using WinRT.Interop;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MyNotesApp
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class MainWindow : Window
    {
        private AppWindow? _appWindow;
        private const string APPTITLE = "My Notes App";

        public MainWindow()
        {
            InitializeComponent();
            _appWindow = GetCurrentAppWindow();
            if (_appWindow == null) throw new Exception("AppWindows not exist.");
            _appWindow.Title = APPTITLE;
            ExtendsContentIntoTitleBar = true;
        }

        private AppWindow? GetCurrentAppWindow()
        { 
            IntPtr handle = WindowNative.GetWindowHandle(this);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(handle);
            return AppWindow.GetFromWindowId(windowId);
        }

        internal void SetPageTitle(string title)
        {
            if(_appWindow == null)
            {
                _appWindow = this.GetCurrentAppWindow();

            }
            if(_appWindow != null)
                _appWindow.Title = $"{APPTITLE} - {title}";

        }
    }


}
