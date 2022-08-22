using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BennyBooks.Models.ViewModels
{
    public class ShoppingCartVM
    {//ViewModel is used to encapsulate the multiple entities into single entity. It is basically a combination
     //of data models into single object and rendering by the view
        public IEnumerable<ShoppingCart> CartList { get; set; } // Selects returns all items (a projection),
        public double CartTotal { get; set; }
        
    }
}
