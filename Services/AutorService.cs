using BibliotecaMVC.Models;

namespace BibliotecaMVC.Services
{
    // Actividad 2: Implementación de IAutorService.
    // Aquí se traslada la lógica de gestión y consulta de autores que antes
    // vivía directamente dentro de AutoresController.
    public class AutorService : IAutorService
    {
        // Se mantiene 'static' para conservar los mismos datos en memoria entre peticiones,
        // tal como funcionaba antes en el controlador (no hay base de datos en este ejercicio).
        private static List<Autor> _autores = new List<Autor>
        {
            new Autor { Id = 1, Nombre = "Gabriel", Apellido = "García Márquez", Nacionalidad = "Colombiana", FechaNacimiento = new DateTime(1927, 3, 6), Activo = false },
            new Autor { Id = 2, Nombre = "Isabel", Apellido = "Allende", Nacionalidad = "Chilena", FechaNacimiento = new DateTime(1942, 8, 2), Activo = true },
            new Autor { Id = 3, Nombre = "Mario", Apellido = "Vargas Llosa", Nacionalidad = "Peruana", FechaNacimiento = new DateTime(1936, 3, 28), Activo = true }
        };

        public List<Autor> ObtenerAutores()
        {
            return _autores;
        }

        public Autor? ObtenerAutorPorId(int id)
        {
            return _autores.FirstOrDefault(a => a.Id == id);
        }

        public void CrearAutor(Autor autor)
        {
            autor.Activo = true;

            if (_autores.Any())
            {
                autor.Id = _autores.Max(x => x.Id) + 1;
            }
            else
            {
                autor.Id = 1;
            }

            _autores.Add(autor);
        }

        public bool ActualizarAutor(Autor autorEditado)
        {
            var autorExistente = _autores.FirstOrDefault(a => a.Id == autorEditado.Id);
            if (autorExistente == null)
            {
                return false;
            }

            autorExistente.Nombre = autorEditado.Nombre;
            autorExistente.Apellido = autorEditado.Apellido;
            autorExistente.Nacionalidad = autorEditado.Nacionalidad;
            autorExistente.FechaNacimiento = autorEditado.FechaNacimiento;
            autorExistente.Activo = autorEditado.Activo;

            return true;
        }

        public bool EliminarAutor(int id)
        {
            var autor = _autores.FirstOrDefault(a => a.Id == id);
            if (autor == null)
            {
                return false;
            }

            _autores.Remove(autor);
            return true;
        }
    }
}
