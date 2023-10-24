using FinanceApp.Validations;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models
{
    public class Account
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo nombre es requerido")]
        [StringLength(maximumLength: 50)]
        [FirstLetterUpperCase]
        public string Name { get; set; }
        [Display(Name="Tipo cuenta")]
        public int AccountTypeId { get; set; }
        public decimal Balance { get; set; }
        [StringLength(maximumLength: 1000)]
        public string Description { get; set; }
        public string AccountType { get; set; }
    }
}
