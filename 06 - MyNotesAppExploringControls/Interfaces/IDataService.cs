using MyNotesApp.Enums;
using MyNotesApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotesApp.Interfaces
{
    public interface IDataService
    {
        IList<Note> GetNotes();
        Note GetNote(int id);
        int AddNote(Note note);
        void UpdateNote(Note note);
        IList<EnumNoteType> GetEnumNoteTypes();
        int SelectedNoteId {  get; set; }
        IList<Note> GetNotes(EnumNoteType enumNoteType);
    }
}
