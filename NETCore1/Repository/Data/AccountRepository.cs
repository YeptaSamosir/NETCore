using Microsoft.EntityFrameworkCore;
using NETCore1.Context;
using NETCore1.Hash;
using NETCore1.Models;
using NETCore1.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NETCore1.Repository.Data
{
    public class AccountRepository : GeneralRepository<MyContext, LoginVm, string>
    {
        private readonly MyContext myContext;
        public AccountRepository (MyContext myContext) : base(myContext)
        {
            this.myContext = myContext;
        }


        public int Login(LoginModel loginVM)
        {
            var emailCheck = myContext.Persons.SingleOrDefault(x => x.Email.Equals(loginVM.Email));
            
            if (emailCheck == null)
            {
                return 3;
            }
            else
            {
                var passwordCheck = myContext.Accounts.SingleOrDefault(x => x.NIK.Equals(emailCheck.NIK));

                if (emailCheck != null)
                {
                    if (Hashing.ValidatePassword(loginVM.Password, passwordCheck.Password))
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 2;
                }
            }

        }
        public string GetEmail(string email)
        {
            //var login = (from p in myContext.Persons where p.Email == email select new PersonViewModel { NIK = p.NIK }).ToList();
            var checkEmail = myContext.Persons.Where(x => x.Email.Equals(email)).FirstOrDefault();

            if (checkEmail == null)
            {
                return null;
            }
            else
            {
                return checkEmail.NIK;
            }
        }
        public void ForgotPassword(ForgotPassword forgetPassword)
        {
            var emailCheck = myContext.Persons.Where(x
                => x.Email.Equals(forgetPassword.Email)).FirstOrDefault();

            //if email exist
            if (emailCheck != null)
            {
                // generate guid
                string guid = Guid.NewGuid().ToString();
                string stringHtmlMessage = $"Password Baru Anda: {guid}";
                string hashPW = Hashing.HashPassword(guid);
                // update database
                var checkEmail = myContext.Accounts.Where(e => e.NIK == emailCheck.NIK).FirstOrDefault();
                checkEmail.Password = hashPW;
                Update(checkEmail);

                Email(stringHtmlMessage, forgetPassword.Email);
            }
            else
            {

            }
        }


        public static void Email(string stringHtmlMessage, string destinationEmail)
        {

            MailMessage message = new MailMessage();
            SmtpClient client = new SmtpClient();
            message.From = new MailAddress("lapungproject@gmail.com");
            message.To.Add(new MailAddress(destinationEmail));
            message.Subject = "Reset Password";
            message.IsBodyHtml = true;
            message.Body = stringHtmlMessage;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("lapungproject@gmail.com", "lapungproject19");
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Send(message);
        }


        public int ChangePassword(ChangePassword changePassword)
        {

            try
            {
                var emailCheck = myContext.Persons.SingleOrDefault(x => x.Email.Equals(changePassword.Email));
                if (emailCheck == null)
                {
                    return 3;
                }
                else
                {
                    var passwordCheck = myContext.Accounts.SingleOrDefault(x => x.NIK.Equals(emailCheck.NIK));

                    if (emailCheck != null)
                    {
                        if (Hashing.ValidatePassword(changePassword.CurrentPassword, passwordCheck.Password))
                        {
                            var account = myContext.Accounts.Where(n => n.NIK == emailCheck.NIK).FirstOrDefault();
                            account.Password = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);
                            Update(account);
                            myContext.SaveChanges();
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return 2;
                    }
                }
            }
            catch
            {
                throw new Exception();
            }
           
        }
        public string[] GetRole(string nnik)
        {
           
            var getRole = (from p in myContext.Accounts
                           where p.NIK == nnik
                           join a in myContext.RoleAccounts
                           on p.NIK equals a.Nik
                           join r in myContext.Roles
                           on a.Role_id equals r.Id
                           select new Role
                           {
                               RoleName = r.RoleName
                           }).ToList();
            string[] roles = new string[getRole.Count];

            for (int i = 0; i < getRole.Count; i++)
            {
                    roles[i] = getRole[i].RoleName;    
                
            }

            return roles;

        }

    }

}

