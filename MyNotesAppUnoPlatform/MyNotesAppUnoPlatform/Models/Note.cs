namespace MyNotesAppUnoPlatform.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
        public EnumNoteType EnumNoteType { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly UpdateAt { get; set; }
    }
}
