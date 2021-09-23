using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore1.Models
{
    public class PersonViewModel
    {
        [Key]
        public string NIK { get; set; }
        public string NamaDepan { get; set; }
        public string NamaBelakang { get; set; }
        public string Telp{ get; set; }
        public int Education_id { get; set; }
        public string Unversity_Id { get; set; }
        public string Email { get; set; }
        public DateTime TglLahir { get; set; }
        public string Password { get; set; }
        public string Degree{ get; set; }
        public string GPA { get; set; }

    }
}
