using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using System.Text;

namespace SportsStore.WebUI.HtmlHelpers
{
    public static class PaggingHelpers
    {

        public static MvcHtmlString PageLings(this HtmlHelper html,
                                               PageInfo pageinfo,
                                               Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for(int i=1;i<=pageinfo.TotalPages;i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if(i==pageinfo.Currentpage)
                {
                    tag.AddCssClass("selected");
                    tag.AddCssClass("btn-primary");

                }

                tag.AddCssClass("btn btn-default");
                result.Append(tag.ToString());

                
            }

            return MvcHtmlString.Create(result.ToString());

        }


    }
}