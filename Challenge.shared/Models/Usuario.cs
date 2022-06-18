using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Models
{
    public class Usuario
    {
        //[Key]
        public int Id { get; set; }
        public string Email { get; set; }
        //[ForeignKey("FK_Usuario_Rol_Id")]
        public int RolId { get; set; }

        //[NotMapped]
        public Rol Rol { get; set; }
        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool Activo { get; set; }
    }
}
