using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.IO;
using Windows.Storage;
using MyNotesApp.Model;
using MyNotesApp.Enums;
using Dapper;
using Dapper.Contrib.Extensions;
using MyNotesApp.Interfaces;

namespace MyNotesApp.Services
{
    public class SqliteDataService : IDataService
    {
        private const string DBNAME = "noteData.db";
        private IList<Note> _notes = new List<Note>();

        public int SelectedNoteId { get; set; }

        public SqliteDataService()
        {

        }

        public async Task InitializeDataAsync()
        {
            using (var db = await GetOpenConnectionAsync())
            {
                await CreateNoteTableAsync(db);
                await PopulateNotesAsync(db);
            }
        }

        public async Task AddNoteAsync(Note note)
        {
            using var connection = await GetOpenConnectionAsync();
            await InsertNoteWithDapperAsync(connection, note);
        }

        public async Task UpdateNoteAsync(Note note)
        {
            using var connection = await GetOpenConnectionAsync();
            await UpdateNoteAsync(connection, note);
        }

        public async Task DeleteNoteAsync(int id)
        {
            using var connection = await GetOpenConnectionAsync();
            await DeleteNoteAsync(connection, id);
        }

        public async Task<IList<Note>> GetAllNotesAsync()
        {
            IList<Note> notes = new List<Note>();
            using var connection = await GetOpenConnectionAsync();
            notes = await GetAllNotesWithDapperAsync(connection);
            return notes;
        }

        public async Task<Note?> GetNoteAsync(int id)
        {
            IList<Note> notes = new List<Note>();
            using var connection = await GetOpenConnectionAsync();
            notes = await GetAllNotesWithDapperAsync(connection);
            //Filter the list to get the note for our id.
            return notes.FirstOrDefault(f => f.Id == id);
        }

        private async Task PopulateNotesAsync(SqliteConnection connection)
        {
            _notes = await GetAllNotesWithDapperAsync(connection);
            if (_notes.Count == 0)
            {
                Note note = new Note();
                note.Id = 1;
                note.Title = "My first note";
                note.Content = "This a text of example";
                note.EnumNoteType = Enums.EnumNoteType.Note;
                await InsertNoteWithDapperAsync(connection, note);

                _notes = await GetAllNotesWithDapperAsync(connection);
            }
        }

        private async Task<SqliteConnection> GetOpenConnectionAsync()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync(DBNAME, CreationCollisionOption.OpenIfExists).AsTask().ConfigureAwait(false);
            string dbPath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DBNAME);
            var cn = new SqliteConnection($"Filename={dbPath}");
            await cn.OpenAsync();
            return cn;
        }

        private async Task CreateNoteTableAsync(SqliteConnection connection)
        {
            string tableCommand = @"CREATE TABLE IF NOT EXISTS Notes (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                    Title NVARCHAR(50) NOT NULL,
                                    Content NVARCHAR(250) NOT NULL,
                                    NoteType INTEGER NOT NULL)";
            using var createTable = new SqliteCommand(tableCommand, connection);
            await createTable.ExecuteNonQueryAsync();
        }

        private async Task InsertNoteAsync(SqliteConnection connection, Note note)
        {
            using var insertCommand = new SqliteCommand
            {
                Connection = connection,
                CommandText = "INSERT INTO Notes VALUES(NULL,@Title, @Content, @NoteType);"
            };
            insertCommand.Parameters.AddWithValue("@Title", note.Title);
            insertCommand.Parameters.AddWithValue("@Content", note.Content);
            insertCommand.Parameters.AddWithValue("@NoteType", (int)note.EnumNoteType);
            await insertCommand.ExecuteNonQueryAsync();
        }

        private async Task InsertNoteWithDapperAsync(SqliteConnection connection, Note note)
        {
            var newIds = await connection.QueryAsync<long>($@"INSERT INTO Notes 
                                                            ( {nameof(note.Title)}, {nameof(note.Content)}, NoteType )
                                                            VALUES (@{nameof(note.Title)}, @{nameof(note.Content)}, @{nameof(note.EnumNoteType)});
                                                            SELECT last_insert_rowid();
                                                            ", note);
            note.Id = (int)newIds.First();
        }

        private async Task<IList<Note>> GetAllNotesWithDapperAsync(SqliteConnection connection)
        {
            var notes = await connection.QueryAsync<Note>(@" SELECT Id, Title, Content, NoteType AS EnumNoteType FROM Notes");

            return notes.ToList();
        }

        private async Task UpdateNoteAsync(SqliteConnection connection, Note note)
        {
            await connection.QueryAsync(@"  UPDATE Notes 
                                            SET Title @Title,
                                            Content = @Content,
                                            NoteType = @EnumNoteType
                                            WHERE Id = @Id;", note);

        }

        private async Task DeleteNoteAsync(SqliteConnection connection, int id)
        {
            await connection.DeleteAsync<Note>(new Note { Id = id });
        }

        public IList<Note> GetNotes()
        {
            throw new NotImplementedException();
        }

        public Note GetNote(int id)
        {
            throw new NotImplementedException();
        }

        public int AddNote(Note note)
        {
            throw new NotImplementedException();
        }

        public void UpdateNote(Note note)
        {
            throw new NotImplementedException();
        }

        public IList<EnumNoteType> GetEnumNoteTypes()
        {
            throw new NotImplementedException();
        }

        public IList<Note> GetNotes(EnumNoteType enumNoteType)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Example to get more of two object in the same query
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        //private async Task<List<MediaItem>> GetAllMediaItemsAsync(SqliteConnection db)
        //{
        //    var itemsResult = await db.QueryAsync<MediaItem, Medium, MediaItem>
        //                    (
        //                        @"SELECT
        //                            [MediaItems].[Id],
        //                            [MediaItems].[Name],
        //                            [MediaItems].[ItemType] AS MediaType,
        //                            [MediaItems].[LocationType] AS Location,
        //                            [Mediums].[Id],
        //                            [Mediums].[Name],
        //                            [Mediums].[MediumType] AS MediaType
        //                        FROM
        //                            [MediaItems]
        //                        JOIN
        //                            [Mediums]
        //                        ON
        //                            [Mediums].[Id] = [MediaItems].[MediumId]",
        //                        (item, medium) =>
        //                        {
        //                            item.MediumInfo = medium;
        //                            return item;
        //                        }
        //                    );

        //    return itemsResult.ToList();
        //}

    }
}
