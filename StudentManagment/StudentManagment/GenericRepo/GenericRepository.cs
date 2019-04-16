using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace StudentManagment.GenericRepo
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected DbSet<TEntity> _DbSet;

        protected readonly DbContext _DbContext;

        public GenericRepository()
        {

        }

        public GenericRepository(DbContext DbContext)
        {
            _DbContext = DbContext;
            _DbSet = DbContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _DbSet;
        }

        public TEntity GetById(int id)
        {
            return _DbSet.Find(id);
        }

        public bool Delete(TEntity obj)
        {
            try
            {
                _DbSet.Remove(obj);
                _DbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool Insert(TEntity obj)
        {
            try
            {

                _DbSet.Add(obj);
                _DbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool Edit(TEntity obj)
        {
            try
            {

                _DbContext.Entry(obj).State = EntityState.Modified;
                _DbContext.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            return _DbSet.Where(predicate); 
        }

    }
}