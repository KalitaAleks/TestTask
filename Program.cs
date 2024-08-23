using System.Reflection;
using System.Text.Json;
using System.Text.RegularExpressions;

var Builder = WebApplication.CreateBuilder(args);
Builder.Services.AddControllers();
var App = Builder.Build();
App.MapControllers();
App.Run();

