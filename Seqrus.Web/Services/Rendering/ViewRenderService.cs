using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Seqrus.Web.Services.Rendering;

namespace Seqrus.Web.Services
{
    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public ViewRenderService(IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public string RenderToString<TModel>(string viewPath, TModel model)
        {
            var viewEngineResult = _razorViewEngine.GetView("~/", viewPath, false);

            if (!viewEngineResult.Success)
            {
                throw new ViewNotFoundException($"Couldn't find view {viewPath}");
            }

            var view = viewEngineResult.View;

            using (var sw = new StringWriter())
            {
                var viewContext = new ViewContext()
                {
                    HttpContext =
                        new DefaultHttpContext { RequestServices = _serviceProvider },
                    ViewData = new ViewDataDictionary<TModel>(new EmptyModelMetadataProvider(),
                            new ModelStateDictionary())
                        { Model = model },
                    Writer = sw
                };
                view.RenderAsync(viewContext).GetAwaiter().GetResult();
                return sw.ToString();
            }
        }

        public async Task<string> RenderToStringAsync(string viewName, object model)
        {
            var httpContext = new DefaultHttpContext {RequestServices = _serviceProvider};
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                var viewResult = _razorViewEngine.FindView(actionContext, viewName, false);

                if (viewResult.View == null)
                    throw new ViewNotFoundException($"{viewName} does not match any available view");

                var viewDictionary =
                    new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        Model = model
                    };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }
    }
}