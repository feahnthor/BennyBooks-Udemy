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
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        private ApplicationDbContext _db;

        public CompanyRepository(ApplicationDbContext db) : base(db) // Pass db as parameter
        {
            _db = db;
        }

        public Task<int> SaveAsync()
        {
            return _db.SaveChangesAsync();
        }

        public void Update(Company obj)
        {
            _db.Companies.Update(obj);
        }
    }
}
