using ImplementCors.Base.Urls;
using Microsoft.AspNetCore.Http;
using NETCore1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ImplementCors.Repositories.Data
{
    public class PersonRepository : GeneralRepository<Person, string>
    {
        private readonly Address address;
        private readonly HttpClient httpClient;
        private readonly string request;
        private readonly IHttpContextAccessor contextAccessor;
        public PersonRepository(Address address, string request = "Person/") : base(address, request)
        {
            this.address = address;
            this.request = request;
            contextAccessor = new HttpContextAccessor();
            httpClient = new HttpClient
            {
                BaseAddress = new Uri(address.link)
            };
        }
        public async Task<List<PersonViewModel>> GetAllProfile()
        {
            List<PersonViewModel> registers = new List<PersonViewModel>();
            using (var response = await httpClient.GetAsync(request + "getPerson"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                registers = JsonConvert.DeserializeObject<List<PersonViewModel>>(apiResponse);
            }
            return registers;
        }

        public async Task<PersonViewModel> GetByNik(string nik)
        {
            PersonViewModel personViewModel = new PersonViewModel();

            using (var response = await httpClient.GetAsync(request + "getperson/" + nik))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                personViewModel = JsonConvert.DeserializeObject<PersonViewModel>(apiResponse);
            }
            return personViewModel;
        }
    }
}
