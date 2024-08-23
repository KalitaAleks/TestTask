namespace TestTask.Objects
{
    public class NodeFiles
    {
        public string? FileName { get; set; }
        public bool Result { get; set; }
        public List<ErrorInFile> Errors { get; set; } = new();
        public string? Scantime { get; set; }
    }
}



