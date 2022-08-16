using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BennyBooks.Models.ViewModels
{
    public class ProductVM
    {//ViewModel is used to encapsulate the multiple entities into single entity. It is basically a combination
     //of data models into single object and rendering by the view
        public Product Product { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; } // Selects returns all items (a projection),
        [ValidateNever]
        public IEnumerable<SelectListItem> CoverTypeList { get; set; }

        /* This will help make a tightly bound view so there will be no need for a ViewBag or ViewData
         OLD USED IN CONTROLLER
        ----------------------
        Product product = new Product();
        IEnumerable<SelectListItem> CategoryList = _unityOfWork.Category.GetAll().Select( // Selects returns all items (a projection),
            u => new SelectListItem
            {
                Text = u.Name, // Have user select name then we will get have the Id associated with it.
                Value = u.Id.ToString()
            });
        IEnumerable<SelectListItem> CoverTypeList = _unityOfWork.CoverType.GetAll().Select(
            u => new SelectListItem
            {
                Text = u.Name, // Have user select name then we will get have the Id associated with it.
                Value = u.Id.ToString()
            });

            ViewBag.CategoryList = CategoryList;
             Viewbag is from C# 4.0 and allows us to transfer data from our controller to our View(not vice-versa) that wasn't part of the model.
              Values are on temporary and will be set to null during a redirect. It will be automatically be passed
              The first thing after the period can be anything as its an accessor
              Is a wrapper for ViewData, it internally inerts data into ViewData dictionry. So the key of ViewData and property of ViewBag MUST NOT MATCH
             
            ViewData["CoverTypeList"] = CoverTypeList;
            /* ViewData allows for transer of data from controller to our View(not vice-versa) that wasn't part of the model.
             * Is derived from ViewDataDictionary which is a dictionary type
             * ViewData value MUST be type cast before use. Lifespan is only as long as the current http request (exactly the same as Viewbag)
             

            return View(product); 
        -------------------------------------------
        */
    }
}
