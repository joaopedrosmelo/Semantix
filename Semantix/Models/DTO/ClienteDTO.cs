using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Semantix.Models.DTO
{
    public class ClienteDTO
    {
        public int Cod { get; set; }
        public string Nome { get; set; }
        [DisplayName("Endereços")]
        public List<EnderecoDTO> EnderecosDTO { get; set; }
        [DisplayName("Telefones")]
        public List<TelefoneDTO> TelefonesDTO { get; set; }
    }
}
