using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;
using BibliotecaMVC.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BibliotecaMVC.Controllers
{
    [Authorize]
    public class LibrosController : Controller
    {
        private readonly BibliotecaContext _context;
        private readonly ILogger<LibrosController> _logger;

        public LibrosController(BibliotecaContext context, ILogger<LibrosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation("Accediendo a la lista de libros");
            try
            {
                var libros = await _context.Libros.ToListAsync();
                return View(libros);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la lista de libros");
                throw;
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation("Consultando los detalles del libro con ID: {Id}", id);
            try
            {
                var libro = await _context.Libros.FindAsync(id);
                if (libro == null)
                {
                    _logger.LogWarning("Libro con ID: {Id} no encontrado", id);
                    return NotFound("Libro no encontrado");
                }
                return View(libro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener los detalles del libro con ID: {Id}", id);
                throw;
            }
        }

        [Authorize(Roles = "Admin, Bibliotecario")]
        public IActionResult Create()
        {
            _logger.LogInformation("Iniciando la creación de un nuevo libro");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Bibliotecario")]
        public async Task<IActionResult> Create(Libro libro)
        {
            _logger.LogInformation("Creando un nuevo libro");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("El libro enviado no cumple con los requerimientos mínimos");
                return View(libro);
            }

            try
            {
                _context.Libros.Add(libro);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Libro creado correctamente con ID: {Id}", libro.ID);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Se produjo un error al crear el libro");
                throw;
            }
        }

        [Authorize(Roles = "Admin, Bibliotecario")]
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation("Iniciando edición del libro con ID: {Id}", id);
            try
            {
                var libro = await _context.Libros.FindAsync(id);
                if (libro == null)
                {
                    _logger.LogWarning("Libro con ID: {Id} no encontrado para su edición", id);
                    return NotFound("Libro no encontrado");
                }
                return View(libro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el libro con ID: {Id} para su edición", id);
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Bibliotecario")]
        public async Task<IActionResult> Edit(Libro libro)
        {
            _logger.LogInformation("Guardando cambios del libro con ID: {Id}", libro.ID);
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Los cambios enviados para el libro con ID: {Id} no cumplen con los requerimientos mínimos", libro.ID);
                return View(libro);
            }

            try
            {
                _context.Libros.Update(libro);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Libro con ID: {Id} actualizado correctamente", libro.ID);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar el libro con ID: {Id}", libro.ID);
                throw;
            }
        }

        [Authorize(Roles = "Admin, Bibliotecario")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Solicitando confirmación para eliminar el libro con ID: {Id}", id);
            try
            {
                var libro = await _context.Libros.FindAsync(id);
                if (libro == null)
                {
                    _logger.LogWarning("Libro con ID: {Id} no encontrado para su eliminación", id);
                    return NotFound("Libro no encontrado");
                }
                return View(libro);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el libro con ID: {Id} para su eliminación", id);
                throw;
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Bibliotecario")]
        public async Task<IActionResult> DeleteDeVelda(int id)
        {
            _logger.LogInformation("Iniciando eliminación del libro con ID: {Id}", id);
            try
            {
                var libro = await _context.Libros.FindAsync(id);
                if (libro == null)
                {
                    _logger.LogWarning("Libro con ID: {Id} no encontrado al intentar eliminarlo", id);
                    return RedirectToAction(nameof(Index));
                }

                _context.Libros.Remove(libro);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Libro con ID: {Id} eliminado correctamente", id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Se produjo un error al eliminar el libro con ID: {Id}", id);
                throw;
            }
        }
    }
}