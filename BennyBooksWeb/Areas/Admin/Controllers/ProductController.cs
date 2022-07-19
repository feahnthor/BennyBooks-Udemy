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
public class ProductController : Controller
{
    private readonly IUnitOfWork _unityOfWork; // ApplicationDbContext is now hosted in Irepository and reference in Program.cs
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(IUnitOfWork unityOfWork, IWebHostEnvironment hostEnvironment) // Constructor being passed db from Program.cs
    {
        _unityOfWork = unityOfWork;
        _hostEnvironment = hostEnvironment;
    }

    public IActionResult Index() // to Add new view, right click this function and Add New View and Select "Razor View"
    {
        //IEnumerable<Product> objCategoryList = _unityOfWork.Product.GetAll(); // Grabs all columns in the database, no sql required, look into LINQ            
        return View();
    }

    
    
    // UPSERT replaces Create and Edit
    public IActionResult Upsert(Guid? id)
    // Used by product.js, Upsert.cshtml same for post
    {// Unit of work allos us to access all of our controllers
        ProductVM productVM = new() // View now becomes tightly bounded by being managed in the ViewModel
        {
            Product = new(),
            CategoryList = _unityOfWork.Category.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }),
            CoverTypeList = _unityOfWork.CoverType.GetAll().Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }),
        };

        if (id == null && id != Guid.Empty)
        {
            return View(productVM);
        }
        else
        {
            // Will display edit page with all the data for a product
            productVM.Product = _unityOfWork.Product.GetFirstOrDefault(u => u.Id == id);
            return View(productVM);
            // Update product
        }

        
    }


    // POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upsert(ProductVM obj, IFormFile file)
    {
        // Validation check making sure fields don't match
        //if (string.IsNullOrWhiteSpace(obj.Title))
        //{
        //    ModelState.AddModelError("Title", "The Title cannot be made of only whitespace."); // key for AddModel can be CustomerError or field
        //}


        // Checks to make sure the model is valid
        if (ModelState.IsValid )//&& !string.IsNullOrWhiteSpace(obj.Title
        {
            try
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null) // We have an image to add or update Upload
                {
                    string fileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid().ToString(); // makes sure if a similar file name is create, we can replace it
                    string uploads = Path.Combine(wwwRootPath, @"images\products");
                    string extentsion = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null) // an image already exists, we should replace it
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) // remove previous image
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // Copy image to our folder
                    using (Stream fileStreams = new FileStream(Path.Combine(uploads, fileName + extentsion), FileMode.Create))
                    {

                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extentsion;
                }

                if (obj.Product.Id == Guid.Empty) // we are creating as there is no previous product
                {
                    _unityOfWork.Product.Add(obj.Product); // Id is taken from the Upsert.cshtml
                }
                else
                {
                    _unityOfWork.Product.Update(obj.Product);
                }

                TempData["success"] = "Product updated successfully";
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
    [HttpGet] // will get all of the products
    public IActionResult GetAll() //admin/product/getall
    {
        var productLlist = _unityOfWork.Product.GetAll(includeProperties:"Category,CoverType"); // case sensitive for properties
        return Json(new { data = productLlist });
    }

    // POST
    [HttpDelete]
    public IActionResult Delete(Guid? id) // no need for a delete GET
    {
        var obj = _unityOfWork.Product.GetFirstOrDefault(c => c.Id == id); // find obj to delete be id
        if (obj == null)
        {
            return Json(new { success = false, message = "Error while deleting" });
        }

        string oldImagePath = Path.Combine(_hostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
        if (System.IO.File.Exists(oldImagePath)) // remove previous image
        {
            System.IO.File.Delete(oldImagePath);
        }

        _unityOfWork.Product.Remove(obj); // Update the database

        _unityOfWork.SaveAsync();
        return Json(new { success = true, message = "Delete Successful" });
        //TempData["success"] = "obj created successfully";
    }
    #endregion

}
