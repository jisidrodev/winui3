using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MyNotesApp.Enums;
using MyNotesApp.Interfaces;
using MyNotesApp.Model;
using MyNotesApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotesApp.ViewsModels
{
    public partial class NoteDetailViewModel : ObservableObject
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService _dataService;
        private int _noteId = -1;

        [ObservableProperty]
        private string _title = String.Empty;

        [ObservableProperty]
        private string _content = String.Empty;

        [ObservableProperty]
        private ObservableCollection<string> _noteTypes = new();

        [ObservableProperty]
        private string _selectedNoteType = String.Empty;
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
        private bool _isDirty = false;

        private int _selectedNoteId = -1;

        public NoteDetailViewModel(INavigationService navigationService, IDataService dataService)
        {
            this._navigationService = navigationService;
            this._dataService = dataService;
            this.PopulateLists();
        }

        private void PopulateLists()
        {
            NoteTypes.Clear();
            foreach (var ntype in Enum.GetNames(typeof(EnumNoteType)))
            {
                NoteTypes.Add(ntype);
            }
        }

        internal void InitializeNoteDetailData(int noteId)
        {
            _selectedNoteId = noteId;
            this.PopulateExistingNote(_dataService);
            IsDirty = false;
        }

        private void PopulateExistingNote(IDataService dataService)
        {
            if (_selectedNoteId > 0)
            {
                var note = _dataService.GetNote(_selectedNoteId);
                _noteId = note.Id;
                Title = note.Title;
                Content = note.Content;
                SelectedNoteType = note.EnumNoteType.ToString();
            }
        }

        partial void OnTitleChanged(string value)
        {
            IsDirty = true;
        }

        partial void OnContentChanged(string value)
        {
            IsDirty = true;
        }

        partial void OnSelectedNoteTypeChanged(string value)
        {
            IsDirty = true;
        }

        private bool CanSaveNote()
        {
            return IsDirty;
        }

        [RelayCommand( CanExecute = nameof(CanSaveNote))]
        private void Save()
        {
            Note? note = null;
            if(_noteId > 0)
            {
                note = _dataService.GetNote(_noteId);
                note.Title  = Title;
                note.Content = Content;
                note.EnumNoteType = (EnumNoteType)Enum.Parse(typeof(EnumNoteType),SelectedNoteType);
                _dataService.UpdateNote(note);
            }
            else
            {
                note = new Note();
                note.Title = Title;
                note.Content = Content;
                note.EnumNoteType = (EnumNoteType)Enum.Parse(typeof(EnumNoteType), SelectedNoteType);
                _dataService.AddNote(note);
            }

            _navigationService.GoBack();
        }

        [RelayCommand]
        private void Cancel() => _navigationService.GoBack();
    }
}
