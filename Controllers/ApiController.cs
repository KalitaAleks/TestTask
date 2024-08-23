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

        static JsonData? GetData()
        {
            FileStream fs = new("data.json", FileMode.OpenOrCreate);
            JsonData? data = JsonSerializer.Deserialize<JsonData>(fs, JsonOptions);
            fs.Close();
            return data;
        }

        public IActionResult Index()
        {
            return new HtmlResult();
        }

        [Route("AllData"), HttpGet]
        public IActionResult GetAllData()
        {
            return Json(GetData(), JsonOptions);
        }

        [Route("Scan"), HttpGet]
        public IActionResult GetScan()
        {
            JsonData? data = GetData();
            return Json(data!.Scan, JsonOptions);
        }

        [Route("filenames"), HttpGet]
        public IActionResult GetFilenames(bool correct)
        {
            JsonData? data = GetData();
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

        [Route("errors/{index?}"), HttpGet]
        public IActionResult Errors(int? index)
        {
            JsonData? data = GetData();
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

        [Route("errors/count"), HttpGet]
        public int ErrorsCount() 
        { 
            JsonData? data = GetData();
            return data!.Scan!.ErrorCount;
        }

        [Route("query/check"), HttpGet]
        public IActionResult Query()
        {
            JsonData? data = GetData();
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

        [Route("service/serviceInfo"), HttpGet]
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

        [Route("newErrors"), HttpPost]
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
                return Json(RestoreData, JsonOptions);
            }
            catch (Exception)
            {
                return new BadRequestObjectResult(new { message = "incorrect data" });
            }

        }

    }
}
