using BibliotecaMVC.Models;

namespace BibliotecaMVC.Services
{
    // Actividad 1: Interfaz que define las operaciones disponibles para trabajar con Autores.
    // El controlador dependerá de esta abstracción y no de la implementación concreta (DIP - SOLID).
    public interface IAutorService
    {
        // Obtiene el listado completo de autores
        List<Autor> ObtenerAutores();

        // Obtiene un autor a partir de su ID (null si no existe)
        Autor? ObtenerAutorPorId(int id);

        // Crea un nuevo autor y le asigna un nuevo ID
        void CrearAutor(Autor autor);

        // Actualiza los datos de un autor existente. Devuelve false si no existe.
        bool ActualizarAutor(Autor autorEditado);

        // Elimina un autor por su ID. Devuelve false si no existe.
        bool EliminarAutor(int id);
    }
}
