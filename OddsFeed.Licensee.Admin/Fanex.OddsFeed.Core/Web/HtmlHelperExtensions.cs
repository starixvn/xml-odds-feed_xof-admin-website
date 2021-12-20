
namespace Fanex.OddsFeed.Core.Web
{
    using System;
    using System.Collections.Generic;

    using System.Linq.Expressions;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Fanex.Common.Helper;
    using Fanex.OddsFeed.Core.Business;

    public static class HtmlHelperExtensions
    {
        #region Truncate
        public static IHtmlString Truncate(this HtmlHelper htmlHelper, string value, int length)
        {
            var builder = new TagBuilder("a");

            builder.MergeAttribute("href", "javascript:alert(this.title)");
            builder.MergeAttribute("title", value);
            builder.InnerHtml = "[...]";
            value = value.Length <= length
                ? value
                : string.Format("{0} {1}", value.Substring(0, length), builder.ToString());

            return new HtmlString(value);
        }
        #endregion

        #region Radio Button List

        public static IHtmlString RadioButtonList(
            this HtmlHelper helper,
            string name,
            IEnumerable<SelectListItem> items)
        {
            return helper.RadioButtonList(name, items, null);
        }

        public static IHtmlString RadioButtonList(
            this HtmlHelper helper,
            string name,
            IEnumerable<SelectListItem> items,
            object htmlAttributes)
        {
            return helper.RadioButtonList(name, items, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static IHtmlString RadioButtonList(
            this HtmlHelper helper,
            string name,
            IEnumerable<SelectListItem> items,
            IDictionary<string, object> htmlAttributes)
        {
            Check.Argument.IsNotEmpty(name, "name");

            var rdoList = new StringBuilder();
            htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();

            rdoList.Append("<div class='rdolist'>");

            foreach (var item in items)
            {
                var itemId = string.Format("{0}-{1}", name, item.Value);

                htmlAttributes.Remove("id");
                htmlAttributes.Add("id", itemId);

                rdoList.Append(helper.RadioButton(name, item.Value, item.Selected, htmlAttributes));
                rdoList.Append(helper.Label(itemId, item.Text));
            }

            rdoList.Append("</div>");

            return new HtmlString(rdoList.ToString());
        }

        public static IHtmlString RadioButtonListFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> items)
        {
            return helper.RadioButtonListFor(expression, items, null);
        }

        public static IHtmlString RadioButtonListFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> items,
            object htmlAttributes)
        {
            return helper.RadioButtonListFor(
                expression,
                items,
                (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        public static IHtmlString RadioButtonListFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> items,
            IDictionary<string, object> htmlAttributes)
        {
            Check.Argument.IsNotNull(expression, "expression");

            htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();
            var name = ExpressionHelper.GetExpressionText(expression);
            var rdoList = new StringBuilder();

            rdoList.Append("<div class='rdolist'>");

            foreach (var item in items)
            {
                var itemId = string.Format("{0}-{1}", name, item.Value);

                htmlAttributes.Remove("id");
                htmlAttributes.Add("id", itemId);

                rdoList.Append(helper.RadioButtonFor(expression, item.Value, htmlAttributes));
                rdoList.Append(helper.Label(itemId, item.Text));
            }

            rdoList.Append("</div>");

            return new HtmlString(rdoList.ToString());
        }

        #endregion

        #region DateTime
        public static IHtmlString ToFullDateString(
            this DateTime value,
            string dateFormat = "MMM dd, yyyy",
            string timeFormat = "HH:mm tt")
        {
            return new HtmlString(
                string.Format(
                    "<span class='datetime'><span class='date'>{0}</span> <span class='time'>{1}</span></span>",
                    value.ToString(dateFormat),
                    value.ToString(timeFormat)));
        }
        #endregion

        public static IHtmlString ToUserLevel(this int value)
        {
            string level;

            switch (value)
            {
                case (int)Constant.Level.PortalAdmin:
                    level = ViewResources.Portal_Admin;
                    break;
                case (int)Constant.Level.SiteAdmin:
                    level = ViewResources.Site_Admin;
                    break;
                case (int)Constant.Level.User:
                    level = ViewResources.User;
                    break;
                default:
                    level = string.Empty;
                    break;
            }

            return new HtmlString(level);
        }

        public static string DecodeXMLValue(this string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Replace("&amp;", "&");
                value = value.Replace("&lt;", "<");
                value = value.Replace("&gt;", ">");
                value = value.Replace("&quot;", "\"");
                value = value.Replace("&apos;", "''");

                return value;
            }

            return value;
        }
    }
}