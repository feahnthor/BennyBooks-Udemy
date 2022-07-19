using BennyBooksWeb.DataAccess;
using BennyBooks.DataAccess.Repository.IRepository;
using BennyBooks.Models;
using Microsoft.AspNetCore.Mvc;
using log4net;
using Microsoft.EntityFrameworkCore;

namespace BennyBooksWeb.Areas.Admin.Controllers;
[Area("Admin")]
public class CoverTypeController : Controller
{
    private readonly IUnitOfWork _unityOfWork; // ApplicationDbContext is now hosted in Irepository and reference in Program.cs

    public CoverTypeController(IUnitOfWork unityOfWork) // Constructor being passed db from Program.cs
    {
        _unityOfWork = unityOfWork;
    }

    public IActionResult Index() // to Add new view, right click this function and Add New View and Select "Razor View"
    {
        IEnumerable<CoverType> objCategoryList = _unityOfWork.CoverType.GetAll(); // Grabs all columns in the database, no sql required, look into LINQ            
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
    /// <param name="obj"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CoverType obj) // Add to the database
    {
        if (string.IsNullOrWhiteSpace(obj.Name))
        {
            ModelState.AddModelError("name", "The Name cannot be made of only whitespace."); // key for AddModel can be CustomerError or field
        }
        // Checks to make sure the model is valid
        if (ModelState.IsValid && !string.IsNullOrWhiteSpace(obj.Name))
        {
            obj.Id = Guid.NewGuid();
            _unityOfWork.CoverType.Add(obj); // Add to the database
            await _unityOfWork.SaveAsync();
            TempData["success"] = "obj created successfully";

            return RedirectToAction(nameof(Index));
        }
        return View(obj);

    }

    // MIGHT BE A GOOD IDEA TO CREATE A CLASS TO HANDLE SIMLAR CHECKS LIKE UPDATING THE DATABASE
    // GET
    public IActionResult Edit(Guid? id)
    {
        if (id == null) return NotFound();
        //var categoryFromDb = _db.Categories.Find(id);
        //var categoryFromDbSingle = _db.Categories.SingleOrDefault(c => c.Id == id);
        var categoryFromDbFirst = _unityOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);

        if (categoryFromDbFirst == null) return NotFound();

        return View(categoryFromDbFirst);
    }

    // GET View Must be named exactly the same as this get
    public IActionResult Delete(Guid? id)
    {
        if (id == null || _unityOfWork == null) return NotFound();

        var categoryFromDbFirst = _unityOfWork.CoverType.GetFirstOrDefault(c => c.Id == id);

        if (categoryFromDbFirst == null) return NotFound();

        return View(categoryFromDbFirst);
    }


    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CoverType obj)
    {
        // Validation check making sure fields don't match
        if (string.IsNullOrWhiteSpace(obj.Name))
        {
            ModelState.AddModelError("name", "The Name cannot be made of only whitespace."); // key for AddModel can be CustomerError or field
        }


        // Checks to make sure the model is valid
        if (ModelState.IsValid && !string.IsNullOrWhiteSpace(obj.Name))
        {
            try
            {
                _unityOfWork.CoverType.Update(obj); // Update the database
                await _unityOfWork.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return RedirectToAction(nameof(Index));
        }
        return View(obj);

    }

    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeletePOST(Guid? id)
    {
        var categoryFromDb = _unityOfWork.CoverType.GetFirstOrDefault(c => c.Id == id); // find obj to delete be id
        if (categoryFromDb == null) return NotFound();

        _unityOfWork.CoverType.Remove(categoryFromDb); // Update the database

        _unityOfWork.SaveAsync();
        TempData["success"] = "obj created successfully";

        return RedirectToAction(nameof(Index));
    }

}
