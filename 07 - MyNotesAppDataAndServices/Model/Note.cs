using Dapper.Contrib.Extensions;
using Microsoft.UI.Xaml.Controls;
using MyNotesApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNotesApp.Model
{
    public class Note
    {
        /// <summary>
        /// To delete a Note is necessary to add Key attribute
        /// </summary>
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string Content { get; set; } = String.Empty;
        public EnumNoteType EnumNoteType { get; set; }
        public DateOnly CreatedAt { get; set; } = DateOnly.FromDateTime(DateTime.Now);
        public DateOnly UpdateAt { get; set; }

        /// <summary>
        /// Dapper ignores this property
        /// </summary>
        //[Computed]
        //public Person Person { get; set; }
    }
}
