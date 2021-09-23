using NETCore1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore1.Repository.Interface
{
    interface IPersonRepository
    {
        IEnumerable<Person> Get();
        Person Get(string NIK);

        int Insert(Person person);
        int Update(Person person);
        int Delete(string NIK);

    }
}
