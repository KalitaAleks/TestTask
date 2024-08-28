using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TestTask.Objects;

namespace TestTask.Controllers
{
    [Route("api")]
    [ApiController]
    public class ServiceController : Controller
    {
        /// <summary>
        /// Получение информации о сервере
        /// </summary>
        [HttpGet("service/serviceInfo")]
        public IActionResult ServiceInfo()
        {

        ServiceInfo serviceInfo = new()
            {
                SeviceAppName = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Name,
                SeviceVersion = System.Reflection.Assembly.GetEntryAssembly()!.GetName().Version!.ToString(),
                SeviceDateUtc = DateTime.Now.ToUniversalTime()
            };
            return Ok(serviceInfo);
        }

    }
}
