using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.shared.RequestModels
{
    public class RegisterRequest
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo email es requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo rol es requerido")]
        public int RolId { get; set; }
        public bool Activo { get; set; }
        [Required(ErrorMessage = "El campo contraseña es requerido")]
        [MinLength(5, ErrorMessage = "La contraseña debe contener al menos 5 caracteres")]
        public string? PasswordString { get; set; }
        [Compare("PasswordString", ErrorMessage = "Las contraseñas no coinciden")]
        public string? ConfirmPassword { get; set; }

    }
}
