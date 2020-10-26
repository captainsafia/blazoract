namespace blazoract.Shared
{
    public class GetCompletionsRequest
    {
        public string Code { get; set; }
        public int LineNumber { get; set; }
        public int Column { get; set; }
    }
}
