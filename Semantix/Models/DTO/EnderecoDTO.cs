using System.ComponentModel;

namespace Semantix.Models.DTO
{
    public class EnderecoDTO
    {
        public int Cod { get; set; }
        public string CEP { get; set; }
        public string Numero { get; set; }
        public string Logradouro { get; set; }
        public string Localidade { get; set; }
        public string UF { get; set; }
    }
}