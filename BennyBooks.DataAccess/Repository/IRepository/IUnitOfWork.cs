using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennyBooks.DataAccess.Repository.IRepository
{
    /// <summary>
    /// Holds all our interfaces and makes it so that we can access our methods from the repository while in our controller
    /// </summary>
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        ICoverTypeRepository CoverType { get; }
        IProductRepository Product { get; }
        ICompanyRepository Company { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        Task<int> SaveAsync();
    }
}
