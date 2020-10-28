using System;

namespace blazoract.Shared
{
    public class Cell
    {
        public Cell(string notebookId, string content, int position, CellType type = CellType.Code)
        {
            NotebookId = notebookId;
            Content = content;
            Position = position;
            Type = type;
        }

        public string NotebookId { get; set; }

        public string Content { get; set; }

        public CellType Type { get; set; }

        public int Position { get; set; }
    }

    public enum CellType
    {
        Code,
        Text
    }
}
