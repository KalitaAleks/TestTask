using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TestTask.Objects;
using TestTask.Services.Interfaces;

namespace TestTask.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : Controller
    {


        JsonSerializerOptions JsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true, // не учитываем регистр
            WriteIndented = true,                // отступы для красоты
        };

        private readonly IScannedDataService _scanInfoService;

        public ApiController(IScannedDataService scanInfoService)
        {
            _scanInfoService = scanInfoService;

        }

        /// <summary>
        /// Получает все данные из файла data.json
        /// </summary>
        [HttpGet("data/all")]
        public IActionResult AllData()
        {
            return Json(_scanInfoService.AllData(), JsonOptions);
        }

        /// <summary>
        /// Получает данные об объекте сканирования: время сканирования, бд, сервер, количество ошибок
        /// </summary>
        [HttpGet("scan")]
        public IActionResult Scan()
        {
            return Ok(_scanInfoService.Scan());
        }

        /// <summary>
        /// Получает список имен файлов, у которых свойство result = correct.
        /// </summary>
        [HttpGet("filenames")]
        public IActionResult Filenames(bool correct)
        {
            return Ok(_scanInfoService.Filenames(correct));
        }

        /// <summary>
        /// Получает список файлов с ошибками или один файл по его номеру в списке
        /// </summary>
        [HttpGet("errors")]
        public IActionResult Errors()
        {
            return Ok(_scanInfoService.Errors());
        }

        /// <summary>
        /// Получает один файл из списка файлов с ошибками по его номеру в списке
        /// </summary>
        [HttpGet("errors/{index}")]
        public IActionResult Errors(int index)
        {
            return Ok(_scanInfoService.Errors()[index]); 

        }

        /// <summary>
        /// Получает количество файлов с ошибками
        /// </summary>
        [HttpGet("errors/count")]
        public IActionResult ErrorsCount() 
        {
            return Ok(_scanInfoService.Scan()?.ErrorCount);
        }

        /// <summary>
        /// Получает статистику по файлам query_%
        /// </summary>
        [HttpGet("query/check")]
        public IActionResult Query()
        {
            return Ok(_scanInfoService.Query());
        }


        /// <summary>
        /// Отправка новых данных на сервер и запись их в файл
        /// </summary>
        [HttpPost("errors")]
        public IActionResult Errors([FromBody] JsonData newData)
        {
            try
            {
                string Date = DateTime.Now.ToString().Replace(" ", "_").Replace(".", "-").Replace(":", "-");
                FileStream fs = new("log/" + Date + ".json", FileMode.OpenOrCreate);
                JsonSerializer.Serialize(fs, newData, JsonOptions);
                fs.Close();
                JsonData? data = _scanInfoService.AllData();
                foreach (var xfile in newData!.Files!)
                {
                    data!.Files!.Add(xfile);
                }
                data!.Scan!.ErrorCount += newData!.Scan!.ErrorCount;
                fs = new("data.json", FileMode.OpenOrCreate);
                JsonSerializer.Serialize(fs, data, JsonOptions);
                fs.Close();

                return Ok(newData);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(new { message = "incorrect data: " + ex.Message });
            }

        }

    }
}
