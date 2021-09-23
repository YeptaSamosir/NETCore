using Microsoft.EntityFrameworkCore;
using NETCore1.Context;
using NETCore1.Hash;
using NETCore1.Models;
using NETCore1.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace NETCore1.Repository.Data
{
    public class PersonRepository : GeneralRepository<MyContext, Person, string>
    {
        private readonly MyContext myContext;
        public PersonRepository (MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }

        public PersonViewModel GetPerson(string nnik)
        {
            var GetPerson = (from p in myContext.Persons
                             where p.NIK == nnik
                             join a in myContext.Accounts
                             on p.NIK equals a.NIK
                             join pr in myContext.Profilings
                             on a.NIK equals pr.NIK
                             join e in myContext.Educations
                             on pr.Education_id equals e.Id
                             join u in myContext.Universities
                             on e.Univesity_id equals u.Id

                             select new PersonViewModel
                             {
                                 NIK = p.NIK,
                                 NamaDepan = p.Firstname + " " + p.Lastname,
                                 NamaBelakang = p.Lastname,
                                 Email = p.Email,
                                 Telp = p.Phone,
                                 TglLahir = p.BirthDate,
                                 Password = a.Password,
                                 Degree = e.Degree,
                                 GPA = e.GPA,
                                 Unversity_Id = u.Id,
                                 Education_id = e.Id
                             }).ToList();

            return GetPerson.FirstOrDefault(x => x.NIK == nnik);

        }
        public IEnumerable<PersonViewModel> GetPerson()
        {
            var GetPerson = (from p in myContext.Persons
                             join a in myContext.Accounts
                             on p.NIK equals a.NIK
                             join pr in myContext.Profilings
                             on a.NIK equals pr.NIK
                             join e in myContext.Educations
                             on pr.Education_id equals e.Id
                             join u in myContext.Universities
                             on e.Univesity_id equals u.Id
                             select new PersonViewModel
                             {
                                 NIK = p.NIK,
                                 NamaDepan = p.Firstname + " " + p.Lastname,
                                 NamaBelakang = p.Lastname,
                                 Email = p.Email,
                                 Telp = p.Phone,
                                 TglLahir = p.BirthDate,
                                 Password = a.Password,
                                 Degree = e.Degree,
                                 GPA = e.GPA,
                                 Unversity_Id = u.Name,
                                 Education_id = e.Id
                             }).ToList();

            return GetPerson;

        }
        public int Register(PersonViewModel personViewModel)
        {
            var checkEmail = myContext.Persons.Where(x => x.Email.Equals(personViewModel.Email)).FirstOrDefault();
            var checkNIK = myContext.Persons.Where(x => x.NIK.Equals(personViewModel.NIK)).FirstOrDefault();
            var checkPhone = myContext.Persons.Where(x => x.Phone.Equals(personViewModel.Telp)).FirstOrDefault();

            if (checkEmail is null && checkNIK is null && checkPhone is null)
            {
                var person = new Person()
                {
                    NIK = personViewModel.NIK,
                    Firstname = personViewModel.NamaDepan,
                    Lastname = personViewModel.NamaBelakang,
                    Phone = personViewModel.Telp,
                    BirthDate = personViewModel.TglLahir,
                    Email = personViewModel.Email,
                };
                myContext.Persons.Add(person);
                var insert = myContext.SaveChanges();
                var account = new LoginVm()
                {
                    NIK = personViewModel.NIK,
                    Password = Hashing.HashPassword(personViewModel.Password)
                };
                myContext.Accounts.Add(account);
                insert = myContext.SaveChanges();
                var education = new Education()
                {
                    Degree = personViewModel.Degree,
                    GPA = personViewModel.GPA,
                    Univesity_id = personViewModel.Unversity_Id
                };
                var role_account = new RoleAccount()
                {
                    Nik = personViewModel.NIK,
                    Role_id = 1
                };
                myContext.RoleAccounts.Add(role_account);

                myContext.Educations.Add(education);
                insert = myContext.SaveChanges();
                var profiling = new Profiling
                {
                    NIK = personViewModel.NIK,
                    Education_id = education.Id
                };
                myContext.Profilings.Add(profiling);
                insert = myContext.SaveChanges();
                return 200;
            }
            else if(checkNIK != null)
            {
                return 201;
            }else if(checkEmail != null)
            {
                return 202;
            }
            else
            {
                return 203;
            }
           
        }
      
    }
}
