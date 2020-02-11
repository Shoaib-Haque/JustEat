using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JustEat.Interface;
using JustEat.Models;
using System.Data.Entity;

namespace JustEat.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private JustEatEntities jeEntities = new JustEatEntities();

        public Repository(JustEatEntities jeEntities)
        {
            this.jeEntities = jeEntities;
        }

        public TEntity GetById(int id)
        {
            return jeEntities.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return jeEntities.Set<TEntity>().ToList();
        }

        public void Insert(TEntity entity)
        {
            jeEntities.Set<TEntity>().Add(entity);
            jeEntities.SaveChanges();
        }

        public void Delete(int id)
        {
            jeEntities.Set<TEntity>().Remove(jeEntities.Set<TEntity>().Find(id));
            jeEntities.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            jeEntities.Entry(entity).State = EntityState.Modified;

            jeEntities.SaveChanges();
        }
    }
}