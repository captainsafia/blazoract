using System;

namespace blazoract.Shared
{
    public class ExecuteResult
    {
        public ExecuteResult() { }
        public ExecuteResult(object output)
        {
            Output = output;
        }

        public object Output { get; set; }
    }
}
