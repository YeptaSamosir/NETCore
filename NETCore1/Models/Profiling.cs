using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NETCore1.Models
{
    [Table("tb_m_profilings")]

    public class Profiling
    {
        [Key]
        public string NIK { get; set; }
        [Required]
        public int Education_id { get; set; }
        [JsonIgnore]
        public virtual Education Education { get; set; }
        [JsonIgnore]
        public virtual LoginVm Account { get; set; }

    }
}