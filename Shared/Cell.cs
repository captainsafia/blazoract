using System;

namespace blazoract.Shared
{
    public class Cell
    {
        public Cell(string content, int position, CellType type = CellType.Code)
        {
            Content = content;
            Position = position;
            Type = type;
        }
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
