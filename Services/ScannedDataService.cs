using TestTask.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using TestTask.Objects;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.Mvc;
namespace TestTask.Services
{
    public class ScannedDataService : IScannedDataService
    {
        private readonly JsonData? _jsonData = GetData();



        private static JsonData? GetData()
        {

            FileStream fs = new("data.json", FileMode.OpenOrCreate);
            JsonSerializerOptions JsonOptions = new()
            {
                PropertyNameCaseInsensitive = true, // не учитываем регистр
                WriteIndented = true,                // отступы для красоты
            };
            JsonData? data = JsonSerializer.Deserialize<JsonData>(fs, JsonOptions);
            fs.Close();
            return data;
        }

        public JsonData? AllData()
        {
            return _jsonData;
        }

        public NodeScan? Scan()
        {
            return _jsonData?.Scan;
        }

        public IList<string> Filenames(bool correct)
        {

            var FilesNames = _jsonData?.Files?.Where(f=> f.Result==correct).Select(f=>f.FileName).ToList();
            return FilesNames!;
        }

        public IList<BrokenFile> Errors()
        {
            var Errors = _jsonData?.Files?.Where(f => f.Result == false).Select(f => new BrokenFile
            {
                Filename = f.FileName,
                Errors = f.Errors.Select(e => e.Error).ToList()
            }).ToList();
            return Errors!;
        }

        public FileQuery? Query()
        {
            var queries = _jsonData?.Files?.Where(f => f.FileName!.ToLower().TrimStart().StartsWith("query_")).ToList(); ;
            int total = queries!.Count;
            var BrokenQueries = queries.Where(f=> f.Result == false).Select(f=>f.FileName).ToList();
            int broken = BrokenQueries.Count;
            int correct = total - broken;

            var result = new FileQuery
            {
                Total = total,
                Correct = correct,
                Errors = broken,
                Filenames = BrokenQueries
            };
            return result;

        }

    }
}
