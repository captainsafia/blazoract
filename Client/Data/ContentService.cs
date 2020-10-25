using System;
using System.Collections.Generic;
using blazoract.Shared;

namespace blazoract.Client.Data
{
    public class ContentService
    {

        private static List<Cell> Content { get; set; } = new List<Cell>();
        public event Action OnChange;

        public ContentService()
        {
            for (var i = 0; i < 100; i++)
            {
                Content.Add(new Cell($"{i} * {i}", i));
            }
        }
        public List<Cell> GetInitialContent()
        {
            return Content;
        }

        public List<Cell> AddCell(string content, CellType type, int position)
        {
            Content.Insert(position, new Cell(content, position, type));
            OnChange.Invoke();
            return Content;
        }

    }
}