using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Input;
using MyNotesApp.Enums;
using MyNotesApp.Helpers;
using MyNotesApp.Interfaces;
using MyNotesApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MyNotesApp.ViewsModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _selectedNote = String.Empty;

        [ObservableProperty]
        private ObservableCollection<Note> _items = new ObservableCollection<Note>();

        private ObservableCollection<Note>? _allNotes = null;

        [ObservableProperty]
        private IList<string> _notesTypes = new ObservableCollection<string>();

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
        private Note? _selectedNoteItem = null;

        private int _additionalItemCount = 1;

        private const string ALLNOTES = "All";
        private bool _canDeleteItem => _selectedNoteItem != null;

        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        public MainViewModel(INavigationService navigationService, IDataService dataService)
        {
            this._navigationService = navigationService;
            this._dataService = dataService;
            PopulateDataAsync();

        }

        public async Task PopulateDataAsync()
        {
            Items.Clear();
            foreach (var item in await _dataService.GetAllNotesAsync()) 
            {
                Items.Add(item);
            }

            _allNotes = new ObservableCollection<Note>(Items);
            NotesTypes = new ObservableCollection<string>()
            {
                ALLNOTES,
            };

            foreach (var enumNoteType in _dataService.GetEnumNoteTypes()) 
            {
                NotesTypes.Add(enumNoteType.ToString());   
            }

            SelectedNote = NotesTypes[0];
        }


        partial void OnSelectedNoteChanged(string value)
        {
            Items?.Clear();
            if (_allNotes != null)
            {
                foreach (var item in _allNotes)
                {
                    if (string.IsNullOrWhiteSpace(_selectedNote) ||
                        _selectedNote == "All" ||
                        _selectedNote == item.EnumNoteType.ToString())
                    {
                        Items?.Add(item);
                    }
                }
            }
        }

        [RelayCommand]
        public void AddOrEdit()
        {
            var selectedNoteItemId = -1;
            if(SelectedNoteItem != null)
                selectedNoteItemId = SelectedNoteItem.Id;
            _navigationService.NavigateToPage("NoteDetailPage", selectedNoteItemId);
        }

        [RelayCommand(CanExecute = nameof(_canDeleteItem))]
        public async Task DeleteAsync()
        {
            if (SelectedNoteItem != null)
            {
                await _dataService.DeleteNoteAsync(SelectedNoteItem.Id);
                Items?.Remove(SelectedNoteItem);
                _allNotes?.Remove(SelectedNoteItem);
            }
        }

        public void ListViewDoubleTapped(object sender, DoubleTappedRoutedEventArgs args)
        {
            this.AddOrEdit();
        }

        [RelayCommand]
        private void SendToast()
        {
            if (ToastWithAvatar.SendToast())
                NotificationShared.ToastSentSuccessfully();
            else
                NotificationShared.CouldNotSendToast();
        }

        [RelayCommand]
        private void SendToastWithText()
        {
            if (ToastWithText.SendToast())
                NotificationShared.ToastSentSuccessfully();
            else
                NotificationShared.CouldNotSendToast();
        }
    }
}
