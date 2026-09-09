using Microsoft.AspNetCore.Mvc;
using BibliotecaMVC.Models;
using BibliotecaMVC.Services;

namespace BibliotecaMVC.Controllers
{
    public class AutoresController : Controller
    {
        // Actividad 4: el controlador ya no crea la instancia directamente,
        // sino que depende de la abstracción IAutorService (Inversión de Control).
        private readonly IAutorService _autorService;

        // Inyección de Dependencias vía constructor: ASP.NET Core resuelve IAutorService
        // usando lo registrado en Program.cs.
        public AutoresController(IAutorService autorService)
        {
            _autorService = autorService;
        }

        public IActionResult Index()
        {
            var autores = _autorService.ObtenerAutores();
            return View(autores);
        }

        public IActionResult Details(int id)
        {
            var autor = _autorService.ObtenerAutorPorId(id);
            if (autor == null)
            {
                return NotFound("Autor no encontrado");
            }
            return View(autor);
        }

        public IActionResult Edit(int id)
        {
            var autor = _autorService.ObtenerAutorPorId(id);
            if (autor == null)
            {
                return NotFound("Autor no encontrado");
            }
            return View(autor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Autor autor)
        {
            if (!ModelState.IsValid)
            {
                return View(autor);
            }

            var actualizado = _autorService.ActualizarAutor(autor);
            if (!actualizado)
            {
                return NotFound("Autor no encontrado");
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Autor autor)
        {
            if (!ModelState.IsValid)
            {
                return View(autor);
            }

            _autorService.CrearAutor(autor);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var autor = _autorService.ObtenerAutorPorId(id);
            if (autor == null)
            {
                return NotFound("Autor no encontrado");
            }
            return View(autor);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteDeVelda(int id)
        {
            _autorService.EliminarAutor(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
