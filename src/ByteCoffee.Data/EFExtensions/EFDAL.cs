using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace ByteCoffee.Data
{
    public class EFDAL<TEntity> : IDAL<TEntity>
      where TEntity : class,new()
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

        public IQueryable<TEntity> Entities
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
    }
}
