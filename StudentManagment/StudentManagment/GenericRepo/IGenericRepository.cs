using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace StudentManagment.GenericRepo
{
    interface IGenericRepository<TEntity>
    {  
         IQueryable<TEntity> GetAll();

         TEntity GetById(int id);

         bool Delete(TEntity obj);

         bool Insert(TEntity obj);

         bool Edit(TEntity obj);

         IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);
    }
}
