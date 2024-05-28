using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Introduction_HomeTask.Models;
using Introduction_HomeTask.Services;
using Introduction_HomeTask.ViewModels;
using System.Collections;
using Humanizer.Bytes;

namespace Introduction_HomeTask.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(IUnitOfWork unitOfWork, ILogger<CategoriesController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogInformation("Categories controller - Index");
            var listOfCategories  = _unitOfWork.Categories.GetAll();
            return View(listOfCategories);
        }

        //[Produces("image/jpeg", "application/json")]
        public IActionResult GetCategoryImage(int id) 
        {
            byte[] imageData = _unitOfWork.Categories.GetById(id).Picture;

            string contentType = "image/jpeg";

            if (imageData is null) 
            {
                return NotFound();
            }
            return File(imageData, contentType);
        }

        public string GetCategoryImageAsSrc(int id)
        {
            byte[] imageData = _unitOfWork.Categories.GetById(id).Picture;

            string imreBase64Data = Convert.ToBase64String(imageData);
            string imgDataURL = string.Format("data:image/png;base64,{0}", imreBase64Data);
            return imgDataURL;
        }


        public IActionResult EditPicture(int? id) 
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _unitOfWork.Categories.GetById((int)id);
            if (category == null)
            {
                return NotFound();
            }
            return View(new CategoryPictureViewModel { Id = category.CategoryId});
        }

        [HttpPost]
        public async Task<ActionResult> EditPictureAsync(CategoryPictureViewModel categoryPicture)
        {
            if (categoryPicture.File != null )
            {
                using (var dataStream = new MemoryStream())
                {
                    await categoryPicture.File.CopyToAsync(dataStream);

                    var category = _unitOfWork.Categories.GetById(categoryPicture.Id);
                    if (category == null)
                    {
                        return NotFound();
                    }
                    category.Picture = dataStream.ToArray();
                    _unitOfWork.Complete();
                }
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
