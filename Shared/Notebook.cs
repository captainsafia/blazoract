using System;
using System.Collections.Generic;

namespace blazoract.Shared
{
    public class Notebook
    {
        public Notebook(string title, string notebookId)
        {
            Title = title;
            NotebookId = notebookId;
            Created = DateTime.Now;
            Updated = Created;
        }
        public string Title { get; set; }

        public string NotebookId { get; set; }

        public List<Cell> Cells { get; set; }

        public DateTime Created { get; set; }

        public DateTime Updated { get; set; }
    }
}
