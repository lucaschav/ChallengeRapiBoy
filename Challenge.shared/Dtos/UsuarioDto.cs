using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.shared.Dtos
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo email es requerido")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "El campo rol es requerido")]
        public int RolId { get; set; }
        public RolDto? Rol { get; set; }
        public bool Activo { get; set; }
        [MinLength(5,ErrorMessage = "La contraseña debe tener minimo 5 caracteres")]
        public string? PasswordString { get; set; }
        [Compare("PasswordString", ErrorMessage = "Las contraseñas no coinciden")]
        public string? ConfirmPassword { get; set; }

        public string? Token { get; set; }
        public string? RefreshToken { get; set; } = string.Empty;
        public DateTime? TokenCreated { get; set; }
        public DateTime? TokenExpires { get; set; }
    }
}
