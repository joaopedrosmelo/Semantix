using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Semantix.Data;
using Semantix.Models;
using Semantix.Models.DB;
using Semantix.Models.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Semantix.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ApplicationDBContext _context;
        private static IMapper _mapper;
        private readonly APICEP _apiCEP;

        public ClientesController(ApplicationDBContext context, IMapper mapper, IOptions<APICEP> apiCEP)
        {
            _context = context;
            _mapper = mapper;
            _apiCEP = apiCEP.Value;
        }
        // GET: ClientesController
        public ActionResult Index()
        {
            var clientes = _mapper.Map<List<ClienteDTO>>(_context.ClienteDBs
                .Include(c => c.EnderecoDBs)
                .Include(c => c.TelefoneDBs)).ToList();

            foreach (var cliente in clientes)
            {
                foreach (var endereco in cliente.EnderecosDTO)
                {
                    var cep = ConsultaCEP(endereco.CEP);
                    endereco.Localidade = cep.localidade;
                    endereco.Logradouro = cep.logradouro;
                    endereco.UF = cep.uf;
                }
            }

            ViewBag.Clientes = clientes;
            return View();
        }
        // GET: ClientesController/Incluir
        public ActionResult Incluir()
        {
            return View("Incluir");
        }

        // GET: ClientesController/Excluir
        public ActionResult Excluir()
        {
            return View("Excluir");
        }

        // POST: ClientesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                var cliente = new ClienteDTO()
                {
                    Nome = collection["Nome"],
                    EnderecosDTO = new List<EnderecoDTO>()
                    {
                    },
                    TelefonesDTO = new List<TelefoneDTO>()
                    {
                    }
                };

                var validaCampos = true;
                var x = 0;
                while (validaCampos)
                {
                    var cep = ConsultaCEP(collection[$"EnderecosDTO[{x}].CEP"]);
                    cliente.EnderecosDTO.Add(new EnderecoDTO()
                    {
                        CEP = cep.cep.Replace("-", "").Trim(),
                        Numero = collection[$"EnderecosDTO[{x}].Numero"]
                    });
                    if(string.IsNullOrEmpty(collection[$"EnderecosDTO[{x+1}].CEP"]))
                    {
                        validaCampos = false;
                    }
                    x++;
                }

                validaCampos = true;
                x = 0;
                while (validaCampos)
                {
                    cliente.TelefonesDTO.Add(new TelefoneDTO()
                    {
                        Numero = collection[$"TelefonesDTO[{x}].Numero"]
                    });
                    if (string.IsNullOrEmpty(collection[$"TelefonesDTO[{x + 1}].Numero"]))
                    {
                        validaCampos = false;
                    }
                    x++;
                }

                _context.Add(_mapper.Map<ClienteDB>(cliente));
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: ClientesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(IFormCollection collection)
        {
            try
            {
                var cliente = _context.ClienteDBs.Include(c => c.EnderecoDBs).Include(c => c.TelefoneDBs).Where(c => c.Cod == int.Parse(collection["Cod"])).FirstOrDefault();
                cliente.Nome = collection["Nome"];
                var x = 0;
                cliente.EnderecoDBs.ForEach(e =>
                {
                    e.CEP = collection[$"EnderecosDTO[{x}].CEP"];
                    e.Numero = collection[$"EnderecosDTO[{x}].Numero"];
                    x++;
                });
                x = 0;
                cliente.TelefoneDBs.ForEach(t =>
                {
                    t.Numero = collection[$"TelefonesDTO[{x}].Numero"];
                    x++;
                });

                _context.Update(cliente);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // POST: ClientesController/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(IFormCollection collection)
        {
            try
            {
                var codCliente = collection["Cod"];
                var cliente = _context.ClienteDBs
                    .Include(c => c.EnderecoDBs)
                    .Include(c => c.TelefoneDBs)
                    .Where(c => c.Cod == int.Parse(codCliente)).FirstOrDefault();
                _context.Remove(cliente);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        private CEP ConsultaCEP(string CEP)
        {
            WebRequest webRequest = WebRequest.Create(_apiCEP.endpoint + CEP + "/" + _apiCEP.formato);

            string responseData = "";
            using (var reader = new StreamReader(webRequest.GetResponse().GetResponseStream()))
            {
                responseData = reader.ReadToEnd();
            }

            var cep = new CEP();
            if (!string.IsNullOrEmpty(responseData))
            {
                var json = JObject.Parse(responseData);
                cep = JsonSerializer.Deserialize<CEP>(json.ToString());
            }

            return cep;
        }
    }
}
