using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NETCore1.Models
{
    [Table("tb_tr_role_accounts")]
    public class RoleAccount
    {
        [Key]
        public int Id{ get; set; }
        public string Nik { get; set; }
        public int Role_id { get; set; }
        public virtual LoginVm Account{ get; set; }
        public virtual Role Role{ get; set; }




    }
}
