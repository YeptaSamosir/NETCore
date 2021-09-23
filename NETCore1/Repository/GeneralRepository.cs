using Microsoft.EntityFrameworkCore;
using NETCore1.Context;
using NETCore1.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore1.Repository
{
    public class GeneralRepository<Context, Entity, Key> : IRepository<Entity, Key>
        where Entity : class
        where Context : MyContext
    {
        private readonly MyContext myContext;
        private readonly DbSet<Entity>dbSet;

        public GeneralRepository(MyContext myContext)
        {
            this.myContext = myContext;
            dbSet = myContext.Set<Entity>();
        }

        public int Delete(Key key)
        {
            var delete = dbSet.Find(key);
            if (delete == null)
            {
                throw new ArgumentNullException();
            }
            dbSet.Remove(delete);
            var deleted = myContext.SaveChanges();
            return deleted;
        }

        public IEnumerable<Entity> Get()
        {
            if (dbSet.ToList().Count == 0)
            {
                throw new ArgumentNullException();
            }
            return dbSet.ToList();
        }

        public Entity Get(Key key)
        {
            if (dbSet.Find(key) != null)
            {
                return dbSet.Find(key);
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        public int Insert(Entity entity)
        {
            try
            {
                dbSet.Add(entity);
                var insert = myContext.SaveChanges();
                return insert;
            }
            catch
            {
                throw new DbUpdateException();
            }
        }

        public int Update(Entity entity)
        {
            if (entity==null)
            {
                throw new Exception();
            }
            myContext.Entry(entity).State= EntityState.Modified;
            var update = myContext.SaveChanges();
         
            return update;
        }
    }
}
