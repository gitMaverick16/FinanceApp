using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [StringLength(maximumLength:50, ErrorMessage = "No puede ser mayor a {1} caracteres")]
        public string Name { get; set; }
        [Display(Name = "Tipo Operacion")]
        public OperationType OperationTypeId { get; set; }
        public int UserId { get; set; }
    }
}
