using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace _4patas.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public virtual Animal Animal { get; set; }
        [NotMapped]
        public virtual List<SelectListItem> Animals { get; set; }
        public string Description { get; set; }

        [DisplayName("Nome da Imagem")]
        public string Image { get; set; }

        [NotMapped]
        [DisplayName("Imagem")]
        public IFormFile ImageFile { get; set; }
    }
}
