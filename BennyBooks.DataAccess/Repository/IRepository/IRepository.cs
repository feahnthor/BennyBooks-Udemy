using System.Linq.Expressions;

namespace BennyBooks.DataAccess.Repository.IRepository
{
    /// <summary>
    /// General class following the Repositor pattern to have a central place to pull from the database
    /// Update Will not be consistent throughout all generic repositories as there may
    ///     be times where it might update images or something else. It will not be included here
    /// </summary>
    /// <typeparam name="GenericDbObject">Any class</typeparam>
    public interface IRepository<GenericDbObject> where GenericDbObject : class // Generic repository where T is a class
    {
        // T - Category, not just limited to that, any Controller should be able to call this to get a table
        GenericDbObject GetFirstOrDefault(Expression<Func<GenericDbObject, bool>> filter, string? includeProperties = null); // Similar to FirstOrDefaultAsync(c => c.Id == id); //
        //Look at 114 of Udemy video  https://www.udemy.com/course/complete-aspnet-core-21-course/learn/lecture/29333122#overview
        IEnumerable<GenericDbObject> GetAll(Expression<Func<GenericDbObject, bool>>? filter = null, string? includeProperties = null); // Just like CategoryController.Index(), grab all types from the database, can be null
        void Add(GenericDbObject entity); // Add to the databse, takes in a object
        void Remove(GenericDbObject entity);
        void RemoveRange(IEnumerable<GenericDbObject> entity); // Remove multiple things
    }
}
