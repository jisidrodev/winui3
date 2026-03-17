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
    public class MainViewModel : BindableBase
    {
        private string _selectedNote = String.Empty;
        private ObservableCollection<Note>? _items = null;
        private ObservableCollection<Note>? _allNotes = null;
        private IList<string>? _notesType = null;
        private Note? _selectedNoteItem = null;
        private int _additionalItemCount = 1;
        private bool _canDeleteItem => _selectedNoteItem != null;
        public ICommand? AddEditCommand {  get; set; }
        public ICommand? DeleteCommand {  get; set; }

        public MainViewModel()
        {
            PopulateData();
            DeleteCommand = new RelayCommand(DeleteItem, () => _canDeleteItem);
            //No CanExecute param is needed for this command
            //beacause you can always add or edit items.
            AddEditCommand = new RelayCommand(AddOrEditItem);
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



            _items = new ObservableCollection<Note>()
            {
                note,
                note2,
                note3,
                note4
            };

            _allNotes = new ObservableCollection<Note>(_items);


            _notesType = new ObservableCollection<string>()
            {
                "All",
                nameof(EnumNoteType.Task),
                nameof(EnumNoteType.Note)
            };
            if(NoteTypes != null)
                SelectedNote = NoteTypes[0];
        }

        public ObservableCollection<Note>? Items
        {
            get
            {
                return _items;
            }
            set
            {
                SetProperty(ref _items, value);
            }
        }

        public IList<string>? NoteTypes
        {
            get
            {
                return _notesType;
            }
            set
            {
                SetProperty(ref _notesType, value);
            }
        }

        public string SelectedNote
        {
            get
            {
                return _selectedNote;
            }
            set
            {
                SetProperty(ref _selectedNote, value);

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
        }

        public Note? SelectedNoteItem
        {
            get => _selectedNoteItem;
            set
            {
                SetProperty(ref _selectedNoteItem, value);
                if(DeleteCommand != null)
                    ((RelayCommand)DeleteCommand).RaiseCanExecuteChanged();
            }
        }

        public void AddOrEditItem()
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

        public void DeleteItem()
        {
            if(SelectedNoteItem != null)
            {
                Items?.Remove(SelectedNoteItem);
                _allNotes?.Remove(SelectedNoteItem);
            }
        }
    }
}
