using NETCore1.Context;
using NETCore1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore1.Repository.Data
{
    public class RoleRepository : GeneralRepository<MyContext, Role, int>
    {

        private readonly MyContext myContext;
            public RoleRepository(MyContext myContext) : base(myContext)
            {
            this.myContext = myContext;
            }

        public bool InsertRole(Role role)
        {
            var checkRole = myContext.Roles.Where(x => x.RoleName.Equals(role.RoleName)).FirstOrDefault();

            if (checkRole != null)
            {
                return false;
            }
            else
            {
                myContext.Roles.Add(role);
                myContext.SaveChanges();
                return true;
            }
        }
    }
}
