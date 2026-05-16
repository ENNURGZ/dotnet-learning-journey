using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using SportsStore.Models.ViewModels;

namespace SportsStore.Infrastructure;

[HtmlTargetElement("div", Attributes = "page-model")]
// NEW: Add PageLinkTagHelper for pagination links
public class PageLinkTagHelper(IUrlHelperFactory urlHelperFactory) : TagHelper
{
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext? ViewContext { get; set; }
    
    public PagingInfo? PageModel { get; set; }
    public string? PageAction { get; set; }
    
    // NEW: Add PageRoute property for custom routing
    public string? PageRoute { get; set; }
    
    // NEW: Add HtmlAttributeName for tag helper attributes
    [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
    // NEW: Add PageUrlValues dictionary for dynamic URL parameters
    public Dictionary<string, object> PageUrlValues { get; } = new Dictionary<string, object>();

    public bool PageClassesEnabled { get; set; }
    public string PageClass { get; set; } = string.Empty;
    public string PageClassNormal { get; set; } = string.Empty;
    public string PageClassSelected { get; set; } = string.Empty;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (ViewContext != null && PageModel != null)
        {
            IUrlHelper urlHelper = urlHelperFactory.GetUrlHelper(ViewContext);
            TagBuilder result = new TagBuilder("div");
            for (int i = 1; i <= PageModel.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                
                // NEW: Set productPage parameter in dictionary
                PageUrlValues["productPage"] = i;
                
                // NEW: Generate URL using Action or Route
                if (!string.IsNullOrEmpty(PageRoute))
                {
                    tag.Attributes["href"] = urlHelper.RouteUrl(PageRoute, PageUrlValues);
                }
                else
                {
                    tag.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
                }
                
                if (PageClassesEnabled)
                {
                    tag.AddCssClass(PageClass);
                    tag.AddCssClass(i == PageModel.CurrentPage
                        ? PageClassSelected : PageClassNormal);
                }

                tag.InnerHtml.Append(i.ToString());
                result.InnerHtml.AppendHtml(tag);
            }
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
