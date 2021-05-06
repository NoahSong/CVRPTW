using CVRPTW.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CVRPTW.Web.Controllers
{
    public class ClientAppController : Controller
    {
        private readonly IOptions<AppSettingsModel> _appSettings;

        public ClientAppController(IOptions<AppSettingsModel> appSettings)
        {
            _appSettings = appSettings;
        }

        [HttpGet, Route("")]
        public ViewResult ServeClient()
        {
            ViewBag.GoogleMapsApiKey = _appSettings.Value.Google.Maps.ApiKey;
            return View("~/Views/ClientApp.cshtml");
        }
    }
}