using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Seqrus.Web.Helpers
{
    [HtmlTargetElement("risk-exposure", TagStructure = TagStructure.WithoutEndTag)]
    public class RiskExposureTagHelper : TagHelper
    {
        public bool? IsExposed { get; set; }

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            output.Attributes.SetAttribute("class", $"label label-{GetLabelType(IsExposed)}");
            output.Content.SetContent(GetContent(IsExposed));
            output.TagMode = TagMode.StartTagAndEndTag;
            return Task.CompletedTask;
        }

        private static string GetLabelType(bool? isExposed)
        {
            switch (isExposed)
            {
                case null:
                    return "default";
                case true:
                    return "danger";
                default:
                    return "success";
            }
        }

        private static string GetContent(bool? isExposed)
        {
            switch (isExposed)
            {
                case null:
                    return "Not implemented";
                case true:
                    return "Exposed";
                default:
                    return "Not exposed";
            }
        }
    }
}