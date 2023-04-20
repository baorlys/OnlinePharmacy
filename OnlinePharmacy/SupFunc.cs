using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Text.RegularExpressions;

namespace OnlinePharmacy
{
    public static class SupFunc
    {
        public static string RemoveUnicode(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "â", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ă", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "đ",
                "é","è","ẻ","ẽ","ẹ","ê","ế","ề","ể","ễ","ệ",
                "í","ì","ỉ","ĩ","ị",
                "ó","ò","ỏ","õ","ọ","ô","ố","ồ","ổ","ỗ","ộ","ơ","ớ","ờ","ở","ỡ","ợ",
                "ú","ù","ủ","ũ","ụ","ư","ứ","ừ","ử","ữ","ự",
                "ý","ỳ","ỷ","ỹ","ỵ",};
            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a", "a",
                "d",
                "e","e","e","e","e","e","e","e","e","e","e",
                "i","i","i","i","i",
                "o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o","o",
                "u","u","u","u","u","u","u","u","u","u","u",
                "y","y","y","y","y",};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
                text = text.Replace(arr1[i].ToUpper(), arr2[i].ToUpper());
            }
            return text;
        }

        public static string ConvertToMeta(string text)
        {
            string result = RemoveUnicode(text);
            result = Regex.Replace(result, @"[^0-9a-zA-Z]+", "-").ToLower();
            if (result[result.Length - 1] == '-') result = result.Substring(0, result.Length - 1);
            return result;
        }

        public static string FirstCharToUpper(string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => input[0].ToString().ToUpper() + input.Substring(1)
        };

        public static IHtmlContent NoEncodeActionLink(
    this IHtmlHelper htmlHelper,
    string linkText,
    string actionName,
    string controllerName,
    string areaName = null,
    object routeValues = null,
    object htmlAttributes = null)
        {
            var tag = new TagBuilder("a");
            tag.InnerHtml.AppendHtml(linkText);
            var urlHelperFactory = htmlHelper.ViewContext.HttpContext.RequestServices.GetService<IUrlHelperFactory>();
            var urlHelper = urlHelperFactory.GetUrlHelper(htmlHelper.ViewContext);
            tag.Attributes.Add("href", urlHelper.Action(actionName, controllerName, routeValues, htmlHelper.ViewContext.HttpContext.Request.Scheme, htmlHelper.ViewContext.HttpContext.Request.Host.Value, areaName));
            tag.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            return tag;
        }




    }
}
