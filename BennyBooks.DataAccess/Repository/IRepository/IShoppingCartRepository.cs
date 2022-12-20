using BennyBooks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennyBooks.DataAccess.Repository.IRepository
{

    public interface IShoppingCartRepository : IRepository<ShoppingCart>
    {
        int IncrementCount(ShoppingCart shoppingCart, int count);
        int DecrementCount(ShoppingCart shoppingCart, int count);
        // Create IncreamentCountAsync() and DecrementCountAsync()
        Task IncrementCountAsync(ShoppingCart shoppingCart, int count);
        Task DecrementCountAsync(ShoppingCart shoppingCart, int count);

    }
}
