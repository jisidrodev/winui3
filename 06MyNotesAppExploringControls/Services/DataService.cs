using MyNotesApp.Enums;
using MyNotesApp.Interfaces;
using MyNotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotesApp.Services
{
    public class DataService : IDataService
    {
        private IList<EnumNoteType>? _noteTypes;
        private IList<Note>? _notes;
        public DataService()
        {
            this.PopulateNoteTypes();
            this.PopulateNotes();
        }

        private void PopulateNoteTypes()
        {
            _noteTypes = new List<EnumNoteType> { EnumNoteType.Note, EnumNoteType.Task };
        }

        private void PopulateNotes()
        {
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

            _notes = new List<Note>
            {
                note,
                note2,
                note3,
                note4
            };
        }

        public int SelectedNoteId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public int AddNote(Note note)
        {
            int id = 0;
            if(_notes != null)
            {
                note.Id = _notes.Max(i => i.Id) + 1;
                _notes.Add(note);

                id = note.Id;
            }
            return id;
        }

        public IList<EnumNoteType> GetEnumNoteTypes()
        {
            if (_noteTypes == null)
                throw new Exception($"The property {nameof(_noteTypes)} is empty.");
            return _noteTypes;
        }

        public Note GetNote(int id)
        {
            if (_notes == null)
                throw new Exception($"The property {nameof(_notes)} is empty.");
            var note = _notes.FirstOrDefault(i => i.Id == id);
            if (note == null)
                throw new Exception($"The note not exist.");
            return note;
        }

        public IList<Note> GetNotes()
        {
            if (_notes == null)
                throw new Exception($"The property {nameof(_notes)} is empty.");
            return _notes;
        }

        public void UpdateNote(Note note)
        {
            if(_notes == null)
                throw new Exception($"The property {nameof(_notes)} is empty.");

            var idx = -1;
            var matchedItem =
                (from x in _notes
                 let ind = idx++
                 where x.Id == note.Id
                 select ind).FirstOrDefault();

            if (idx == -1)
            {
                throw new Exception("Unable to update item. Item not found in collection.");
            }

            _notes[idx] = note;
        }

        public IList<Note> GetNotes(EnumNoteType enumNoteType)
        {
            if (_notes == null)
                throw new Exception($"The property {nameof(_notes)} is empty.");
            return _notes.Where(w => w.EnumNoteType == enumNoteType).ToList();
        }
    }
}
