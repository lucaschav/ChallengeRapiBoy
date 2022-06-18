using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class Rol
    {
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; }
    }
}
