using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.Extensions.Configuration;
using Semantix.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Semantix.Data
{
    public class ApplicationDBContext : DbContext
    {
        public IConfiguration Configuration { get; }
        public DbSet<ClienteDB> ClienteDBs { get; set; }
        public DbSet<EnderecoDB> EnderecoDBs { get; set; }
        public DbSet<TelefoneDB> TelefoneDBs { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnderecoDB>()
                .HasOne(c => c.ClienteDB)
                .WithMany(e => e.EnderecoDBs)
                .HasForeignKey(c => c.Cod_Cliente);

            modelBuilder.Entity<TelefoneDB>()
                .HasOne(c => c.ClienteDB)
                .WithMany(e => e.TelefoneDBs)
                .HasForeignKey(c => c.Cod_Cliente);
        }
    }
}
