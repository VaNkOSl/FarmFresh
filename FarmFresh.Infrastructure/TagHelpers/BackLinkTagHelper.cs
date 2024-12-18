using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;

namespace FarmFresh.Infrastructure.TagHelpers;

public class BackLinkTagHelper : AnchorTagHelper
{
    public BackLinkTagHelper(IHtmlGenerator generator) 
        : base(generator)
    {
    }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        base.Process(context, output);
        output.TagName = "a";
        output.AddClass("btn", HtmlEncoder.Default);
        output.AddClass("btn-outline-secondary", HtmlEncoder.Default);
        var referer = ViewContext.HttpContext.Request.Headers["Referer"];
        output.Attributes.SetAttribute("href", referer);
    }
}
