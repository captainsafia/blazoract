using System;

namespace blazoract.Shared
{
    public class ExecuteRequest
    {
        public ExecuteRequest(string input)
        {
            Input = input;
        }
        public string Input { get; set; }
    }
}
