// --------------------------------------------------------------------------
// <copyright file="SelectListRepository.cs" company=" Solutions Vietnam">
// Copyright (c)  Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Phillip.Truong" date="2013.2.26">Created</change>
// </history>

namespace Fanex.Databank.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Mvc;
    using Data.Repository;
    using Fanex.Data;

    /// <summary>
    /// Class Convert data reader to SelectList.
    /// </summary>
    public class SelectListRepository
    {
        /// <summary>
        /// Public function convert data reader to SelectListItem.
        /// </summary>
        /// <param name="settingKey">The setting key.</param>
        /// <param name="order">If True then (Value, Text). Else (Text, Value). </param>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns>IEnumerable of Select List Item.</returns>
        public IEnumerable<SelectListItem> Get(string settingKey, bool order = true, string selectedValue = "")
        {
            var result = new List<SelectListItem>();
            using (ObjectDb objectDb = new ObjectDb(settingKey))
            {
                using (IDataReader reader = objectDb.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var item = new SelectListItem();
                            if (order)
                            {
                                item.Value = reader[0].ToString();
                                item.Text = reader[1].ToString();
                                item.Selected = selectedValue.Equals(reader[1].ToString(), StringComparison.OrdinalIgnoreCase);
                            }
                            else
                            {
                                item.Value = reader[1].ToString();
                                item.Text = reader[0].ToString();
                                item.Selected = selectedValue.Equals(reader[1].ToString(), StringComparison.OrdinalIgnoreCase);
                            }

                            result.Add(item);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Public function convert data reader to SelectListItem.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="order">If True then (Value, Text). Else (Text, Value). </param>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns>IEnumerable of Select List Item.</returns>
        public IEnumerable<SelectListItem> Get(ICriteria criteria, bool order = true, string selectedValue = "")
        {
            var result = new List<SelectListItem>();
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.AssignParamValues(criteria.GetSpParams());
                using (IDataReader reader = objectDb.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var item = new SelectListItem();
                            if (order)
                            {
                                item.Value = reader[0].ToString();
                                item.Text = reader[1].ToString();
                                item.Selected = selectedValue.Equals(reader[1].ToString(), StringComparison.OrdinalIgnoreCase);
                            }
                            else
                            {
                                item.Value = reader[1].ToString();
                                item.Text = reader[0].ToString();
                                item.Selected = selectedValue.Equals(reader[1].ToString(), StringComparison.OrdinalIgnoreCase);
                            }

                            result.Add(item);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Public function convert data reader to SelectListItem.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <param name="order">If True then (Value, Text). Else (Text, Value). </param>
        /// <param name="selectedValue">The selected value.</param>
        /// <returns>IEnumerable of Select List Item with Group.</returns>
        public IEnumerable<SelectListItem> GetWithGroup(ICriteria criteria, bool order = true, string selectedValue = "")
        {
            var result = new List<SelectListItem>();
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.AssignParamValues(criteria.GetSpParams());
                using (IDataReader reader = objectDb.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var item = new SelectListItem();
                            item.Group = new SelectListGroup();

                            int colIndex = 0;
                            if (order)
                            {
                                item.Value = reader[colIndex++].ToString();
                                item.Text = reader[colIndex++].ToString();
                                item.Group.Name = reader[colIndex].ToString();
                                item.Group.Disabled = false;

                                item.Selected = selectedValue.Equals(reader[1].ToString(), StringComparison.OrdinalIgnoreCase);
                            }
                            else
                            {
                                item.Text = reader[colIndex++].ToString();
                                item.Value = reader[colIndex++].ToString();
                                item.Group.Name = reader[colIndex].ToString();
                                item.Group.Disabled = false;

                                item.Selected = selectedValue.Equals(reader[1].ToString(), StringComparison.OrdinalIgnoreCase);
                            }

                            result.Add(item);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Public function convert data reader to SelectListItem.
        /// </summary>
        /// <param name="criteria">The Criteria.</param>
        /// <param name="order">If True then (Value, Text). Else (Text, Value). </param>
        /// <returns>Select List Item.</returns>
        public IEnumerable<SelectListItem> GetAndGenerateCheck(ICriteria criteria, bool order = true)
        {
            var result = new List<SelectListItem>();
            using (ObjectDb objectDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objectDb.AssignParamValues(criteria.GetSpParams());
                using (IDataReader reader = objectDb.ExecuteReader())
                {
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            var item = new SelectListItem();
                            if (order)
                            {
                                item.Value = reader[0].ToString();
                                item.Text = reader[1].ToString();
                                item.Selected = bool.Parse(reader[2].ToString());  ////get "IsChecked" column
                            }
                            else
                            {
                                item.Value = reader[1].ToString();
                                item.Text = reader[0].ToString();
                                item.Selected = bool.Parse(reader[2].ToString()); ////get "IsChecked" column
                            }

                            result.Add(item);
                        }
                    }
                }
            }

            return result;
        }
    }
}