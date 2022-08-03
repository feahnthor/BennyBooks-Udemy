using BennyBooksWeb.DataAccess;
using BennyBooks.DataAccess.Repository.IRepository;
using BennyBooks.Models;
using Microsoft.AspNetCore.Mvc;
using log4net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using BennyBooks.Models.ViewModels;

namespace BennyBooksWeb.Areas.Admin.Controllers;
[Area("Admin")]
public class CompanyController : Controller
{
    private readonly IUnitOfWork _unityOfWork; // ApplicationDbContext is now hosted in Irepository and reference in Program.cs

    public CompanyController(IUnitOfWork unityOfWork) // Constructor being passed db from Program.cs
    {
        _unityOfWork = unityOfWork;
    }

    public IActionResult Index() // to Add new view, right click this function and Add New View and Select "Razor View"
    {        
        return View();
    }

    
    
    // UPSERT replaces Create and Edit
    public IActionResult Upsert(Guid? id)
    // Used by company.js, Upsert.cshtml same for post
    {
        Company company = new Company(); // View now becomes tightly bounded by being managed in the ViewModel

        if (id == null || id == Guid.Empty)
        {
            return View(company);
        }
        else
        {
            // Will display edit page with all the data for a product
            company = _unityOfWork.Company.GetFirstOrDefault(u => u.Id == id); // Grab item and data
            return View(company);
        }

        
    }


    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(ProductVM obj)
    {
        // Checks to make sure the model is valid
        if (ModelState.IsValid )//&& !string.IsNullOrWhiteSpace(obj.Title
        {
            try
            {
                if (obj.Product.Id == Guid.Empty) // we are creating as there is no previous product
                {
                    _unityOfWork.Product.Add(obj.Product); // Id is taken from the Upsert.cshtml
                    TempData["success"] = "Company created successfully";
                }
                else
                {
                    _unityOfWork.Product.Update(obj.Product);
                    TempData["success"] = "Company updated successfully";
                }
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



    #region API CALLS
    [HttpGet] // will get all of companies
    public IActionResult GetAll() //admin/company/getall
    {
        var companyLlist = _unityOfWork.Company.GetAll();
        return Json(new { data = companyLlist });
    }

    // POST
    [HttpDelete]
    public IActionResult Delete(Guid? id) // no need for a delete GET
    {
        var obj = _unityOfWork.Company.GetFirstOrDefault(c => c.Id == id); // find obj to delete be id
        if (obj == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }

        _unityOfWork.Company.Remove(obj); // Update the database

        _unityOfWork.SaveAsync();
        return Json(new { success = true, message = "Delete Successful" });
    }
    #endregion

}
