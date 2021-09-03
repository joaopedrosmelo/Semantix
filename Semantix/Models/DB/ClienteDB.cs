using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Semantix.Models.DB
{
    [Table("Cliente")]
    public class ClienteDB
    {
        [Key]
        public int Cod { get; set; }
        public string Nome { get; set; }
        public List<EnderecoDB> EnderecoDBs { get; set; }
        public List<TelefoneDB> TelefoneDBs { get; set; }
    }
}
