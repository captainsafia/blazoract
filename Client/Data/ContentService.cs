using System;
using System.Collections;
using blazoract.Shared;

namespace blazoract.Client.Data
{
    public class ContentService
    {

        private static ArrayList Content { get; set; } = new ArrayList() {
            { new Cell("1+1") },
            { new Cell("2+2") },
            { new Cell("this is a text cell", CellType.Text)},
            { new Cell("3+3") },

        };

        public event Action OnChange;

        public ContentService() { }
        public ArrayList GetInitialContent()
        {
            return Content;
        }

        public ArrayList AddCell(string content, CellType type, int position)
        {
            Content.Insert(position, new Cell(content, type));
            OnChange.Invoke();
            return Content;
        }

    }
}