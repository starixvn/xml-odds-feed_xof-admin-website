// --------------------------------------------------------------------------
// <copyright file="HtmlHelperExtensions.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Thai.Tran" date="2013.1.7">Created</change>
// </history>

namespace Fanex.OddsFeed.Framework.Mvc
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq.Expressions;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using Fanex.Common.Helper;

    [SuppressMessage("StyleCop.CSharp.DocumentationRules",
        "SA1632:DocumentationTextMustMeetMinimumCharacterLength",
        Justification = "Reviewed.")]
    [SuppressMessage(
         "StyleCop.CSharp.StylecopPlus.Rules",
         "SP2101:MethodMustNotContainMoreLinesThan",
         Justification = "Reviewed. Suppression is OK here.")]

    /// <summary>
    /// The declare of class HtmlHelperExtensions.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        #region Render Datetime Extension        
        private static readonly DateTimeFormatInfo _DateTimeFormat = 
            DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture);

        /// <summary>
        /// Dates the picker.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="date">The date.</param>
        /// <param name="minDate">The min date.</param>
        /// <param name="maxDate">The max date, default is now.</param>
        /// <param name="formatDate">The format date.</param>
        /// <param name="clientFormateDate">Format date time of client side.</param>
        /// <param name="cssDatePicker">The CSS date picker.</param>
        /// <returns>Return the date picker.</returns>
        public static MvcHtmlString DatePicker(
            this HtmlHelper htmlHelper, 
            string name, 
            DateTime? date,
            DateTime? minDate = null, 
            DateTime? maxDate = null, 
            string formatDate = "MMM dd, yyyy",
            string clientFormateDate = "yyyy/MM/dd", 
            string cssDatePicker = "datepicker")
        {
            Check.Argument.IsNotNull(formatDate, "formateDate");
            Check.Argument.IsNotNull(clientFormateDate, "clientFormateDate");

            var textBoxMaxDate = maxDate;

            if (!textBoxMaxDate.HasValue)
            {
                textBoxMaxDate = DateTime.Now;
            }

            var textbox = htmlHelper.TextBox(
                name,
                date.HasValue ? date.Value.ToString(formatDate) : string.Empty,
                new
                {
                    @class = cssDatePicker,
                    mindate = minDate != null && minDate != DateTime.MinValue
                    ? ((DateTime)minDate).ToString(clientFormateDate)
                    : "null",
                    maxdate = ((DateTime)textBoxMaxDate).ToString(clientFormateDate)
                });

            return MvcHtmlString.Create(textbox.ToString());
        }

        public static MvcHtmlString DatePickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html, 
            Expression<Func<TModel, TValue>> expression, 
            DateTime? minDate = null, 
            DateTime? maxDate = null, 
            string formatDate = "MMM dd, yyyy")
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            DateTime? value = null;

            if (metaData.Model is DateTime?)
            {
                value = (DateTime?)metaData.Model;
            }

            return html.DatePicker(metaData.PropertyName, value, minDate, maxDate, formatDate);
        }

        /// <summary>
        /// Years the picker.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="currentYear">The current year.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Return the date picker.</returns>
        public static MvcHtmlString YearPicker(
            this HtmlHelper htmlHelper,
            string name,
            int fromYear = 2006,
            int currentYear = 2000,
            object htmlAttributes = null)
        {
            var list = new List<SelectListItem>();
            var now = DateTime.Now.Year;

            for (var i = now; i >= fromYear; i--)
            {
                list.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = i == currentYear
                });
            }

            var dropDownList = htmlHelper.DropDownList(name, list, htmlAttributes);

            return MvcHtmlString.Create(dropDownList.ToString());
        }

        /// <summary>
        /// Years the picker for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Return the date picker.</returns>
        public static MvcHtmlString YearPickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            int fromYear = 2006,
            object htmlAttributes = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var currentYear = (int)metaData.Model;

            return html.YearPicker(metaData.PropertyName, fromYear, currentYear, htmlAttributes);
        }

        /// <summary>
        /// Month picker.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name.</param>
        /// <param name="currentMonth">The current month.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>MVC HtmlString.</returns>
        public static MvcHtmlString MonthPicker(this HtmlHelper htmlHelper, string name, int currentMonth, object htmlAttributes = null)
        {
            ////currentYear = currentYear ?? DateTime.Now.Year;
            var list = new List<SelectListItem>();
            const byte endMonth = 12; ////(currentYear.Value < DateTime.Now.Year) ? 12 : DateTime.Now.Month;
            
            for (var i = endMonth; i >= 1; i--)
            {
                list.Add(new SelectListItem
                {
                    Text = _DateTimeFormat.GetMonthName(i),
                    Value = i.ToString(),
                    Selected = i == currentMonth
                });
            }

            var dropDownList = htmlHelper.DropDownList(name, list, htmlAttributes);

            return MvcHtmlString.Create(dropDownList.ToString());
        }
        #endregion
    }
}
