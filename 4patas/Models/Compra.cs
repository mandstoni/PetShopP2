using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace _4patas.Models
{
    public class Compra
    {
        public int Id { get; set; }
        public Produto NomeProduto { get; set; }
        [NotMapped]
        public virtual List<SelectListItem> NomeProdutos { get; set; }
        public Usuario NomeUsuario { get; set; }
        [NotMapped]
        public virtual List<SelectListItem> NomeUsuarios { get; set; }
        public Funcionario NomeFuncionario { get; set; }
        [NotMapped]
        public virtual List<SelectListItem> NomeFuncionarios { get; set; }
    }
}
