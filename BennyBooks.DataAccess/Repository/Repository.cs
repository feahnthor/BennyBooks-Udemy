using BennyBooks.DataAccess.Repository.IRepository;
using BennyBooksWeb.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BennyBooks.DataAccess.Repository
{
    public class Repository<GenericDbObject> : IRepository<GenericDbObject> where GenericDbObject : class // Implements generic repository
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<GenericDbObject> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            // To test out include properites COMMENT THIS OUT
            //_db.Products.Include(u => u.Category).Include(u => u.CoverType);
            this.dbSet = _db.Set<GenericDbObject>(); // get the dbSet and set it to the particular instance it is being called
        }

        public void Add(GenericDbObject entity)
        {
            dbSet.Add(entity);
        }

        public async Task AddAsync(GenericDbObject entity, CancellationToken cancellationToken = default)
        {
            await dbSet.AddAsync(entity, cancellationToken);
        }

        // includeProp - "Category,CoverType"   -- case sensitive
        public IEnumerable<GenericDbObject> GetAll(Expression<Func<GenericDbObject, bool>>? filter = null, string? includeProperties = null) // makes filter nullable
        {
            IQueryable<GenericDbObject> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            
            if (includeProperties != null)
            {
                // Will not brake if there are commas seperating properties, including ,,,
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp); // Include property so that our js files don't break when trying to get data from GetAll() from the API get
                }
            }
            return query.ToList();
        }
        
        public async Task<IEnumerable<GenericDbObject>> GetAllAsync(Expression<Func<GenericDbObject, bool>>? filter = null,
            string? includeProperties = null,
            CancellationToken cancellationToken = default
            )
        {
            IQueryable<GenericDbObject> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                // Will not brake if there are commas seperating properties, including ,,,
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp); // Include property so that our js files don't break when trying to get data from GetAll() from the API get
                }
            }
            cancellationToken.ThrowIfCancellationRequested();
            return await query.ToListAsync(cancellationToken);
        }

        public GenericDbObject GetFirstOrDefault(Expression<Func<GenericDbObject, bool>> filter, string? includeProperties = null)
        {
            IQueryable<GenericDbObject> query = dbSet;
            query = query.Where(filter);
            if (includeProperties != null)
            {
                // Will not brak is there are commas seperating properties, including ,,,
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp); // Include property so that our js files don't break when trying to get data from GetAll() from the API get
                }
            }
            return query.FirstOrDefault(); // might return null
        }

        public Task<GenericDbObject> GetFirstOrDefaultAsync(Expression<Func<GenericDbObject, bool>> filter, 
            string? includeProperties = null,
            CancellationToken cancellationToken = default)
        {
            IQueryable<GenericDbObject> query = dbSet;
            query = query.Where(filter);
            if (includeProperties != null)
            {
                // Will not brak is there are commas seperating properties, including ,,,
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp); // Include property so that our js files don't break when trying to get data from GetAll() from the API get
                }
            }
            
            cancellationToken.ThrowIfCancellationRequested(); // will cancell this task
            return  query.FirstOrDefaultAsync(cancellationToken); // might return null
        }

        public void Remove(GenericDbObject entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<GenericDbObject> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
