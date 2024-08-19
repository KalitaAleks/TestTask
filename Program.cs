using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

var Builder = WebApplication.CreateBuilder(args);
var App = Builder.Build();
Builder.Configuration.AddJsonFile("data.json");
var  Data = new JsonData();
App.Configuration.Bind(Data);
List<BrokenFile> ListErrorFiles = GetErrorFiles(Data);

App.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;
    string expressionForGuidErrors = @"^/api/errors/\w{}$";
    string expressionForValue = @"^/api/filenames/\w{1}$";

    if (path == "/api/allData" && request.Method == "GET")
    {
        await GetAllData(response);
    }
    else if (path == "/api/Scan" && request.Method == "GET")
    {
        await GetScan(response, Data);
    }
    else if (Regex.IsMatch(path, expressionForValue) && request.Method == "GET")
    {

        bool value = Convert.ToBoolean(Convert.ToInt32(path.Value?.Split("/")[3]));
        await GetFilenames(value, response);
    }
    else if (path == "/api/errors" && request.Method == "GET")
    {
        int? index = null;
        await GetErrorsFiles(index, response);
    }
    else if (Regex.IsMatch(path, expressionForGuidErrors) && request.Method == "GET")
    {
        int? index = Convert.ToInt32(path.Value?.Split("/")[3]);
        await GetErrorsFiles(index, response);
    }
    else if (path == "/api/errors/count" && request.Method == "GET")
    {
        await GetErorsCount(response, Data);
    }
    else if (path == "/api/query/check" && request.Method == "GET")
    {
        await GetQuerys(response);
    }
    else if (path == "/api/service/serviceInfo" && request.Method == "GET")
    {
        await GetServiceInfo(response);
    }
    else if (path == "/api/newErrors" && request.Method == "POST")
    {
        await CreateNewData(response, request);
    }
    else
    {
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/index.html");
    }

});


App.Run();
async Task GetAllData(HttpResponse response)
{
    await response.SendFileAsync("data.json");
}
async Task GetScan(HttpResponse response, JsonData json)
{
    var Scan = json.Scan;
    await response.WriteAsJsonAsync(Scan);
}
async Task GetFilenames(bool? value, HttpResponse response)
{
    List<string> FilesNames = new();
            foreach (var xfile in Data.Files!)
            {
                string filename = $"FileName: {xfile.FileName}";
                bool result = xfile.Result;
                if (result == value)
                {
                    FilesNames.Add(filename);
               }
            }
    await response.WriteAsJsonAsync(FilesNames);
}
async Task GetErrorsFiles(int? index, HttpResponse response)
{
    try
    {
        if (index == null)
        {
            await response.WriteAsJsonAsync(ListErrorFiles);
        }
        else
        {
            await response.WriteAsJsonAsync(ListErrorFiles[index.Value]);
        }
    }
    catch (Exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "incorrect data" });
    }
}
async Task GetErorsCount(HttpResponse response, JsonData json)
{
    int errorCount = json.Scan!.ErrorCount;
    await response.WriteAsJsonAsync(errorCount);
}
async Task GetQuerys(HttpResponse response)
{
    FileQuery querys = new();
    foreach (var xfile in Data.Files!)
    {
        string filename = $"{xfile.FileName}";
        if (filename.Contains("query", StringComparison.CurrentCulture))
        {
            bool result = xfile.Result;
            if (result == false)
            {
                querys.QuerysFilenames.Add(filename);
                querys.ErrorsCount++;
            }
            else { querys.QCorrect++; }
            querys.Total++;

        }
    }
    await response.WriteAsJsonAsync(querys);
}
async Task GetServiceInfo(HttpResponse response)
{
    ServiceInfo serviceInfo = new()
    {
        SeviceAppName = Assembly.GetEntryAssembly()!.GetName().Name,
        SeviceVersion = Assembly.GetEntryAssembly()!.GetName().Version!.ToString(),
        SeviceDateUtc = DateTime.Now.ToUniversalTime()
    };
    await response.WriteAsJsonAsync(serviceInfo);
}
async Task CreateNewData(HttpResponse response, HttpRequest request)
{
    
    try
    {
        // получаем сроку Json
        string? newData = await request.ReadFromJsonAsync<string?>();
        string Date = DateTime.Now.ToString().Replace(" ", "_").Replace(".", "-").Replace(":", "-");
        JsonData? RestoreData = new();
        RestoreData = JsonSerializer.Deserialize<JsonData>(newData!);
        File.WriteAllText(Date + ".json", newData);
        await response.SendFileAsync(Date + ".json");
    }
    catch (Exception)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = "incorrect data" });
    }
}

List<BrokenFile> GetErrorFiles(JsonData xdata)
{
    List<BrokenFile> Errors = new();
    foreach (var xfile in xdata.Files!)
    {
        string xfilename = $"FileName: {xfile.FileName}";
        bool result = xfile.Result;
        if (result == false)
        {
            BrokenFile eFile = new()
            {
                Filename = xfilename
            };
            foreach (var xError in xfile.Errors)
            {
                eFile.StrErrors.Add(xError.ErrorDescription);
            }
            Errors.Add(eFile);

        }
    }
    return Errors;

}



