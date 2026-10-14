using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;
using BibliotecaMVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BibliotecaMVC.Controllers
{
    [Authorize]
    public class LibrosController : Controller
    {

        private readonly BibliotecaContext _context;

        public LibrosController(BibliotecaContext context)
        {
            _context = context;
        }

        // Consultar: cualquier usuario autenticado (Admin o Usuario)
        public async Task<IActionResult> Index()
        {
            var libros = await _context.Libros.ToListAsync();
            return View(libros);
        }

        public async Task<IActionResult> Details(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound("Libro no encontrado");
            }
            return View(libro);
        }

        // Agregar: solo Admin
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Libro libro)
        {
            if (!ModelState.IsValid)
            {
                return View(libro);
            }

            _context.Libros.Add(libro);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Editar: solo Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound("Libro no encontrado");
            }
            return View(libro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(Libro libro)
        {
            if (!ModelState.IsValid)
            {
                return View(libro);
            }

            _context.Libros.Update(libro);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Eliminar: solo Admin
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound("Libro no encontrado");
            }
            return View(libro);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDeVelda(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}