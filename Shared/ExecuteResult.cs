namespace blazoract.Shared
{
    public class ExecuteResult
    {
        // Tell the client which kind of object we're sending, so it can try to JSON-deserialize it as that type
        public string OutputType { get; set; }
        public string OutputJson { get; set; }

        // In case the client can't deserialize this type (or the server can't serialize it), fall back on a string representation
        public string OutputToString { get; set; }
    }
}
