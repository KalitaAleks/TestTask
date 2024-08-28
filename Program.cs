using TestTask.Services.Interfaces;
using TestTask.Services;


var Builder = WebApplication.CreateBuilder(args);
Builder.Services.AddScoped<IScannedDataService, ScannedDataService>();
Builder.Services.AddControllers();
Builder.Services.AddEndpointsApiExplorer();
Builder.Services.AddSwaggerGen();
var App = Builder.Build();
if (App.Environment.IsDevelopment())
{
    App.UseSwagger();
    App.UseSwaggerUI();
}
App.UseHttpsRedirection();
App.MapControllers();
App.Run();

