using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _4patas.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string NomeAnimal { get; set; }
        public string tipoAnimal { get; set; }
    }
}
