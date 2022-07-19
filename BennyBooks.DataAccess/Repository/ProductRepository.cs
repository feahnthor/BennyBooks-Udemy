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
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db) // Pass db as parameter
        {
            _db = db;
        }

        public Task<int> SaveAsync()
        {
            return _db.SaveChangesAsync();
        }

        public void Update(Product obj)
        {
            // get a copy of the db, better way to only update columns we want instead of the entire row
            var objFromDb = _db.Products.FirstOrDefault(p => p.Id == obj.Id);
            if (objFromDb != null)
            {
                objFromDb.Title = obj.Title;
                objFromDb.Description = obj.Description;
                objFromDb.Author = obj.Author;
                objFromDb.CategoryId = obj.CategoryId;
                objFromDb.CoverTypeId = obj.CoverTypeId;
                objFromDb.Price = obj.Price;
                objFromDb.Price50 = obj.Price50;
                objFromDb.Price100 = obj.Price100;
                objFromDb.PageCount = obj.PageCount;

                if (obj.ImageUrl != null) // Update image
                {
                    objFromDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
