using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using _4patas.Models;

namespace _4patas.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<_4patas.Models.Produto> Produto { get; set; }
        public DbSet<_4patas.Models.Funcionario> Funcionario { get; set; }
        public DbSet<_4patas.Models.Animal> Animal { get; set; }
        public DbSet<_4patas.Models.Compra> Compra { get; set; }
        public DbSet<_4patas.Models.Usuario> Usuario { get; set; }
    }
}
