using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using _4patas.Data;
using _4patas.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace _4patas.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public UsuarioController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;

        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuario.Include(a => a.Animal).ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.Include(a => a.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            var user = new Usuario();
            var animal = _context.Animal.ToList();

            user.Animals = new List<SelectListItem>();

            foreach (var a in animal)
            {
                user.Animals.Add(new SelectListItem { Text = a.NomeAnimal, Value = a.Id.ToString() });
            }
            return View(user);
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Cpf, Description,ImageFile")] Usuario usuario)
        {

            int _animalId = int.Parse(Request.Form["Animl"].ToString());
            var animal = _context.Animal.FirstOrDefault(e => e.Id == _animalId);
            usuario.Animal = animal;

            if (ModelState.IsValid)
            {

                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(usuario.ImageFile.FileName);
                string extension = Path.GetExtension(usuario.ImageFile.FileName);
                usuario.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/image", fileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await usuario.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            var usuario = _context.Usuario.Include(a => a.Animal).First(u => u.Id == id);

            var animal = _context.Animal.ToList();

            usuario.Animals = new List<SelectListItem>();

            foreach (var a in animal)
            {
                usuario.Animals.Add(new SelectListItem { Text = a.NomeAnimal, Value = a.Id.ToString() });
            }

    
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cpf")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            int _animalId = int.Parse(Request.Form["Animal"].ToString());
            var animal = _context.Animal.FirstOrDefault(a => a.Id == _animalId);
            usuario.Animal = animal;

            if (ModelState.IsValid)
            {
                try
                {
                    var productCompare = _context.Usuario.Find(usuario.Id);

                    usuario.Image = (usuario.ImageFile == null) ? "" : usuario.ImageFile.FileName;

                    if (!CompareFileName(productCompare.Image, usuario.Image))
                    {
                        //Remover Imagem anterior
                        var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", productCompare.Image);
                        if (System.IO.File.Exists(imagePath))
                            System.IO.File.Delete(imagePath);

                        //Incluir nova
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(usuario.ImageFile.FileName);
                        string extension = Path.GetExtension(usuario.ImageFile.FileName);
                        usuario.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/image", fileName);

                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await usuario.ImageFile.CopyToAsync(fileStream);
                        }
                    }

                    productCompare.Description = usuario.Description;
                    productCompare.Image = string.IsNullOrEmpty(usuario.Image) ? productCompare.Image : usuario.Image;

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }
        private bool CompareFileName(string name, string newName)
        {
            //Se não foi selecionada uma imagem nova fica a antiga. 
            if (string.IsNullOrEmpty(newName))
                return true;

            if (string.IsNullOrEmpty(name))
                return false;

            //extensão do arquivo
            var validateName = name.Split('.');
            var validateNewName = newName.Split('.');

            if (validateName[1] != validateNewName[1])
                return false;

            //Remover a data gerada
            string nameOld = validateName[0].Replace(validateName[0]
                                            .Substring(validateName[0].Length - 9, 9), "");

            if (newName == nameOld)
                return true;

            return false;
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuario.Include(a => a.Animal)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Usuario.FindAsync(id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", product.Image);

            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            var usuario = await _context.Usuario.FindAsync(id);
            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }
    }
}
