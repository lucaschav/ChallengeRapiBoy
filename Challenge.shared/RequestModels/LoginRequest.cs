using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge.shared.RequestModels
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "El campo email es requerido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "El campo contraseña es requerido")]
        [MinLength(5, ErrorMessage = "La contraseña debe tener minimo 5 caracteres")]
        public string Password { get; set; }
    }
}
