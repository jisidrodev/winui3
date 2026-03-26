using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using MyNotesApp.Enums;
using MyNotesApp.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

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

        private bool _isLoaded = false;
        private List<Note>? _items = null;
        private IList<string>? _notesType = null;
        private IList<Note>? _allNotes = null;
        public MainWindow()
        {
            InitializeComponent();
            ItemList.Loaded += ItemList_Loaded;
            ItemFilter.Loaded += ItemFilter_Loaded;
            ItemFilter.SelectionChanged += ItemFilter_SelectionChanged;
        }

        private void ItemFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var updatedItems = (from item in _allNotes
                                where
                                    string.IsNullOrWhiteSpace(ItemFilter.SelectedValue.ToString()) ||
                                    ItemFilter.SelectedValue.ToString() == "All" ||
                                    ItemFilter.SelectedValue.ToString() == item.EnumNoteType.ToString()
                                select item).ToList();
            ItemList.ItemsSource = updatedItems;



        }

        private void ItemFilter_Loaded(object sender, RoutedEventArgs e)
        {
            var filterCombo = (ComboBox)sender;
            PopulateData();
            filterCombo.ItemsSource = _notesType;
            filterCombo.SelectedIndex = 0;
        }

        private void ItemList_Loaded(object sender, RoutedEventArgs e)
        {
            var listView = (ListView)sender;
            this.PopulateData();
            listView.ItemsSource = _items;
        }

        public void PopulateData()
        {
            if (_isLoaded) return;

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
            note4.Id = 2;
            note4.Title = "My second task";
            note4.Content = "This a text of example";
            note4.EnumNoteType = Enums.EnumNoteType.Task;

            _allNotes = new List<Note>()
            {
                note,
                note2,
                note3,
                note4
            };

            _items = new List<Note>()
            {
                note,
                note2,
                note3,
                note4
            };



            _notesType = new List<string>()
            {
                "All",
                nameof(EnumNoteType.Task),
                nameof(EnumNoteType.Note)
            };

        }

        private async void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog();
            dialog.Title = "My app notes";
            dialog.Content = "Adding notes to the collection in not yet supported.";
            dialog.CloseButtonText = "OK";
            dialog.XamlRoot = Content.XamlRoot;

            await dialog.ShowAsync();
        }
    }


}
