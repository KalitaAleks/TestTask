//using System.Reflection;
//using System.Text.Json;

//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();
//app.UseDefaultFiles();
//app.UseStaticFiles();
//builder.Configuration.AddJsonFile("data.json");
//JsonData data = new JsonData();
//app.Configuration.Bind(data);
//List<ErorrsFile> Errors = GetErrorFiles(data);

//app.MapGet("/api/allData", (context) => context.Response.SendFileAsync("data.json"));
//app.MapGet("api/Scan", () => 
//    {
//        string? scanTime = data.Scan.ScanTime;
//        string? db = data.Scan.Db;
//        string? server = data.Scan.Server;
//        int errorCount = data.Scan.ErrorCount;
//        return "Scan: \n ScanTime: " + scanTime +"\n Db: " + db + "\n Server: " + server + "\n ErorsCount: " + errorCount;
//    });
//app.MapGet("api/filenames/{value}", (bool value) =>
//    {
//        List<string> FilesNames = new();
//        foreach (var xfile in data.Files)
//        {
//            string filename = $"FileName: {xfile.Filename}";
//            bool result = xfile.Result;
//            if (result == value)
//            {
//                FilesNames.Add(filename);
//            }
//        }
//        return FilesNames;
//    });
//app.MapGet("api/errors", () =>
//    {
//            return Errors;
//    });
//app.MapGet("api/errors/{index}", (int index) =>
//{
//    return Errors[index];
//});
//app.MapGet("api/errors/count", () =>
//{
//    int errorCount = data.Scan.ErrorCount;
//    return "ErorsCount: " + errorCount;
//});
//app.MapGet("api/query/check", () =>
//{
//    FileQuery querys = new FileQuery();
//    foreach (var xfile in data.Files)
//    {
//        string filename = $"{xfile.Filename}";
//        if (filename.IndexOf("query") !=-1)
//            {
//            bool result = xfile.Result;
//            if (result == false)
//            {
//                querys.Filenames.Add(filename);
//                querys.Errors++;
//            }
//            else { querys.Correct++; }
//            querys.Total++;

//        }

//    }
//    return querys;


//});
//app.MapPost("api/newErrors", (string json) =>
//{
    
//    string Date = DateTime.Now.ToString().Replace(" ", "_").Replace(".", "-").Replace(":", "-");
//    JsonData? RestoreData = JsonSerializer.Deserialize<JsonData>(json)!;
//    File.WriteAllText(Date + ".json", json);
//    return json;
//});
//app.MapGet("api/service/serviceInfo", ()=>
//{
//    ServiceInfo serviceInfo = new ServiceInfo();
//    serviceInfo.AppName = Assembly.GetEntryAssembly()!.GetName().Name;
//    serviceInfo.Version = Assembly.GetEntryAssembly()!.GetName().Version!.ToString();
//    serviceInfo.DateUtc = DateTime.Now.ToUniversalTime();
//    return serviceInfo;
//});

//app.Run();

//List <ErorrsFile> GetErrorFiles(JsonData xdata)
//{
//    List<ErorrsFile> Errors = new();
//    foreach (var xfile in xdata.Files)
//    {
//        string xfilename = $"FileName: {xfile.Filename}";
//        bool result = xfile.Result;
//        if (result == false)
//        {
//            ErorrsFile eFile = new();
//            eFile.Filename = xfilename;
//            foreach (var xError in xfile.Errors)
//            {
//                eFile.Error.Add(xError.Error);
//            }
//            Errors.Add(eFile);

//        }
//    }
//    return Errors;

//}
//public class JsonData
//{
//    public NodeScan Scan { get; set; } = new();
//    public List<NodeFiles> Files { get; set; } = new();
//}
//public class NodeScan
//{ 
//    public string? ScanTime { get; set; }
//    public string? Db { get; set; }
//    public string? Server { get; set; }
//    public int ErrorCount { get; set; }
//}
//public class NodeFiles
//{
//    public string? Filename { get; set; }
//    public bool Result { get; set; }
//    public List<ErrorInFile> Errors { get; set; } = new();
//    public string? Scantime { get; set; }
//}
//public class ErrorInFile
//{
//    public string? Module { get; set; }
//    public int Ecode { get; set; }
//    public string? Error { get; set; }
//}
//public class ErorrsFile
//{
//    public string? Filename { get; set; }
//    public List<string?> Error { get; set; }= new();
//}
//public class FileQuery
//{
//    public int Total { get; set; } = 0;
//    public int Correct { get; set; } = 0;
//    public int Errors { get; set; } = 0;
//    public List<string?> Filenames { get; set; } = new();
//}
//public class ServiceInfo
//{
//    public string? AppName { get; set; }
//    public string? Version{ get; set; }
//    public DateTime DateUtc{ get; set; }
//}


