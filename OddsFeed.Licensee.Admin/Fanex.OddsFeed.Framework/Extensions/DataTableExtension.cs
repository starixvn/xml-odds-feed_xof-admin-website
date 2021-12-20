namespace Fanex.OddsFeed.Framework.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Common.Extensions;

    public static class DataTableExtension
    {
        private static readonly List<Type> NumericTypes = new List<Type>
        {
            typeof(byte), typeof(decimal),
            typeof(double), typeof(short),
            typeof(int), typeof(long),
            typeof(sbyte), typeof(float),
            typeof(uint),
            typeof(ulong)
        };

        public static bool IsNumeric(this DataColumn col)
        {
            if (col == null)
            {
                return false;
            }

            return NumericTypes.Contains(col.DataType);
        }

        public static void AddSumRow(this DataTable dataTable)
        {
            var newDataRow = dataTable.NewRow();
            foreach (DataColumn column in dataTable.Columns)
            {
                // If Numeric, do this
                if (column.IsNumeric())
                {
                    double sumOfColumn = 0;
                    double valueOfCell = 0;
                    var isdouble = false;
                    foreach (DataRow row in dataTable.Rows)
                    {
                        valueOfCell = 0;
                        /* Check if it has value add this, or not then set 0 value
                         */

                        isdouble = double.TryParse(row[column].ToString(), out valueOfCell);
                        if (isdouble)
                        {
                            sumOfColumn += valueOfCell;
                        }
                        else
                        {
                            if (row[column].ToString().IsNullOrTrimEmpty())
                            { // If row has nothing, set 0 value for it
                                row[column] = 0;
                            }
                        }
                    }

                    /* Add value to sum column*/
                    newDataRow[column] = sumOfColumn;
                    sumOfColumn = 0;
                }
            }

            dataTable.Rows.Add(newDataRow);
        }
    }
}