using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Semantix.Models.DB
{
    [Table("Endereco")]
    public class EnderecoDB
    {
        [Key]
        public int Cod { get; set; }
        public string CEP { get; set; }
        public string Numero { get; set; }
        public int Cod_Cliente { get; set; }
        public ClienteDB ClienteDB { get; set; }
    }
}
