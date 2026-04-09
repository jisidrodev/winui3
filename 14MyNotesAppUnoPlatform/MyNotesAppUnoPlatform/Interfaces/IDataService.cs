namespace MyNotesAppUnoPlatform.Interfaces
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
