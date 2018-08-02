using System.Threading.Tasks;

namespace Seqrus.Web.Services.Rendering
{
    public interface IViewRenderService
    {
        Task<string> RenderToStringAsync(string viewName, object model);
        string RenderToString<TModel>(string viewPath, TModel model);
    }
}