using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Documents;
using MyNotesApp.Enums;
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
        private ObservableCollection<Note>? _items = null;

        private ObservableCollection<Note>? _allNotes = null;

        [ObservableProperty]
        private IList<string>? _notesTypes = null;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
        private Note? _selectedNoteItem = null;

        private int _additionalItemCount = 1;
        private bool _canDeleteItem => _selectedNoteItem != null;

        public MainViewModel()
        {
            PopulateData();
          
        }

        public void PopulateData()
        {
            //if (_isLoaded) return;

            Note note = new Note();
            note.Id = 1;
            note.Title = "My first note";
            note.Content = "This a text of example";
            note.EnumNoteType = Enums.EnumNoteType.Note;


            Note note2 = new Note();
            note2.Id = 2;
            note2.Title = "My first task";
            note2.Content = "This a text of example";
            note2.EnumNoteType = Enums.EnumNoteType.Task;

            Note note3 = new Note();
            note3.Id = 3;
            note3.Title = "My second note";
            note3.Content = "This a text of example";
            note3.EnumNoteType = Enums.EnumNoteType.Note;

            Note note4 = new Note();
            note4.Id = 4;
            note4.Title = "My second task";
            note4.Content = "This a text of example";
            note4.EnumNoteType = Enums.EnumNoteType.Task;



            Items = new ObservableCollection<Note>()
            {
                note,
                note2,
                note3,
                note4
            };

            _allNotes = new ObservableCollection<Note>(Items);


            NotesTypes = new ObservableCollection<string>()
            {
                "All",
                nameof(EnumNoteType.Task),
                nameof(EnumNoteType.Note)
            };

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
            // Note this is temporary until
            // we use a real data source for items.
            const int startingItemCount = 4;

            var newItem = new Note
            {
                Id = startingItemCount + _additionalItemCount,
                Title = "My task " + (startingItemCount + _additionalItemCount),
                Content = "This a text of example",
                EnumNoteType = Enums.EnumNoteType.Task,
            };

            Items?.Add(newItem);
            _additionalItemCount++;
        }

        [RelayCommand(CanExecute = nameof(_canDeleteItem))]
        public void Delete()
        {
            if (SelectedNoteItem != null)
            {
                Items?.Remove(SelectedNoteItem);
                _allNotes?.Remove(SelectedNoteItem);
            }
        }
    }
}
