using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ByteCoffee.Data
{
    public interface IDAL<TEntity>
        where TEntity : class, new()
    {
        #region 属性

        /// <summary>
        /// Gets the DAO.
        /// </summary>
        /// <value>
        /// The DAO.
        /// </value>
        IDAO DAO { get; }

        /// <summary>
        /// 获取 当前实体的查询数据集(未跟踪到数据库)
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        IQueryable<TEntity> ReadEntities { get; }

        /// <summary>
        /// 获取 当前实体的数据集
        /// </summary>
        /// <value>
        /// The entities.
        /// </value>
        IQueryable<TEntity> TrackEntities { get; }

        #endregion

        #region 公共方法

        /// <summary>
        /// 设定数据上下文,它一般由构架方法注入
        /// </summary>
        /// <param name="dao">The unit of work.</param>
        void SetDbContext(IDAO dao);

        #region 同步

        /// <summary>
        /// 根据主键得到实体
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        TEntity Find(params object[] ids);

        /// <summary>
        /// 添加实体并提交到数据服务器
        /// </summary>
        /// <param name="item">Item to add to repository</param>
        /// <returns></returns>
        int Insert(TEntity item);

        /// <summary>
        /// 批量插入实体记录集合
        /// </summary>
        /// <param name="items">实体记录集合</param>
        /// <returns>
        /// 操作影响的行数
        /// </returns>
        int Insert(IEnumerable<TEntity> items);

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="items">实体记录集合</param>
        /// <returns></returns>
        int AddRange(IEnumerable<TEntity> items);

        /// <summary>
        /// 移除实体并提交到数据服务器
        /// 如果表存在约束，需要先删除子表信息
        /// </summary>
        /// <param name="item">Item to delete</param>
        /// <returns></returns>
        int Delete(TEntity item);

        /// <summary>
        /// 移除实体并提交到数据服务器
        /// 如果表存在约束，需要先删除子表信息
        /// </summary>
        /// <param name="Ids">The ids.</param>
        /// <returns></returns>
        int Delete(params object[] Ids);

        /// <summary>
        /// 删除实体记录集合
        /// </summary>
        /// <param name="items">实体记录集合</param>
        /// <returns>
        /// 操作影响的行数
        /// </returns>
        int Delete(IEnumerable<TEntity> items);

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="items">实体记录集合</param>
        /// <returns></returns>
        int RemoveRange(IEnumerable<TEntity> items);

        /// <summary>
        /// 修改实体并提交到数据服务器
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        int Update(TEntity item);

        /// <summary>
        /// 修改实体并提交到数据服务器
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        int Update(IEnumerable<TEntity> items);
        #endregion

        #region 异步

        /// <summary>
        /// 根据主键得到实体
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        Task<TEntity> FindAsync(params object[] ids);

        /// <summary>
        /// 添加实体并提交到数据服务器
        /// </summary>
        /// <param name="item">Item to add to repository</param>
        /// <returns></returns>
        Task<int> InsertAsync(TEntity item);

        /// <summary>
        /// 批量插入实体记录集合
        /// </summary>
        /// <param name="items">实体记录集合</param>
        /// <returns>
        /// 操作影响的行数
        /// </returns>
        Task<int> InsertAsync(IEnumerable<TEntity> items);

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="items">实体记录集合</param>
        /// <returns></returns>
        Task<int> AddRangeAsync(IEnumerable<TEntity> items);

        /// <summary>
        /// 移除实体并提交到数据服务器
        /// 如果表存在约束，需要先删除子表信息
        /// </summary>
        /// <param name="item">Item to delete</param>
        /// <returns></returns>
        Task<int> DeleteAsync(TEntity item);

        /// <summary>
        /// 移除实体并提交到数据服务器
        /// 如果表存在约束，需要先删除子表信息
        /// </summary>
        /// <param name="Ids">The ids.</param>
        /// <returns></returns>
        Task<int> DeleteAsync(params object[] Ids);

        /// <summary>
        /// 删除实体记录集合
        /// </summary>
        /// <param name="items">实体记录集合</param>
        /// <returns>
        /// 操作影响的行数
        /// </returns>
        Task<int> DeleteAsync(IEnumerable<TEntity> items);

        /// <summary>
        /// 批量删除数据
        /// </summary>
        /// <param name="items">实体记录集合</param>
        /// <returns></returns>
        Task<int> RemoveRangeAsync(IEnumerable<TEntity> items);

        /// <summary>
        /// 修改实体并提交到数据服务器
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        Task<int> UpdateAsync(TEntity item);

        /// <summary>
        /// 修改实体并提交到数据服务器
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        Task<int> UpdateAsync(IEnumerable<TEntity> items);

        #endregion

        #endregion
    }

}
