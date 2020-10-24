using System;

namespace blazoract.Shared
{
    public class Cell
    {
        public Cell(string content, CellType type = CellType.Code)
        {
            Content = content;
            Type = type;
        }
        public string Content { get; set; }

        public CellType Type { get; set; }
    }

    public enum CellType
    {
        Code,
        Text
    }
}
