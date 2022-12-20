using BennyBooks.DataAccess.Repository.IRepository;
using BennyBooks.Models;
using BennyBooksWeb.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennyBooks.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        private ApplicationDbContext _db;

        public ShoppingCartRepository(ApplicationDbContext db) : base(db) // Pass db as parameter
        {
            _db = db;
        }

        public int DecrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count -= count;
            return shoppingCart.Count;
        }

        public int IncrementCount(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count += count;
            return shoppingCart.Count;
        }

        // Create IncreamentCountAsync() and DecrementCountAsync()
        public async Task DecrementCountAsync(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count -= count;
            await _db.SaveChangesAsync();
        }

        public async Task IncrementCountAsync(ShoppingCart shoppingCart, int count)
        {
            shoppingCart.Count += count;
            await _db.SaveChangesAsync();
        }

        public Task<int> SaveAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
