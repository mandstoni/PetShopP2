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

namespace _4patas.Controllers
{
    [Authorize]
    public class CompraController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompraController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Compras
        public async Task<IActionResult> Index()
        {
            return View(await _context.Compra.Include(p => p.NomeProduto).
                Include(f => f.NomeFuncionario).Include(u => u.NomeUsuario)
                .ToListAsync());
        }

        // GET: Compras/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.Include(p => p.NomeProduto).
                Include(f => f.NomeFuncionario).Include(u => u.NomeUsuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // GET: Compras/Create
        public IActionResult Create()
        {
            var compra = new Compra();
            var user = _context.Usuario.ToList();
            var fun = _context.Funcionario.ToList();
            var prod = _context.Produto.ToList();

            compra.NomeUsuarios = new List<SelectListItem>();
            compra.NomeProdutos = new List<SelectListItem>(); 
            compra.NomeFuncionarios = new List<SelectListItem>();

            foreach (var u in user)
            {
                compra.NomeUsuarios.Add(new SelectListItem { Text = u.Nome, Value = u.Id.ToString() });
            }
            foreach (var f in fun)
            {
                compra.NomeFuncionarios.Add(new SelectListItem { Text = f.Nome, Value = f.Id.ToString() });
            }
            foreach (var p in prod)
            {
                compra.NomeProdutos.Add(new SelectListItem { Text = p.DescricaoProduto, Value = p.Id.ToString() });
            }

            return View(compra);
        }

        // POST: Compras/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id")] Compra compra)
        {
            int _userId = int.Parse(Request.Form["Usuario"].ToString());
            var usuario = _context.Usuario.FirstOrDefault(e => e.Id == _userId);
            compra.NomeUsuario = usuario;
            
            int _funId = int.Parse(Request.Form["Funcionario"].ToString());
            var funcionario = _context.Funcionario.FirstOrDefault(e => e.Id == _funId);
            compra.NomeFuncionario = funcionario;

            int _prodId = int.Parse(Request.Form["Produto"].ToString());
            var produto = _context.Produto.FirstOrDefault(e => e.Id == _prodId);
            compra.NomeProduto = produto;

            if (ModelState.IsValid)
            {
                _context.Add(compra);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(compra);
        }

        // GET: Compras/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = _context.Compra.Include(a => a.NomeUsuario).Include(a => a.NomeProduto).
                Include(a => a.NomeFuncionario).First(u => u.Id == id);

            var user = _context.Usuario.ToList();
            var prod = _context.Produto.ToList();
            var fun = _context.Funcionario.ToList();

            compra.NomeFuncionarios = new List<SelectListItem>();
            compra.NomeProdutos = new List<SelectListItem>();
            compra.NomeUsuarios = new List<SelectListItem>();
            
            foreach (var u in user)
            {
                compra.NomeUsuarios.Add(new SelectListItem { Text = u.Nome, Value = u.Id.ToString() });
            }
            foreach (var f in fun)
            {
                compra.NomeFuncionarios.Add(new SelectListItem { Text = f.Nome, Value = f.Id.ToString() });
            }
            foreach (var p in prod)
            {
                compra.NomeProdutos.Add(new SelectListItem { Text = p.DescricaoProduto, Value = p.Id.ToString() });
            }

            if (compra == null)
            {
                return NotFound();
            }
            return View(compra);
        }

        // POST: Compras/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id")] Compra compra)
        {
            if (id != compra.Id)
            {
                return NotFound();
            }

            int _userId = int.Parse(Request.Form["Usuario"].ToString());
            var usuario = _context.Usuario.FirstOrDefault(e => e.Id == _userId);
            compra.NomeUsuario = usuario;

            int _funId = int.Parse(Request.Form["Funcionario"].ToString());
            var funcionario = _context.Funcionario.FirstOrDefault(e => e.Id == _funId);
            compra.NomeFuncionario = funcionario;

            int _prodId = int.Parse(Request.Form["Produto"].ToString());
            var produto = _context.Produto.FirstOrDefault(e => e.Id == _prodId);
            compra.NomeProduto = produto;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(compra);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompraExists(compra.Id))
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
            return View(compra);
        }

        // GET: Compras/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var compra = await _context.Compra.Include(p => p.NomeProduto).
                Include(f => f.NomeFuncionario).Include(u => u.NomeUsuario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (compra == null)
            {
                return NotFound();
            }

            return View(compra);
        }

        // POST: Compras/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var compra = await _context.Compra.FindAsync(id);
            _context.Compra.Remove(compra);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompraExists(int id)
        {
            return _context.Compra.Any(e => e.Id == id);
        }
    }
}
