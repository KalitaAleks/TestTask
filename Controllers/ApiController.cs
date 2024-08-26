using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using TestTask.Objects;

namespace TestTask.Controllers
{
    [Route("/{api?}")]
    public class ApiController : Controller
    {
        private static JsonSerializerOptions JsonOptions{ get; } = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true, // не учитываем регистр
            WriteIndented = true,                // отступы для красоты
        };

        private static JsonData? GetData()
        {
            FileStream fs = new("data.json", FileMode.OpenOrCreate);
            JsonData? data = JsonSerializer.Deserialize<JsonData>(fs, JsonOptions);
            fs.Close();
            return data;
        }

        private static readonly JsonData? data = GetData();

        public IActionResult Index()
        {
            return new HtmlResult();
        }

        [HttpGet("data/all")]
        public IActionResult GetAllData()
        {
            return Json(data, JsonOptions);
        }

        [HttpGet("scan")]
        public IActionResult GetScan()
        {
            return Json(data!.Scan, JsonOptions);
        }

        [HttpGet("filenames")]
        public IActionResult GetFilenames(bool correct)
        {
           
            List<string> FilesNames = new();
            foreach (var xfile in data!.Files!)
            {
                string filename = $"FileName: {xfile.FileName}";
                bool result = xfile.Result;
                if (result == correct)
                {
                    FilesNames.Add(filename);
                }
            }
            return Json(FilesNames, JsonOptions);
        }

        [HttpGet("errors/{index?}")]
        public IActionResult Errors(int? index)
        {
            List<BrokenFile> Errors = new();
            foreach (var xfile in data!.Files!)
            {
                string xfilename = $"FileName: {xfile.FileName}";
                if (xfile.Result == false)
                {
                    BrokenFile eFile = new()
                    {
                        Filename = xfilename
                    };
                    foreach (var xError in xfile.Errors)
                    {
                        eFile.Errors.Add(xError.Error);
                    }
                    Errors.Add(eFile);
                }
            }
            try
            {
                if (index.HasValue) { return Json(Errors[index.Value], JsonOptions); }
                else { return Json(Errors, JsonOptions); }
            }
            catch
            {
                return new BadRequestObjectResult(new { message = "incorrect data" });
            }
        }

        [HttpGet("errors/count")]
        public int ErrorsCount() 
        { 
            return data!.Scan!.ErrorCount;
        }

        [HttpGet("query/check")]
        public IActionResult Query()
        {
            FileQuery queries = new();
            foreach (var xfile in data!.Files!)
            {
                string filename = $"{xfile.FileName}";
                if (filename.Contains("query", StringComparison.CurrentCulture))
                {
                    bool result = xfile.Result;
                    if (result == false)
                    {
                        queries.Filenames.Add(filename);
                        queries.Errors++;
                    }
                    else { queries.Correct++; }
                    queries.Total++;
                }
            }
            return Json(queries, JsonOptions);
        }

        [HttpGet("service/serviceInfo")]
        public IActionResult ServiceInfo()
        {
            ServiceInfo serviceInfo = new()
            {
                SeviceAppName = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name,
                SeviceVersion = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Version!.ToString(),
                SeviceDateUtc = DateTime.Now.ToUniversalTime()
            };
            return Json(serviceInfo, JsonOptions);
        }

        [HttpPost("errors")]
        public IActionResult CreateNewData([FromBody] string newData)
        {
            try
            {
                string Date = DateTime.Now.ToString().Replace(" ", "_").Replace(".", "-").Replace(":", "-");
                JsonData? RestoreData = new();
                RestoreData = JsonSerializer.Deserialize<JsonData>(newData, JsonOptions);
                FileStream fs = new("log/" + Date + ".json", FileMode.OpenOrCreate);
                JsonSerializer.Serialize(fs, RestoreData, JsonOptions);
                fs.Close();

                foreach (var xfile in RestoreData!.Files!)
                { 
                  data!.Files!.Add(xfile);
                }
                data!.Scan!.ErrorCount += RestoreData!.Scan!.ErrorCount;
                fs = new("data.json", FileMode.OpenOrCreate);
                JsonSerializer.Serialize(fs, data, JsonOptions);
                fs.Close();

                return Json(RestoreData, JsonOptions);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult(new { message = "incorrect data" });
            }

        }

    }
}
