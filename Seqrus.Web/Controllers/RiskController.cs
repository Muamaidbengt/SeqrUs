using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Seqrus.Web.Helpers;
using Seqrus.Web.Services;
using Seqrus.Web.Services.Rendering;

namespace Seqrus.Web.Controllers
{
    public class RiskController : Controller
    {
        private readonly ComplianceSettings _settings;
        private readonly IViewRenderService _viewRenderService;

        public RiskController(ComplianceSettings settings, IViewRenderService viewRenderService)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _viewRenderService = viewRenderService ?? throw new ArgumentNullException(nameof(viewRenderService));
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_settings);
        }

        [HttpGet]
        public IActionResult FailDatabaseConnection()
        {
            // For extra security, the connection string is compiled into our source code so noone will ever see it
            const string secretConnectionString = "Server=8.8.8.8;User Id=S4nt4;Password=R00d0lph;";
            throw new GuruMeditationException("Unable to connect to database");
        }

        [HttpGet]
        public IActionResult Secrets()
        {
            return View();
        }

        [HttpGet]
        public IActionResult IframeTest()
        {
            var stream = new MemoryStream();
            var request = HttpContext.Request;
            var framedContent = UriHelper.BuildAbsolute(request.Scheme,
                request.Host,
                request.PathBase,
                request.Path.Value.ToLowerInvariant().Replace(nameof(IframeTest).ToLowerInvariant(), nameof(Iframe)),
                request.QueryString);

            var iframeTestMarkup = _viewRenderService.RenderToString("~/Views/Risk/IframeTest.cshtml", framedContent);

            var bytes = Encoding.UTF8.GetBytes(iframeTestMarkup);
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            HttpContext.Response.Headers[HeaderNames.ContentDisposition] = "attachment;filename=\"iframetest.html\"";
            return new FileStreamResult(stream, MediaTypeNames.Text.Html);
        }

        [HttpGet]
        public IActionResult Iframe()
        {
            return View();
        }
    }
}