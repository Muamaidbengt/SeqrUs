using System.IO;
using System.Net.Mime;
using System.Text;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Seqrus.Web.Services.Rendering;
using Seqrus.Web.ViewModels;

namespace Seqrus.Web.Controllers
{
    public class StoreController : Controller
    {
        private readonly IViewRenderService _viewRenderService;

        public StoreController(IViewRenderService viewRenderService)
        {
            _viewRenderService = viewRenderService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var cart = new SprocketCartModel();
            return View(cart);
        }

        [HttpPost]
        public IActionResult Purchase(SprocketCartModel cart)
        {
            if (!ModelState.IsValid)
                return View(nameof(Index), cart);

            var order = SprocketOrderModel.FromCart(cart);
            return View(order);
        }

        [HttpGet]
        public IActionResult CrossSitePostTest()
        {
            var stream = new MemoryStream();
            var request = HttpContext.Request;
            var framedContent = UriHelper.BuildAbsolute(request.Scheme,
                request.Host,
                request.PathBase,
                request.Path.Value.ToLowerInvariant().Replace(nameof(CrossSitePostTest).ToLowerInvariant(), nameof(Purchase)),
                request.QueryString);

            var iframeTestMarkup = _viewRenderService.RenderToString("~/Views/Store/CrossSitePostTest.cshtml", framedContent);

            var bytes = Encoding.UTF8.GetBytes(iframeTestMarkup);
            stream.Write(bytes, 0, bytes.Length);
            stream.Seek(0, SeekOrigin.Begin);
            HttpContext.Response.Headers[HeaderNames.ContentDisposition] = "attachment;filename=\"crossSitePostTest.html\"";
            return new FileStreamResult(stream, MediaTypeNames.Text.Html);
        }
    }
}
