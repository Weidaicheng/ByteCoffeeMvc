using ByteCoffee.Utility.Data;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ByteCoffee.Utility.Extensions
{
    public class BootstrapTableSortModel
    {
        /// <summary>
        /// Gets or sets the field.
        /// </summary>
        /// <value>
        /// The field.
        /// </value>
        public string field { get; set; }

        /// <summary>
        /// Gets or sets the order.
        /// </summary>
        /// <value>
        /// The order.
        /// </value>
        public string order { get; set; }
    }

    public static class BootstrapTableExtensions
    {
        public static SortCondition[] SortConditions(this string sort, string sortDir, string defaultSort, ListSortDirection defaultSortDir)
        {
            var sortConditions = new List<SortCondition>();
            if (!string.IsNullOrEmpty(sort))
            {
                sortConditions.Add(sortDir.ToLower() == "desc"
                                            ? new SortCondition(sort, ListSortDirection.Descending)
                                            : new SortCondition(sort, ListSortDirection.Ascending));
            }
            else
            { sortConditions.Add(new SortCondition(defaultSort, defaultSortDir)); }
            return sortConditions.ToArray();
        }

        public static SortCondition[] SortConditions(this string sort, string sortDir, List<SortCondition> sortList)
        {
            var sortConditions = new List<SortCondition>();
            if (!string.IsNullOrEmpty(sort))
            {
                sortConditions.Add(sortDir.ToLower() == "desc"
                                            ? new SortCondition(sort, ListSortDirection.Descending)
                                            : new SortCondition(sort, ListSortDirection.Ascending));
            }
            else
            { sortConditions.AddRange(sortList.Select(item => new SortCondition(item.SortField, item.ListSortDirection))); }
            return sortConditions.ToArray();
        }

        /// <summary>
        /// Gets the bootstrap table sort data.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static SortCondition GetBootstrapTableSortData(this string json)
        {
            try
            {
                var sortObj = json.Length > 0 ? JsonConvert.DeserializeObject<BootstrapTableSortModel>(json) : null;
                if (sortObj != null)
                {
                    if (sortObj.field.IsNullOrEmpty()
                        || sortObj.order.IsNullOrEmpty())
                    { return null; }

                    var _SortCondition = new SortCondition(sortObj.field);
                    if (string.Compare(sortObj.order, "asc", true) == 0)
                    { _SortCondition.ListSortDirection = ListSortDirection.Ascending; }
                    else if (string.Compare(sortObj.order, "desc", true) == 0)
                    { _SortCondition.ListSortDirection = ListSortDirection.Descending; }
                    return _SortCondition;
                }
                return null;
            }
            catch { return null; }
        }

        /// <summary>
        /// Gets the bootstrap table sort data list.
        /// </summary>
        /// <param name="json">The json.</param>
        /// <returns>初始排序集合</returns>
        public static SortCondition[] GetBootstrapTableSortDataList(this string json)
        {
            try
            {
                List<SortCondition> sortList = new List<SortCondition>();
                var sortObj = json.Length > 0 ? JsonConvert.DeserializeObject<BootstrapTableSortModel>(json) : null;
                if (sortObj != null)
                {
                    if (sortObj.field.IsNullOrEmpty()
                        || sortObj.order.IsNullOrEmpty())
                    { return null; }

                    var _SortCondition = new SortCondition(sortObj.field);

                    if (string.Compare(sortObj.order, "asc", true) == 0)
                    { _SortCondition.ListSortDirection = ListSortDirection.Ascending; }
                    else if (string.Compare(sortObj.order, "desc", true) == 0)
                    { _SortCondition.ListSortDirection = ListSortDirection.Descending; }
                    else
                    { _SortCondition.ListSortDirection = ListSortDirection.Ascending; }

                    sortList.Add(_SortCondition);
                }
                if (sortList.Count > 0)
                { return sortList.ToArray(); }
                else
                { return null; }
            }
            catch { return null; }
        }
    }
}