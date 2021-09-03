using AutoMapper;
using Semantix.Models.DB;
using Semantix.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Semantix.Profiles
{
    public class Profiles : Profile
    {
        public Profiles()
        {
            CreateMap<ClienteDB, ClienteDTO>()
                .ForMember(cd => cd.EnderecosDTO, cdb => cdb.MapFrom(cliente => cliente.EnderecoDBs))
                .ForMember(cd => cd.TelefonesDTO, cdb => cdb.MapFrom(cliente => cliente.TelefoneDBs))
                .ReverseMap();
            CreateMap<EnderecoDB, EnderecoDTO>().ReverseMap();
            CreateMap<TelefoneDB, TelefoneDTO>().ReverseMap();
        }
    }
}
