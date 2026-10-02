using System.ComponentModel.DataAnnotations;

namespace BibliotecaMVC.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "El username es obligatorio")]
        [Display(Name = "Nombre de usuario")]
        public string Username { get; set; }=string.Empty;

        [Required(ErrorMessage = "El email es obligatorio")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        public string Email { get; set; }=string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }=string.Empty;

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
        [Display(Name = "Confirmar contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
