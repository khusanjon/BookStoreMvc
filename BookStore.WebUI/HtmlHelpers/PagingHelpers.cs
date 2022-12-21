﻿using BookStore.WebUI.Models;
using System;
using System.Text;
using System.Web.Mvc;

namespace BookStore.WebUI.HtmlHelpers
{
    public static class PagingHelpers
    {
        // метод PageLinks() генерирует HTML-разметку для набора ссылок на страницы с использованием информации,
        // предоставленной в объекте PagingInfo. Параметр Func принимает делегат, который применяется для генерации ссылок,
        // обеспечивающих просмотр других страниц.
        public static MvcHtmlString PageLinks(this HtmlHelper html,
                                              PagingInfo pagingInfo,
                                              Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.TotalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.CurrentPage)
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