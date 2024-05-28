using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Introduction_HomeTask.Models;
using Introduction_HomeTask.Services;

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
    }
}
