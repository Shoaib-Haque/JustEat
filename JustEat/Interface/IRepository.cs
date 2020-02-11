using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JustEat.Interface
{
    interface IRepository<TEntity> where TEntity : class
    {

        TEntity GetById(int id);
        IEnumerable<TEntity> GetAll();

        void Insert(TEntity entity);
        void Delete(int id);

        void Update(TEntity entity);

        //void Save();
    }
}
