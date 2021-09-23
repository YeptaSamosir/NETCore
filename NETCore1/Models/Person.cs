using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NETCore1.Models
{
    [Table("tb_m_persons")]
    public class Person
    {
        [Key]
        public string NIK { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Lastname { get; set; }
        [Phone]
        public string Phone { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        public DateTime BirthDate { get; set; }
        [JsonIgnore]
        public virtual LoginVm Account { get; set; }





    }
}
