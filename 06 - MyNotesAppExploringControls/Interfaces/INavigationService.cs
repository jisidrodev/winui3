using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotesApp.Interfaces
{
    public interface INavigationService
    {
        string CurrentPage { get; }
        void NavigateToPage(string page);
        void NavigateToPage(string page, object? parameter);
        void GoBack();
    }
}
