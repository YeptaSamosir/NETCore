using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NETCore1.Models
{
    [Table("tb_m_accounts")]

    public class LoginVm
    {
        [Key]
        public string NIK { get; set; }
        [Required]
        public string Password { get; set; }
        [JsonIgnore]
        public virtual Person Person { get; set; }
        public virtual Profiling Profiling { get; set; }
        public virtual ICollection<RoleAccount> RoleAccounts { get; set; }



    }

}
