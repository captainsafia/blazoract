using System;

namespace blazoract.Shared
{
    public class ExecuteRequest
    {
        public ExecuteRequest(string notebookId, string code)
        {
            NotebookId = notebookId;
            Code = code;
        }

        public string NotebookId { get; set; }

        public string Code { get; set; }
    }
}
