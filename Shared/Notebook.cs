using System.Collections.Generic;

namespace blazoract.Shared
{
    public class Notebook
    {
        public Notebook(string title, string notebookId)
        {
            Title = title;
            NotebookId = notebookId;
        }
        public string Title { get; set; }

        public string NotebookId { get; set; }

        public List<Cell> Cells { get; set; }

    }
}
