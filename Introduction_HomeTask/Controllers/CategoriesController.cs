using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Introduction_HomeTask.Models;

namespace Introduction_HomeTask.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly NorthwindContext _context;

        public CategoriesController(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var listOfCategories  = await _context.Categories.ToListAsync();
            return View(listOfCategories);
        }
    }
}
