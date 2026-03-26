using Microsoft.UI.Xaml.Controls;
using MyNotesApp.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotesApp.Services
{
    public class NavigationService : INavigationService
    {
        private Frame? _appFrame = null;
        private readonly IDictionary<string, Type> _pages = new ConcurrentDictionary<string, Type>();

        public const string RootPage = "(Root)";
        public const string UnknownPage = "(Unknown)";
        public NavigationService(Frame rootFrame)
        {
            _appFrame = rootFrame;
        }

        public void Configure(string page, Type type)
        {
            if (_pages.Values.Any(v => v == type))
            {
                throw new ArgumentException($"The {type.Name} view has already been registered under another name.");
            }

            _pages[page] = type;
        }

        public string CurrentPage
        {
            get
            {
                var frame = _appFrame;
                if (frame == null) throw new ArgumentException($"Please, to specify frame");

                if (frame.BackStackDepth == 0)
                    return RootPage;

                if (frame.Content == null)
                    return UnknownPage;

                var type = frame.Content.GetType();

                if (_pages.Values.All(v => v != type))
                    return UnknownPage;

                var item = _pages.Single(i => i.Value == type);

                return item.Key;
            }
        }

        public void GoBack()
        {
            if (_appFrame?.CanGoBack == true)
            {
                _appFrame.GoBack();
            }
        }

        public void NavigateToPage(string page)
        {
            this.NavigateToPage(page, null);
        }

        public void NavigateToPage(string page, object? parameter)
        {
            if (!_pages.ContainsKey(page))
            {
                throw new ArgumentException($"Unable to find a page registered with the name {page}.");
            }

            _appFrame?.Navigate(_pages[page], parameter);
        }
    }
}
