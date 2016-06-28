// --------------------------------------------------------------------------
// <copyright file="HtmlHelperExtensions.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Anders.Le" date="2012.8.2">Created</change>
// </history>

namespace Fanex.OddsFeed.Core.Web
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Fanex.Common.Helper;
    using Fanex.OddsFeed.Core.Business;

    ////public static class BundleHelper
    ////{
    ////    public static IHtmlString RenderScript(string path)
    ////    {
    ////        string virtualPath = path.Substring(0, path.Length - 3);
    ////        BundleTable.Bundles.Add(new ScriptBundle(virtualPath).Include(path));

    ////        return Scripts.Render(virtualPath);
    ////    }
    ////}

    /// <summary>
    /// HTML Helpers.
    /// </summary>    
    public static class HtmlHelperExtensions
    {
        #region Truncate
       /// <summary>
       /// Truncates a string if it is longer than a given length.
       /// </summary>
       /// <param name="htmlHelper">The html helper.</param>
       /// <param name="value">The value truncated.</param>
       /// <param name="length">The length to truncate.</param>
       /// <returns>The string after truncated.</returns>
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
            object htmlAttributes = null;
            return helper.RadioButtonList(name, items, htmlAttributes);
        }

        public static IHtmlString RadioButtonList(
            this HtmlHelper helper,
            string name,
            IEnumerable<SelectListItem> items,
            object htmlAttributes)
        {
            return helper.RadioButtonList(name, items, (IDictionary<string, object>)HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// RadioButtons the list.
        /// </summary>
        /// <param name="helper">The helper.</param>
        /// <param name="name">The radio button name.</param>
        /// <param name="items">The items of radio button.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Object IHtmlString.</returns>
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
                string itemId = string.Format("{0}-{1}", name, item.Value);

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
            object htmlAttributes = null;
            return helper.RadioButtonListFor(expression, items, htmlAttributes);
        }

        /// <summary>
        /// RadioButtons the list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="items">The items.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Object IHtmlString.</returns>
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

        /// <summary>
        /// RadioButtons the list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="helper">The helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="items">The items.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Object IHtmlString.</returns>
        public static IHtmlString RadioButtonListFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> items,
            IDictionary<string, object> htmlAttributes)
        {
            Check.Argument.IsNotNull(expression, "expression");

            htmlAttributes = htmlAttributes ?? new Dictionary<string, object>();
            string name = ExpressionHelper.GetExpressionText(expression);
            var rdoList = new StringBuilder();

            rdoList.Append("<div class='rdolist'>");

            foreach (var item in items)
            {
                string itemId = string.Format("{0}-{1}", name, item.Value);

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
        /// <summary>
        /// Converts the value of the current System.DateTime object to its equivalent string
        /// representation using the specified format.
        /// </summary>
        /// <param name="value">The date need to format.</param>
        /// <param name="dateFormat">The date format.</param>
        /// <param name="timeFormat">The time format.</param>
        /// <returns>The full date string.</returns>
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

        /// <summary>
        /// Converts the value of the user level to its equivalent string representation.
        /// </summary>
        /// <param name="value">The level of user.</param>
        /// <returns>Return user level.</returns>
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

        /// <summary>
        /// Decode xml of [f_EncodeXMLValue].
        /// </summary>
        /// <param name="value">The xml value.</param>
        /// <returns>The decode xml.</returns>
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