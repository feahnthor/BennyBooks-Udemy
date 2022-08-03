﻿using BennyBooks.DataAccess.Repository.IRepository;
using BennyBooksWeb.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennyBooks.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db); // Needs to be named the same as IUnitOfWork to avoid errors
            CoverType = new CoverTypeRepository(_db);
            Product = new ProductRepository(_db);
            Company = new CompanyRepository(_db);
        }
        public ICategoryRepository Category { get; private set; } // 
        public ICoverTypeRepository CoverType { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; private set; }


        public Task<int> SaveAsync()
        {
            return _db.SaveChangesAsync();
        }
    }
}
