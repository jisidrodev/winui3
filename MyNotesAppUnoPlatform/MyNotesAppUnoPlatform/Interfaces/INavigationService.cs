namespace MyNotesAppUnoPlatform.Interfaces
{
    public interface INavigationService
    {
        string CurrentPage { get; }
        void NavigateToPage(string page);
        void NavigateToPage(string page, object? parameter);
        void GoBack();
    }
}
