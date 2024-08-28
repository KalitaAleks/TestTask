using TestTask.Objects;
namespace TestTask.Services.Interfaces
{
    public interface IScannedDataService
    {
        JsonData? AllData();
        NodeScan? Scan();
        IList<string> Filenames(bool correct);
        IList<BrokenFile> Errors();
        FileQuery? Query();
    }
}
