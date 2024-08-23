namespace TestTask.Objects
{
    public class FileQuery
    {
        public int Total { get; set; } = 0;
        public int Correct { get; set; } = 0;
        public int Errors { get; set; } = 0;
        public List<string?> Filenames { get; set; } = new();
    }
}



