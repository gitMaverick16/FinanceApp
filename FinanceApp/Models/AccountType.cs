using FinanceApp.Validations;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Models
{
    public class AccountType //: IValidatableObject
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El campo nombre es requerido")]
        [StringLength(maximumLength:50, MinimumLength =3, ErrorMessage ="La longitud del campo Nombre debe estar entre {2} y {1}")]
        [Display(Name = "Nombre del tipo cuenta")]
        //[FirstLetterUpperCase]
        public string Name { get; set; }
        public int UsuarId { get; set; }
        public int Order { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Name != null && Name.Length > 0)
        //    {
        //        var fistLetter = Name[0].ToString();
        //        if (fistLetter != fistLetter.ToUpper())
        //        {
        //            yield return new ValidationResult("La primera letra debe ser mayuscula",
        //                new[] { nameof(Name) });
        //        }
        //    }
        //}

        /*Test of other validations*/
        //[Required(ErrorMessage ="El campo {0} es requerido")]
        //[EmailAddress(ErrorMessage ="El campo debe ser un correo electronico valido")]
        //public string Email{ get; set; }
        //[Range(minimum:18, maximum:130, ErrorMessage ="El valor debe estar entre {1} y {2}")]
        //public int Age { get; set; }
        //[Url(ErrorMessage ="El campo debe ser una URL valida")]
        //public string URL { get; set; }
        //[CreditCard(ErrorMessage ="La tarjeta de credito no es valida")]
        //public string CreditCard { get; set; }
    }
}
