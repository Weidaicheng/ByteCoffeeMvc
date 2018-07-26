using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace ByteCoffee.Data
{
    public class EFDAL<TEntity> : IDAL<TEntity>
      where TEntity : class, new()
    {
        private DbContext _db;

        protected DbContext Db
        {
            get
            {
                if (_db == null)
                { throw new NullReferenceException("Db对象为null"); }
                return _db;
            }
            private set { this._db = value; }
        }

        private IDAO _dao;

        public IDAO DAO
        {
            get
            {
                if (_dao == null)
                { throw new NullReferenceException("DAO对象为null"); }
                return _dao;
            }
            private set { this._dao = value; }
        }

        public IQueryable<TEntity> ReadEntities
        {
            get { return Db.Set<TEntity>().AsNoTracking(); }
        }

        public IQueryable<TEntity> TrackEntities
        {
            get { return Db.Set<TEntity>(); }
        }

        public void SetDbContext(IDAO dao)
        {
            if (dao == null)
            { throw new NullReferenceException("dao对象为null"); }

            this.Db = (DbContext)dao;
            this.DAO = dao;
        }

        #region 同步实现

        public virtual TEntity Find(params object[] ids)
        {
            return Db.Set<TEntity>().Find(ids);
        }

        public virtual int Insert(TEntity item)
        {
            EntityState state = Db.Entry(item).State;
            if (state == EntityState.Detached)
            { Db.Entry(item).State = EntityState.Added; }
            return DAO.SaveChanges();
        }

        public virtual int Insert(IEnumerable<TEntity> items)
        {
            items.ToList().ForEach(i =>
            {
                Db.Entry<TEntity>(i);
                Db.Set<TEntity>().Add(i);
            });
            return DAO.SaveChanges();
        }

        public virtual int AddRange(IEnumerable<TEntity> items)
        {
            Db.Set<TEntity>().AddRange(items);
            return DAO.SaveChanges();
        }

        public virtual int Delete(TEntity item)
        {
            Db.Entry(item).State = EntityState.Deleted;
            return DAO.SaveChanges();
        }

        public virtual int Delete(params object[] Ids)
        {
            Ids.ToList().ForEach(i =>
            {
                TEntity item = Db.Set<TEntity>().Find(i);
                Db.Entry(item).State = EntityState.Deleted;
            });
            return DAO.SaveChanges();
        }

        public virtual int Delete(IEnumerable<TEntity> items)
        {
            items.ToList().ForEach(i =>
            {
                Db.Set<TEntity>().Attach(i);
                Db.Set<TEntity>().Remove(i);
            });
            return DAO.SaveChanges();
        }

        public virtual int RemoveRange(IEnumerable<TEntity> items)
        {
            Db.Set<TEntity>().RemoveRange(items);
            return DAO.SaveChanges();
        }

        public virtual int Update(TEntity item)
        {
            if (Db.Entry(item).State == EntityState.Detached)
            { Db.Set<TEntity>().Attach(item); }
            Db.Entry(item).State = EntityState.Modified;
            return DAO.SaveChanges();
        }

        public virtual int Update(IEnumerable<TEntity> items)
        {
            items.ToList().ForEach(i =>
            {
                if (Db.Entry(i).State == EntityState.Detached)
                { Db.Set<TEntity>().Attach(i); }
                Db.Entry(i).State = EntityState.Modified;
            });
            return DAO.SaveChanges();
        }

        #endregion 同步实现

        #region 异步实现

        public virtual async Task<TEntity> FindAsync(params object[] ids)
        {
            return await Db.Set<TEntity>().FindAsync(ids);
        }

        public virtual async Task<int> InsertAsync(TEntity item)
        {
            var state = Db.Entry(item).State;
            if (state == EntityState.Detached)
            { Db.Entry(item).State = EntityState.Added; }

            return await DAO.SaveChangesAsync();
        }

        public virtual async Task<int> InsertAsync(IEnumerable<TEntity> items)
        {
            items.ToList().ForEach(i =>
            {
                Db.Entry<TEntity>(i);
                Db.Set<TEntity>().Add(i);
            });
            return await DAO.SaveChangesAsync();
        }

        public virtual async Task<int> AddRangeAsync(IEnumerable<TEntity> items)
        {
            Db.Set<TEntity>().AddRange(items);
            return await DAO.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(TEntity item)
        {
            Db.Entry(item).State = EntityState.Deleted;
            return await DAO.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(params object[] Ids)
        {
            //并行?
            Ids.ToList().ForEach(async i =>
            {
                TEntity item = await Db.Set<TEntity>().FindAsync(i);
                Db.Entry(item).State = EntityState.Deleted;
            });
            return await DAO.SaveChangesAsync();
        }

        public virtual async Task<int> DeleteAsync(IEnumerable<TEntity> items)
        {
            items.ToList().ForEach(i =>
            {
                Db.Set<TEntity>().Attach(i);
                Db.Set<TEntity>().Remove(i);
            });
            return await DAO.SaveChangesAsync();
        }

        public virtual async Task<int> RemoveRangeAsync(IEnumerable<TEntity> items)
        {
            Db.Set<TEntity>().RemoveRange(items);
            return await DAO.SaveChangesAsync();
        }

        public virtual async Task<int> UpdateAsync(TEntity item)
        {
            if (Db.Entry(item).State == EntityState.Detached)
            { Db.Set<TEntity>().Attach(item); }
            Db.Entry(item).State = EntityState.Modified;
            return await DAO.SaveChangesAsync();
        }

        public virtual async Task<int> UpdateAsync(IEnumerable<TEntity> items)
        {
            items.ToList().ForEach(i =>
            {
                if (Db.Entry(i).State == EntityState.Detached)
                { Db.Set<TEntity>().Attach(i); }
                Db.Entry(i).State = EntityState.Modified;
            });
            return await DAO.SaveChangesAsync();
        }

        #endregion 异步实现
    }
}