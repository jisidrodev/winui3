using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
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
        //[NotifyCanExecuteChangedFor(nameof(SaveCommand))]
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

        internal async Task InitializeNoteDetailDataAsync(int noteId)
        {
            _selectedNoteId = noteId;
            await this.PopulateExistingNoteAsync(_dataService);
            IsDirty = false;
        }

        private async Task PopulateExistingNoteAsync(IDataService dataService)
        {
            if (_selectedNoteId > 0)
            {
                var note = await _dataService.GetNoteAsync(_selectedNoteId);
                if (note != null)
                {
                    _noteId = note.Id;
                    Title = note.Title;
                    Content = note.Content;
                    SelectedNoteType = note.EnumNoteType.ToString();
                }
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

        //[RelayCommand( CanExecute = nameof(CanSaveNote))]
        private async Task SaveAsync()
        {
            Note? note = null;
            if (_noteId > 0)
            {
                note = await _dataService.GetNoteAsync(_noteId);
                if (note != null)
                {
                    note.Title = Title;
                    note.Content = Content;
                    note.EnumNoteType = (EnumNoteType)Enum.Parse(typeof(EnumNoteType), SelectedNoteType);
                    await _dataService.UpdateNoteAsync(note);
                }

            }
            else
            {
                note = new Note();
                note.Title = Title;
                note.Content = Content;
                note.EnumNoteType = (EnumNoteType)Enum.Parse(typeof(EnumNoteType), SelectedNoteType);
                await _dataService.AddNoteAsync(note);
            }

        }

        [RelayCommand]
        private async Task SaveNoteAndContinueAsync()
        {
            await this.SaveAsync();
            _noteId = 0;
            Title = string.Empty;
            Content = string.Empty;
            SelectedNoteType = string.Empty;
            _selectedNoteId = -1;
            IsDirty = false;
        }

        [RelayCommand]
        private async Task SaveNoteAndReturnAsync()
        {
            await this.SaveAsync();
            _navigationService.GoBack();

        }



        [RelayCommand]
        private void Cancel() => _navigationService.GoBack();
    }
}
