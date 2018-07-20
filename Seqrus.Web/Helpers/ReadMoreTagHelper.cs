using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Seqrus.Web.Helpers
{
    [HtmlTargetElement("read-more")]
    public class ReadMoreTagHelper : TagHelper
    {
        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "a";
            output.PreElement.SetHtmlContent(@"<div class=""seq-info"">");
            output.PostElement.SetHtmlContent(".</div>");
            output.Content.SetContent("Read more");
            output.TagMode = TagMode.StartTagAndEndTag;
            return Task.CompletedTask;
        }
    }
}