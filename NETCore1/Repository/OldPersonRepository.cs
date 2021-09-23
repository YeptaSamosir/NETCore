using Microsoft.EntityFrameworkCore;
using NETCore1.Context;
using NETCore1.Models;
using NETCore1.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore1.Repository
{
    public class OldPersonRepository 
    {
        private readonly MyContext myContext;
        public OldPersonRepository(MyContext myContext)
        {
            this.myContext = myContext;
        }
        public int Delete(string NIK)
        {
            var delete = myContext.Persons.Find(NIK);
            if (delete == null)
            {
                throw new ArgumentNullException();
            }
            myContext.Persons.Remove(delete);
            var deleted = myContext.SaveChanges();
            return deleted;

        }

        public IEnumerable<Person> Get()
        {
            if (myContext.Persons.ToList().Count == 0)
            {
                throw new ArgumentNullException();
            }
            return myContext.Persons.ToList();
            
        }

        public Person Get(string NIK)
        {

            if (myContext.Persons.Find(NIK) != null)
            {
                return myContext.Persons.Find(NIK);
            }
            else
            {
                throw new ArgumentNullException();
            }
            
        }

        public int Insert(Person person)
        {
            try
            {
                myContext.Persons.Add(person);
                var insert = myContext.SaveChanges();
                return insert;
            }
            catch
            {
                throw new DbUpdateException();
            }




        }

        public int Update(Person person)
        {

            if (person.NIK == null)
            {
                throw new Exception();
            }
            myContext.Entry(person).State = EntityState.Modified;
            var update = myContext.SaveChanges();
         
            return update;
        }
    }
}
