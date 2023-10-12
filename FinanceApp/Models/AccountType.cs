using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models
{
    public class AccountType
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo nombre es requerido")]
        [StringLength(maximumLength:50, MinimumLength =3, ErrorMessage ="La longitud del campo Nombre debe estar entre {2} y {1}")]
        public string Name { get; set; }
        public int UsuarId { get; set; }
        public int Order { get; set; }
    }
}
