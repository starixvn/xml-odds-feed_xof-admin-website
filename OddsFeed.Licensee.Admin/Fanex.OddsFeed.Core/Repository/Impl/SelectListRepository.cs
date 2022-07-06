namespace Fanex.OddsFeed.Core.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Mvc;
    using Data.Repository;
    using Fanex.Data;

    public class SelectListRepository : ISelectListRepository
    {
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
                            var item = new SelectListItem
                            {
                                Group = new SelectListGroup()
                            };

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