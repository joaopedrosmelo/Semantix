using System.ComponentModel;

namespace Semantix.Models.DTO
{
    public class TelefoneDTO
    {
        public int Cod { get; set; }
        [DisplayName("Telefone")]
        public string Numero { get; set; }
    }
}