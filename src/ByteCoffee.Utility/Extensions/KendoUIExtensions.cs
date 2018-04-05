namespace ByteCoffee.Utility.Extensions
{
    #region add by qijin 20130906

    ///// <summary>
    ///// ExtJS的排序数据的反序列化类以及帮助方法
    ///// </summary>
    //[Serializable]
    //public class KendoGridSortData
    //{
    //    //[{"field":"Title","dir":"desc","compare":null}]
    //    public string field { get; set; }

    //    public string dir { get; set; }

    //    public string compare { get; set; }
    //}

    #endregion add by qijin 20130906

    public static class KendoUIExtensions
    {
        #region KendoUI Sort Extensions add by qijin 20130923

        ///// <summary>
        ///// Gets the ExtJSGridSortData[] form josn.
        ///// </summary>
        ///// <param name="json">The json.</param>
        ///// <returns></returns>
        //public static KendoGridSortData[] GetKendoGridSortDataFormJosn(this string json)
        //{
        //    try
        //    { return json.Length > 0 ? JsonConvert.DeserializeObject<KendoGridSortData[]>(json) : null; }
        //    catch { return null; }
        //}

        ///// <summary>
        ///// Gets the sort data from kendo sort json.
        ///// </summary>
        ///// <param name="json">The json.</param>
        ///// <returns></returns>
        //public static SortCondition[] GetSortDataFromKendoJson(this string json)
        //{
        //    try
        //    {
        //        return !string.IsNullOrEmpty(json) ?
        //            ConvertSortDataFromKendoGridSortData(JsonConvert.DeserializeObject<KendoGridSortData[]>(json)) : null;
        //    }
        //    catch { return null; }
        //}

        ///// <summary>
        ///// Gets from ext js grid sort data.
        ///// </summary>
        ///// <param name="_sortdata">The _sortdata.</param>
        ///// <returns></returns>
        //private static SortCondition[] ConvertSortDataFromKendoGridSortData(KendoGridSortData[] _sortdata)
        //{
        //    if (_sortdata != null)
        //    {
        //        var list = _sortdata.Select(item => new SortCondition(item.field, string.Compare(item.dir, "ASC", true) == 0 ? ListSortDirection.Ascending : ListSortDirection.Descending)).ToList();
        //        return list.ToArray<SortCondition>();
        //    }
        //    return null;
        //}

        #endregion KendoUI Sort Extensions add by qijin 20130923

        //public static SortCondition[] SortConditions(this string sortString, string defaultSort, ListSortDirection SortDir)
        //{
        //    var sortFromRequest = sortString.GetSortDataFromKendoJson();
        //    var sortConditions = new List<SortCondition>();
        //    if (sortFromRequest != null && sortFromRequest.Length > 0)
        //    { sortConditions.AddRange(sortFromRequest); }
        //    else
        //    { sortConditions.Add(new SortCondition(defaultSort, SortDir)); }
        //    return sortConditions.ToArray();
        //}

        //public static SortCondition[] SortConditions(this string sortString, List<SortCondition> sortList)
        //{
        //    var sortFromRequest = sortString.GetSortDataFromKendoJson();
        //    var sortConditions = new List<SortCondition>();
        //    if (sortFromRequest != null && sortFromRequest.Length > 0)
        //    { sortConditions.AddRange(sortFromRequest); }
        //    else
        //    { sortConditions.AddRange(sortList.Select(item => new SortCondition(item.SortField, item.ListSortDirection))); }
        //    return sortConditions.ToArray();
        //}
    }
}