using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BilgeShop.Data.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        void Add(TEntity entity);
        void Delete(int id);
        void Delete(TEntity entity);
        void Update(TEntity entity);
        TEntity GetById(int id);

        IQueryable<TEntity> GetAll(Expression<Func<TEntity , bool>> predicate = null);

        // Bir Sql sorgunu parametre olarak göndermek istiyorsanız tipi ->  Expression<Func<TEntity , bool>>

        // = null diyerek bu metodun parametre alarak veya almayarak çalışabileceğini gösteriyorum.

        // Parametre alırsa istenilen koşula uyan yapıları dönecek.
        // Parametre almazsa bütün yapıları dönecek.

        TEntity Get(Expression<Func<TEntity, bool>> predicate);
    }
}
