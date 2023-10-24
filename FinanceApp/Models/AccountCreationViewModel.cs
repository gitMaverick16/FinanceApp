using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinanceApp.Models
{
    public class AccountCreationViewModel : Account
    {
        public IEnumerable<SelectListItem> AccountTypes {get; set;}
    }
}
