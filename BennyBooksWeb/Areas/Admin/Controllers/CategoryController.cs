using BennyBooksWeb.DataAccess;
using BennyBooks.DataAccess.Repository.IRepository;
using BennyBooks.Models;
using Microsoft.AspNetCore.Mvc;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace BennyBooksWeb.Areas.Admin.Controllers;
[Area("Admin")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unityOfWork; // ApplicationDbContext is now hosted in Irepository and reference in Program.cs

    public CategoryController(IUnitOfWork unityOfWork) // Constructor being passed db from Program.cs
    {
        _unityOfWork = unityOfWork;
    }

    public IActionResult Index() // to Add new view, right click this function and Add New View and Select "Razor View"
    {
        IEnumerable<Category> objCategoryList = _unityOfWork.Category.GetAll(); // Grabs all columns in the database, no sql required, look into LINQ            
        return View(objCategoryList);
    }

    // GET
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="category"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category) // Add to the database
    {
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name."); // key for AddModel can be CustomerError or field
        }
        // Checks to make sure the model is valid
        if (ModelState.IsValid && !string.IsNullOrWhiteSpace(category.Name))
        {
            category.Id = Guid.NewGuid();
            await _unityOfWork.Category.AddAsync(category); // Add to the database
            await _unityOfWork.SaveAsync();
            TempData["success"] = "Category created successfully";

            return RedirectToAction(nameof(Index));
        }
        return View(category);

    }

    // MIGHT BE A GOOD IDEA TO CREATE A CLASS TO HANDLE SIMLAR CHECKS LIKE UPDATING THE DATABASE
    // GET
    public IActionResult Edit(Guid? id)
    {
        if (id == null) return NotFound();
        //var categoryFromDb = _db.Categories.Find(id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id);
        var categoryFromDbFirst = _unityOfWork.Category.GetFirstOrDefault(c => c.Id == id);

        if (categoryFromDbFirst == null) return NotFound();

        return View(categoryFromDbFirst);
    }

    // GET View Must be named exactly the same as this get
    public IActionResult Delete(Guid? id)
    {
        if (id == null || _unityOfWork == null) return NotFound();

        var categoryFromDbFirst = _unityOfWork.Category.GetFirstOrDefault(c => c.Id == id);

        if (categoryFromDbFirst == null) return NotFound();

        return View(categoryFromDbFirst);
    }


    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Category category)
    {
        // Validation check making sure fields don't match
        if (category.Name == category.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the Name."); // key for AddModel can be CustomerError or field
        }


        // Checks to make sure the model is valid
        if (ModelState.IsValid && !string.IsNullOrWhiteSpace(category.Name))
        {
            try
            {
                _unityOfWork.Category.Update(category); // Update the database
                await _unityOfWork.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }
        return View(category);

    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(Guid? id)
    {
        var categoryFromDb = _unityOfWork.Category.GetFirstOrDefault(c => c.Id == id); // find category to delete be id
        if (categoryFromDb == null) return NotFound();

        _unityOfWork.Category.Remove(categoryFromDb); // Update the database

        _unityOfWork.SaveAsync();
        TempData["success"] = "Category deleted successfully";

        return RedirectToAction(nameof(Index));
    }

}
