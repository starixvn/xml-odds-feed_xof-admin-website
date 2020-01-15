// --------------------------------------------------------------------------
// <copyright file="Extensions.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="thai.tran" date="09-18-2013">Created</change>
// </history>
namespace Fanex.OddsFeed.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Mvc.Html;
    using System.Web.Routing;
    using System.Web.WebPages;

    /// <summary>
    /// Class Extensions.
    /// </summary>
    ////[SuppressMessage(
    ////     "StyleCop.CSharp.StylecopPlus.Rules",
    ////     "SP2101:MethodMustNotContainMoreLinesThan",
    ////     Justification = "Reviewed. Suppression is OK here.")]
    public static class Extensions
    {
        /// <summary>
        /// The single empty item.
        /// </summary>
        private static readonly SelectListItem[] SingleEmptyItem = new[] { new SelectListItem { Text = string.Empty, Value = string.Empty } };

        private static readonly object _O = new object();

        /// <summary>
        /// DateTime Format Info.
        /// </summary>
        private static readonly DateTimeFormatInfo DateTimeFormat = DateTimeFormatInfo.GetInstance(CultureInfo.CurrentCulture);

        /// <summary>
        /// Pagers the specified HTML helper.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRow">The total row.</param>
        /// <param name="url">The URL link.</param>
        /// <returns>Object System.String.</returns>
        public static string Pager(this HtmlHelper htmlHelper, int pageNumber, int pageSize, int totalRow, string url)
        {
            var totalPage = (int)Math.Ceiling(totalRow / (double)pageSize);

            var predot = pageNumber > 5;
            var nextdot = totalPage - pageNumber > 4;

            if (totalPage < 2)
            {
                return string.Empty;
            }

            var pager = new StringBuilder();
            if (!predot)
            {
                for (var i = 1; i < pageNumber; i++)
                {
                    pager.Append(CreatePageNode(url, i));
                }
            }
            else
            {
                pager.Append(CreatePageNode(url, 1));
                pager.Append(CreatePageNode(url, 2));
                pager.Append("<li>...</li>");
                pager.Append(CreatePageNode(url, pageNumber - 2));
                pager.Append(CreatePageNode(url, pageNumber - 1));
            }

            pager.Append(CreatePageNode(url, pageNumber, true));
            if (!nextdot)
            {
                for (var i = pageNumber + 1; i <= totalPage; i++)
                {
                    pager.Append(CreatePageNode(url, i));
                }
            }
            else
            {
                pager.Append(CreatePageNode(url, pageNumber + 1));
                pager.Append(CreatePageNode(url, pageNumber + 2));
                pager.Append("<li>...</li>");
                pager.Append(CreatePageNode(url, totalPage - 1));
                pager.Append(CreatePageNode(url, totalPage));
            }

            return pager.ToString();
        }

        /// <summary>
        /// RadioButtons the list.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="listOfValues">The list of values.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString RadioButtonList<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> listOfValues)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var sb = new StringBuilder();

            if (listOfValues != null)
            {
                // Create a radio button for each item in the list
                foreach (SelectListItem item in listOfValues)
                {
                    // Generate an id to be given to the radio button field
                    var id = string.Format("{0}_{1}", metaData.PropertyName, item.Value);

                    // Create and populate a radio button using the existing html helpers
                    var label = htmlHelper.Label(id, HttpUtility.HtmlEncode(item.Text));
                    var radio = htmlHelper.RadioButtonFor(expression, item.Value, new { id = id }).ToHtmlString();

                    // Create the html string that will be returned to the client
                    // e.g. <input data-val="true" data-val-required="You must select an option" id="TestRadio_1" name="TestRadio" type="radio" value="1" />
                    // <label for="TestRadio_1">Line1</label>
                    sb.AppendFormat("<div class=\"radioButton\">{0}{1}</div>", radio, label);
                }
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        public static MvcForm BeginGetForm(this HtmlHelper htmlHelper)
        {
            return htmlHelper.BeginForm(null, null, FormMethod.Get);
        }

        public static MvcHtmlString DatePicker(
            this HtmlHelper htmlHelper,
            string name,
            DateTime? date,
            DateTime? minDate = null,
            DateTime? maxDate = null,
            string formatDate = "dd MMM, yyyy",
            IEnumerable<DateTime> unavailDates = null)
        {
            const string formatDateForTextBox = "yyyy/MM/dd";

            if (string.IsNullOrWhiteSpace(formatDate))
            {
                throw new ArgumentNullException(nameof(formatDate));
            }

            var disabledDate = (unavailDates != null && unavailDates.Any())
                ? string.Join(",", unavailDates.Select(d => d.ToString("dd MMM yyyy")))
                : string.Empty;

            string formatedMinDate = minDate != null && minDate != DateTime.MinValue ? ((DateTime)minDate).ToString(formatDateForTextBox) : "null";
            string formatedMaxDate = minDate != null && minDate != DateTime.MinValue ? ((DateTime)minDate).ToString(formatDateForTextBox) : "null";

            var textbox = htmlHelper.TextBox(
                name,
                date.HasValue ? date.Value.ToString(formatDate) : string.Empty,
                new
                {
                    @class = "datepicker",
                    mindate = formatedMinDate,
                    maxdate = formatedMaxDate,
                    disabledDate
                });

            return MvcHtmlString.Create(textbox.ToString());
        }

        /// <summary>
        /// Dates the picker for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="html">The HTML content.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="minDate">The minimum date.</param>
        /// <param name="maxDate">The maximum date.</param>
        /// <param name="formatDate">The format date.</param>
        /// <param name="unavailDates">The unavailable dates.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString DatePickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            DateTime? minDate = null,
            DateTime? maxDate = null,
            string formatDate = "dd MMM, yyyy",
             IEnumerable<DateTime> unavailDates = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            DateTime? value = DateTime.Now;

            if (metaData.Model is DateTime? || metaData.Model is DateTime)
            {
                value = (DateTime?)metaData.Model;
            }
            else
            {
                value = null;
            }

            return html.DatePicker(metaData.PropertyName, value, minDate, maxDate, formatDate, unavailDates);
        }

        /// <summary>
        /// Weeks the picker.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name for generate.</param>
        /// <param name="date">The date for generate.</param>
        /// <param name="minDate">The minimum date.</param>
        /// <param name="maxDate">The maximum date.</param>
        /// <param name="formatDate">The format date.</param>
        /// <returns>Object MVCHtmlString.</returns>
        /// <exception cref="System.ArgumentNullException">FormatDate cannot be null.</exception>
        public static MvcHtmlString WeekPicker(
            this HtmlHelper htmlHelper,
            string name,
            DateTime? date,
            DateTime? minDate = null,
            DateTime? maxDate = null,
            string formatDate = "MMM dd, yyyy")
        {
            if (string.IsNullOrWhiteSpace(formatDate))
            {
                throw new ArgumentNullException(nameof(formatDate));
            }

            var mindate = minDate != null && minDate != DateTime.MinValue ? ((DateTime)minDate).ToString("yyyy/MM/dd") : "null";
            var maxdate = maxDate != null && maxDate != DateTime.MaxValue ? ((DateTime)maxDate).ToString("yyyy/MM/dd") : "null";
            var div = string.Format(
                @"<input readonly='true' type='text' id='{0}' name='{0}'"
                + " data-value='{1}' data-mindate='{2}' data-maxdate='{3}' class='weekpicker lfloat' />",
                name,
                date,
                mindate,
                maxdate);

            return MvcHtmlString.Create(div);
        }

        /// <summary>
        /// Weeks the picker for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="html">The HTML for generate.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="value">The value for generate.</param>
        /// <param name="minDate">The minimum date.</param>
        /// <param name="maxDate">The maximum date.</param>
        /// <param name="formatDate">The format date.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString WeekPickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            DateTime? value = null,
            DateTime? minDate = null,
            DateTime? maxDate = null,
            string formatDate = "MMM dd, yyyy")
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            value = (value ?? maxDate) ?? DateTime.Now;

            return html.WeekPicker(metaData.PropertyName, value, minDate, maxDate, formatDate);
        }

        /// <summary>
        /// Month the picker.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name for generate.</param>
        /// <param name="currentMonth">The current month.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString MonthPicker(
            this HtmlHelper htmlHelper,
            string name,
            int currentMonth,
            object htmlAttributes = null)
        {
            const int toMonth = 12;

            var list = new List<SelectListItem>();
            for (var i = 1; i <= toMonth; i++)
            {
                list.Add(new SelectListItem
                {
                    Text = DateTimeFormat.GetMonthName(i),
                    Value = i.ToString(),
                    Selected = i == currentMonth
                });
            }

            var dropDownList = htmlHelper.DropDownList(name, list, htmlAttributes);
            return MvcHtmlString.Create(dropDownList.ToString());
        }

        /// <summary>
        /// Month the picker for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="html">The HTML for generate.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="currentMonth">The current month.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString MonthPickerFor<TModel, TValue>(
            this HtmlHelper<TModel> html,
            Expression<Func<TModel, TValue>> expression,
            int currentMonth,
            ////int? currentYear,
            ////DateTime? minDate = null,
            ////DateTime? maxDate = null,
            object htmlAttributes = null)
        {
            var metaData = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            currentMonth = (int)metaData.Model;

            return html.MonthPicker(metaData.PropertyName, currentMonth, htmlAttributes);
        }

        /// <summary>
        /// Years the picker.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name for generate.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="currentYear">The current year.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString YearPicker(
            this HtmlHelper htmlHelper,
            string name,
            int fromYear = 2006,
            int currentYear = 2000,
            object htmlAttributes = null)
        {
            var list = new List<SelectListItem>();
            var toYear = DateTime.Now.Year;

            for (var i = fromYear; i <= toYear; i++)
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
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TValue">The type of the t value.</typeparam>
        /// <param name="html">The HTML for generate.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="fromYear">From year.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Object MVCHtmlString.</returns>
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
        /// Gets the enumerate description.
        /// </summary>
        /// <typeparam name="TEnum">The type of the t enumerate.</typeparam>
        /// <param name="value">The value.</param>
        /// <returns>Object System.String.</returns>
        public static string GetEnumDescription<TEnum>(TEnum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return (attributes != null) && (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// Enumerates the drop down list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TEnum">The type of the t enumerate.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="excludeValues">The exclude values.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TEnum>> expression,
            object htmlAttributes,
            string excludeValues = "")
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var enumType = GetNonNullableModelType(metadata);
            var values = Enum.GetValues(enumType).Cast<TEnum>();
            var items =
                from value in values
                where !excludeValues.ContainsString(GetEnumDescription(value))
                select new SelectListItem
                {
                    Text = GetEnumDescription(value),
                    Value = value.ToString(),
                    Selected = value.Equals(metadata.Model)
                };

            if (metadata.IsNullableValueType)
            {
                items = SingleEmptyItem.Concat(items);
            }

            return htmlHelper.DropDownListFor(
                expression,
                items,
                htmlAttributes);
        }

        /// <summary>
        /// Determines whether the specified string list contains string.
        /// </summary>
        /// <param name="stringList">The string list.</param>
        /// <param name="search">The search.</param>
        /// <returns><c>true</c> if the specified string list contains string; otherwise, <c>false</c>.</returns>
        public static bool ContainsString(this string stringList, string search)
        {
            var arr = stringList.Split(new char[] { ',' });
            return arr.Any(a => string.Compare(a.Replace("\\", ","), search, true) == 0);
        }

        /// <summary>
        /// Determines whether the specified string contains strings.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="stringList">The string list.</param>
        /// <returns><c>true</c> if the specified string contains strings; otherwise, <c>false</c>.</returns>
        public static bool ContainsStrings(this string str, string stringList)
        {
            var arr = stringList.Split(new char[] { ',' });
            return arr.Any(a => str.Contains(a));
        }

        /// <summary>
        /// Renders the section.
        /// </summary>
        /// <param name="page">The page for  render.</param>
        /// <param name="sectionName">Name of the section.</param>
        /// <param name="defaultContent">The default content.</param>
        /// <returns>Object HelperResult.</returns>
        public static HelperResult RenderSection(
            this WebPageBase page,
            string sectionName,
            Func<object, HelperResult> defaultContent)
        {
            return page.IsSectionDefined(sectionName)
                ? page.RenderSection(sectionName)
                : defaultContent(_O);
        }

        #region CheckBoxList

        /// <summary>
        /// CheckBoxes the list.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name for generate.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name)
        {
            return CheckBoxList(htmlHelper, name, null);
        }

        /// <summary>
        /// CheckBoxes the list.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name for generate.</param>
        /// <param name="selectList">The select list.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString CheckBoxList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxList(htmlHelper, name, selectList, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// CheckBoxes the list.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name for generate.</param>
        /// <param name="selectList">The select list.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString CheckBoxList(
            this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> selectList,
            object htmlAttributes)
        {
            return CheckBoxList(htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }

        /// <summary>
        /// CheckBoxes the list.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name for generate.</param>
        /// <param name="selectList">The select list.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="showNameAttribute">If set to <c>true</c> [show name attribute].</param>
        /// <returns>Object MVCHtmlString.</returns>
        /// <exception cref="System.ArgumentException">Name cannot be null or empty.;name.</exception>
        /// <exception cref="System.ArgumentNullException">Object selectList.</exception>
        public static MvcHtmlString CheckBoxList(
            this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> selectList,
            IDictionary<string, object> htmlAttributes,
            bool showNameAttribute = true)
        {
            var fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullHtmlFieldName))
            {
                throw new ArgumentException("name cannot be null or empty.", nameof(name));
            }

            var selectListFromViewData = false;
            if (selectList == null)
            {
                selectList = htmlHelper.ViewData.Eval(fullHtmlFieldName) as IEnumerable<SelectListItem>;
                selectListFromViewData = true;
            }

            if (selectList == null)
            {
                throw new ArgumentNullException(nameof(selectList));
            }

            object values = null;
            ModelState state;

            if (htmlHelper.ViewData.ModelState.TryGetValue(fullHtmlFieldName, out state)
               && (state.Value != null))
            {
                values = state.Value.ConvertTo(typeof(string[]), null);
            }

            if (!selectListFromViewData && (values == null))
            {
                values = htmlHelper.ViewData.Eval(fullHtmlFieldName);
            }

            if (values != null)
            {
                selectList = CreateListItems(selectList, values);
            }

            var sb = new StringBuilder();
            htmlAttributes = (htmlAttributes == null) ? new RouteValueDictionary() : new RouteValueDictionary(htmlAttributes);

            htmlAttributes.Remove("checked");

            foreach (var item in selectList)
            {
                CreateCheckBox(name, htmlAttributes, showNameAttribute, fullHtmlFieldName, sb, item);
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// CheckBoxes the list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            return CheckBoxListFor(htmlHelper, expression, null);
        }

        /// <summary>
        /// CheckBoxes the list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="selectList">The select list.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxListFor(htmlHelper, expression, selectList, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// CheckBoxes the list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="selectList">The select list.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="showNameAttribute">If set to <c>true</c> [show name attribute].</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            object htmlAttributes,
            bool showNameAttribute = true)
        {
            return CheckBoxListFor(htmlHelper, expression, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), showNameAttribute);
        }

        /// <summary>
        /// CheckBoxes the list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="selectList">The select list.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="showNameAttribute">If set to <c>true</c> [show name attribute].</param>
        /// <returns>Object MVCHtmlString.</returns>
        /// <exception cref="System.ArgumentNullException">Object Expression.</exception>
        public static MvcHtmlString CheckBoxListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            IDictionary<string, object> htmlAttributes,
            bool showNameAttribute = true)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return CheckBoxList(htmlHelper, ExpressionHelper.GetExpressionText(expression), selectList, htmlAttributes, showNameAttribute);
        }

        #endregion CheckBoxList

        #region CheckBoxDropDownList

        /// <summary>
        /// CheckBoxes the drop down list.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name for generate.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString CheckBoxDropDownList(this HtmlHelper htmlHelper, string name)
        {
            return CheckBoxDropDownList(htmlHelper, name, null);
        }

        /// <summary>
        /// CheckBoxes the drop down list.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name for generate.</param>
        /// <param name="selectList">The select list.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString CheckBoxDropDownList(this HtmlHelper htmlHelper, string name, IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxDropDownList(htmlHelper, name, selectList, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// CheckBoxes the drop down list.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name for generate.</param>
        /// <param name="selectList">The select list.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="showNameAttribute">If set to <c>true</c> [show name attribute].</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString CheckBoxDropDownList(
            this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> selectList,
            object htmlAttributes,
            bool showNameAttribute = true)
        {
            return CheckBoxDropDownList(htmlHelper, name, selectList, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes), showNameAttribute);
        }

        /// <summary>
        /// CheckBoxes the drop down list.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="name">The name for generate.</param>
        /// <param name="selectList">The select list.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="showNameAttribute">If set to <c>true</c> [show name attribute].</param>
        /// <returns>Object MVCHtmlString.</returns>
        /// <exception cref="System.ArgumentException">Name cannot be null or empty.;name.</exception>
        /// <exception cref="System.ArgumentNullException">Exception name selectList.</exception>
        public static MvcHtmlString CheckBoxDropDownList(
            this HtmlHelper htmlHelper,
            string name,
            IEnumerable<SelectListItem> selectList,
            IDictionary<string, object> htmlAttributes,
            bool showNameAttribute = true)
        {
            var fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullHtmlFieldName))
            {
                throw new ArgumentException("name cannot be null or empty.", nameof(name));
            }

            var selectListFromViewData = false;
            if (selectList == null)
            {
                selectList = htmlHelper.ViewData.Eval(fullHtmlFieldName) as IEnumerable<SelectListItem>;
                selectListFromViewData = true;
            }

            if (selectList == null)
            {
                throw new ArgumentNullException(nameof(selectList));
            }

            object values = null;
            ModelState state;
            if (htmlHelper.ViewData.ModelState.TryGetValue(fullHtmlFieldName, out state) && (state.Value != null))
            {
                var stateValue = state.Value.ConvertTo(typeof(string)).ToString();
                values = stateValue.Contains(",") ? stateValue.Split(',') : state.Value.ConvertTo(typeof(string[]), null);
            }

            if (!selectListFromViewData && (values == null))
            {
                values = htmlHelper.ViewData.Eval(fullHtmlFieldName);
            }

            if (values != null)
            {
                selectList = CreateListItems(selectList, values);
            }

            var sb = new StringBuilder();
            htmlAttributes = (htmlAttributes == null) ? new RouteValueDictionary() : new RouteValueDictionary(htmlAttributes);
            htmlAttributes.Remove("checked");

            foreach (var item in selectList)
            {
                CreateCheckBoxInsideDiv(name, htmlAttributes, showNameAttribute, fullHtmlFieldName, sb, item);
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        /// <summary>
        /// CheckBoxes the drop down list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString CheckBoxDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            return CheckBoxDropDownListFor(htmlHelper, expression, null);
        }

        /// <summary>
        /// CheckBoxes the drop down list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="selectList">The select list.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString CheckBoxDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList)
        {
            return CheckBoxDropDownListFor(htmlHelper, expression, selectList, (IDictionary<string, object>)null);
        }

        /// <summary>
        /// CheckBoxes the drop down list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="selectList">The select list.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="showNameAttribute">If set to <c>true</c> [show name attribute].</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString CheckBoxDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            object htmlAttributes,
            bool showNameAttribute = true)
        {
            return CheckBoxDropDownListFor(
                htmlHelper,
                expression,
                selectList,
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes),
                showNameAttribute);
        }

        /// <summary>
        /// CheckBoxes the drop down list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="selectList">The select list.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="showNameAttribute">If set to <c>true</c> [show name attribute].</param>
        /// <returns>Object MVCHtmlString.</returns>
        /// <exception cref="System.ArgumentNullException">Object Expression.</exception>
        public static MvcHtmlString CheckBoxDropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            IDictionary<string, object> htmlAttributes,
            bool showNameAttribute = true)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            return CheckBoxDropDownList(htmlHelper, ExpressionHelper.GetExpressionText(expression), selectList, htmlAttributes, showNameAttribute);
        }

        #endregion CheckBoxDropDownList

        #region Business

        public static string ToFormatedDate(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToFormatedDate();
            }

            return string.Empty;
        }

        public static string ToFormatedDate(this DateTime date)
        {
            return date.ToString("dd MMM, yyyy hh:mm tt");
        }

        public static string ToShortFormatedDate(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToShortFormatedDate();
            }

            return string.Empty;
        }

        public static string ToShortFormatedDate(this DateTime date)
        {
            var dateStr = date.ToString(@"dd MMM, yyyy - HH") + "h" + date.ToString(@"mm\'");
            return dateStr;
        }

        public static string ToMonthAndDayFormatedDate(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToMonthAndDayFormatedDate();
            }

            return string.Empty;
        }

        public static string ToMonthAndDayFormatedDate(this DateTime date)
        {
            return date.ToString("dd-MMM");
        }

        public static string ToMonthAndYearFormatedDate(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToMonthAndYearFormatedDate();
            }

            return string.Empty;
        }

        public static string ToMonthAndYearFormatedDate(this DateTime date)
        {
            return date.ToString("MMM-yyyy");
        }

        public static string ToDateMonthYearFormatedDate(this DateTime date)
        {
            return date.ToString("dd MMM, yyyy");
        }

        public static string ToDateMonthYearFormatedDate(this DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToDateMonthYearFormatedDate();
            }

            return string.Empty;
        }

        /// <summary>
        /// Drops down list for.
        /// </summary>
        /// <typeparam name="TModel">The type of the t model.</typeparam>
        /// <typeparam name="TProperty">The type of the t property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="selectList">The select list.</param>
        /// <param name="firstItem">The first item.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>Object MVCHtmlString.</returns>
        public static MvcHtmlString DropDownListFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> selectList,
            SelectListItem firstItem,
            object htmlAttributes)
        {
            if (firstItem != null)
            {
                if (firstItem.Value != null)
                {
                    var list = new List<SelectListItem>();
                    list.Add(firstItem);

                    if (selectList != null)
                    {
                        foreach (SelectListItem item in selectList)
                        {
                            list.Add(item);
                        }
                    }

                    selectList = list;
                }
            }

            return SelectExtensions.DropDownListFor(htmlHelper, expression, selectList, htmlAttributes);
        }

        #endregion Business

        #region Private Methods

        /// <summary>
        /// Gets the type of the non NULLABLE model.
        /// </summary>
        /// <param name="modelMetadata">The model metadata.</param>
        /// <returns>Object System.Type.</returns>
        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            var realModelType = modelMetadata.ModelType;

            var underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }

            return realModelType;
        }

        /// <summary>
        /// Creates the page node.
        /// </summary>
        /// <param name="url">The URL link.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="isActive">If set to <c>true</c> [is active].</param>
        /// <returns>Object System.String.</returns>
        private static string CreatePageNode(string url, int pageNumber, bool isActive = false)
        {
            return isActive
                ? "<li><a class='active' href='javascript:;'>" + pageNumber + "</a></li>"
                : string.Format("<li><a rel='external' href='" + url + "'>{0}</a></li>", pageNumber);
        }

        /// <summary>
        /// Creates the CheckBox inside div.
        /// </summary>
        /// <param name="name">The name for creating.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="showNameAttribute">If set to <c>true</c> [show name attribute].</param>
        /// <param name="fullHtmlFieldName">Full name of the HTML field.</param>
        /// <param name="sb">The string builder.</param>
        /// <param name="item">The select list item.</param>
        private static void CreateCheckBoxInsideDiv(
         string name,
         IDictionary<string, object> htmlAttributes,
         bool showNameAttribute,
         string fullHtmlFieldName,
         StringBuilder sb,
         SelectListItem item)
        {
            var input = new TagBuilder("input");
            input.MergeAttributes(htmlAttributes);
            input.MergeAttribute("type", "checkbox");
            input.MergeAttribute("id", name + item.Value);
            if (showNameAttribute)
            {
                input.MergeAttribute("name", fullHtmlFieldName, replaceExisting: true);
            }

            input.MergeAttribute("value", item.Value ?? item.Text ?? string.Empty, replaceExisting: true);
            if (item.Selected)
            {
                input.MergeAttribute("checked", "checked");
            }

            ////TagBuilder label = new TagBuilder("label");
            ////label.InnerHtml = input.ToString(TagRenderMode.SelfClosing) + item.Text;
            ////sb.Append(label.ToString());
            var label = new TagBuilder("label");
            label.MergeAttribute("for", name + item.Value);
            label.AddCssClass("font-normal");

            label.InnerHtml = item.Text;

            sb.Append("<div>" + input.ToString(TagRenderMode.SelfClosing) + label + "</div>");
        }

        /// <summary>
        /// Creates the list items.
        /// </summary>
        /// <param name="selectList">The select list.</param>
        /// <param name="values">The values.</param>
        /// <returns>Object IEnumerable{SelectListItem}.</returns>
        private static IEnumerable<SelectListItem> CreateListItems(IEnumerable<SelectListItem> selectList, object values)
        {
            var valueList = values is string[]? values as string[]
            : values.ToString().Split(new[] { ',' });

            var source = values as IEnumerable;

            var set = new HashSet<string>(
               from value in source.Cast<object>()
               select Convert.ToString(value, CultureInfo.CurrentCulture),
               StringComparer.OrdinalIgnoreCase);

            var list = new List<SelectListItem>();

            foreach (SelectListItem item in selectList)
            {
                item.Selected = (item.Value != null) && valueList.Contains(item.Value);
                list.Add(item);
            }

            selectList = list;
            return selectList;
        }

        /// <summary>
        /// Creates the CheckBox.
        /// </summary>
        /// <param name="name">The name for render.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="showNameAttribute">If set to <c>true</c> [show name attribute].</param>
        /// <param name="fullHtmlFieldName">Full name of the HTML field.</param>
        /// <param name="sb">The string builder.</param>
        /// <param name="item">The select list item.</param>
        private static void CreateCheckBox(
            string name,
            IDictionary<string, object> htmlAttributes,
            bool showNameAttribute,
            string fullHtmlFieldName,
            StringBuilder sb,
            SelectListItem item)
        {
            var input = new TagBuilder("input");
            input.MergeAttributes(htmlAttributes);
            input.MergeAttribute("type", "checkbox");
            input.MergeAttribute("id", name + item.Value);
            if (showNameAttribute)
            {
                input.MergeAttribute("name", fullHtmlFieldName, replaceExisting: true);
            }

            input.MergeAttribute("value", item.Value ?? item.Text ?? string.Empty, replaceExisting: true);

            if (item.Selected)
            {
                input.MergeAttribute("checked", "checked");
            }

            var label = new TagBuilder("label")
            {
                InnerHtml = input.ToString(TagRenderMode.SelfClosing) + item.Text
            };
            label.AddCssClass("font-normal");

            sb.Append(label.ToString());
        }

        #endregion Private Methods
    }
}