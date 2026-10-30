using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;
using Microsoft.AspNetCore.Authorization;
using BibliotecaMVC.Services;
using BibliotecaMVC.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BibliotecaMVC.Controllers
{
    [Authorize]
    public class AutoresController : Controller
    {
        private readonly BibliotecaContext _context;
        private readonly ILogger<AutoresController> _logger;

        public AutoresController(BibliotecaContext context, ILogger<AutoresController> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Accediendo a la lista de autores");
            try
            {
                var autores = await _context.Autores.ToListAsync();
                return View(autores);
            }
            catch (Exception ex)
            { 
                _logger.LogError(ex, "Error al obtener la lista de autores");
                throw;
            }
            
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Consultando los detalles del autor con ID: {Id}", id);

            try
            {
                var autor = await _context.Autores.FindAsync(id);
                if (autor == null)
                {
                    _logger.LogWarning("Autor con ID: {Id} no encontrado", id);
                    return NotFound("Autor no encontrado");
                }
                return View(autor);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error al obtener los detalles del autor con ID: {Id}", id);
                throw;
            }
        }

        [Authorize(Roles = "Admin, Bibliotecario")]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Iniciando edición del autor con ID: {Id}", id);
            try
            {
                var autor = await _context.Autores.FindAsync(id);
                if (autor == null)
                {
                    return NotFound("Autor no encontrado");
                }
                return View(autor);
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error al obtener los detalles para su edicion del autor con ID: {Id}", id);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Bibliotecario")]
        public async Task<IActionResult> Edit(int id, Autor autor)
        {
            _logger.LogInformation("Iniciando edición del autor con ID: {Id}", id);
            if (id != autor.Id)
            {
                return BadRequest("ID del autor no coincide");
            }

            if (!ModelState.IsValid)
            {
                return View(autor);
            }
            try
            {
                var exists = await _context.Autores.AnyAsync(a => a.Id == id);
                if (!exists)
                {
                    _logger.LogWarning("Autor con ID: {Id} no encontrado para su edicion", id);
                    return NotFound("Autor no encontrado");
                }
                _context.Update(autor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar el autor con ID: {Id}", id);
                throw;
            }
        }

        [Authorize(Roles = "Admin, Bibliotecario")]
        public IActionResult Create()
        {
            _logger.LogInformation("Iniciando la creación de un nuevo autor");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Bibliotecario")]
        public async Task<IActionResult> Create(Autor autor)
        {
            _logger.LogInformation("Creando un nuevo autor");
            if (!ModelState.IsValid)
            {
                _logger.LogError("El autor enviado  no cumple con los requerimientos minimos");
                return View(autor);
            }

            try
            {
                _context.Autores.Add(autor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Se produjo un error al crear el autor");
                throw;
            }

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Iniciando eliminación del autor con ID: {Id}", id);
            try
            {
                var autor = await _context.Autores.FindAsync(id);
                if (autor == null)
                {
                    _logger.LogWarning("Autor con ID: {Id} no encontrado para su eliminación", id);
                    return NotFound("Autor no encontrado");
                }
                _context.Autores.Remove(autor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Se produjo un error al eliminar el autor con ID: {Id}", id);
                throw;
            }
        }
    }
}
