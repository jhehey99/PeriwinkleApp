using System;

namespace PeriwinkleApp.Core.Sources.Models.Domain
{
    public class JournalEntry
    {
        public int? JournalEntryId { get; set; }
        public int? JournalClientId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public JournalEntry() { }
    }
}
