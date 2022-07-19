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
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db) : base(db) // Pass db as parameter
        {
            _db = db;
        }

        public Task<int> SaveAsync()
        {
            return _db.SaveChangesAsync();
        }

        public void Update(Category obj)
        {
            _db.Categories.Update(obj);
        }
    }
}
