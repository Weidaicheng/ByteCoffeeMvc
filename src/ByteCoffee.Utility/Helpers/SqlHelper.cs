using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ByteCoffee.Utility.Helpers
{
    public class SqlHelper
    {
        ///// <summary>
        ///// 根据ID取得XML中的对应SQL文
        ///// </summary>
        ///// <param name="sqlId">xml配置id</param>
        ///// <param name="pramas"> SQL文中的Format参数</param>
        ///// <returns>返回sql语句</returns>
        //[Obsolete("禁用Fomart占位符，使用参数方式传递SQL文中应该以@id出现")]
        //public static string GetSqlById(string sqlId, object[] pramas)
        //{
        //    var sql = GetSqlById(sqlId);
        //    return string.Format(sql, pramas);
        //}
        ///// <summary>
        ///// 根据~/Configs/Sql.config中节点ID取得XML中的对应SQL文
        ///// </summary>
        ///// <param name="sqlId">ID</param>
        ///// <returns>SQL语句</returns>
        //public static string GetSqlById(string sqlId)
        //{
        //    var path = System.Web.HttpContext.Current.Server.MapPath("~/Configs/Sql.config");
        //    var xdoc = XElement.Load(path);
        //    var els = xdoc.Elements("xmlsql").ToList();
        //    var sql = string.Empty;
        //    if (els.Any())
        //    {
        //        sql = els.First(e =>
        //        {
        //            var xAttribute = e.Attribute("id");
        //            return xAttribute != null && xAttribute.Value == sqlId;
        //        }).Value;
        //    }
        //    return sql;
        //}

        /// <summary>
        /// 描述：构造数据源，复制到指定表名
        /// </summary>
        /// <param name="collection"> 构造数据源</param>
        public static void ExecuteSqlBulkCopy<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var name = typeof(T).Name;
            var dt = new DataTable();
            var dbConn = ConfigurationManager.AppSettings["dbConnection"];//替换了已过期的方法

            //dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            //dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType)).ToArray());

            foreach (var property in props)
            { dt.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType); }

            if (collection.Any())
            {
                for (var i = 0; i < collection.Count(); i++)
                {
                    var tempList = new ArrayList();
                    foreach (var obj in props.Select(pi => pi.GetValue(collection.ElementAt(i), null)))
                    { tempList.Add(obj); }
                    var array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            var _dtDefalut = dt.DefaultView.ToTable(true, props.Select(p => p.Name).ToArray());
            using (var conn = new SqlConnection(dbConn))
            {
                var bulk = new SqlBulkCopy(conn);
                bulk.DestinationTableName = name;
                bulk.BatchSize = _dtDefalut.Rows.Count;
                if (_dtDefalut.Rows.Count != 0)
                {
                    conn.Open();
                    bulk.WriteToServer(_dtDefalut);
                }
                bulk.Close();
            }
        }
    }
}