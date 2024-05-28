using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Introduction_HomeTask.Models;
using Introduction_HomeTask.Configurations;
using Microsoft.Extensions.Options;
using Introduction_HomeTask.Pagination;
using Introduction_HomeTask.Services;

namespace Introduction_HomeTask.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ProductOptions _options;

        public ProductsController(IUnitOfWork unitOfWork, IOptions<ProductOptions> productOptions)
        {
            _unitOfWork = unitOfWork;
            _options = productOptions.Value;
        }

        
        public IActionResult Index(int? pageNumber)
        {
            var products = _unitOfWork.Products.GetAllIncludingSupplierAndCategories();

            return View(PaginatedList<Product>.Create(products, pageNumber ?? 1, _options.MaxAmountOfProducts));
        }

        
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_unitOfWork.Categories.GetAll(), "CategoryId", "CategoryId");
            ViewData["SupplierId"] = new SelectList(_unitOfWork.GetSuppliers(), "SupplierId", "SupplierId");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Products.Add(product);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_unitOfWork.Categories.GetAll(), "CategoryId", "CategoryId", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_unitOfWork.GetSuppliers(), "SupplierId", "SupplierId", product.SupplierId);
            return View(product);
        }

       
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _unitOfWork.Products.GetById((int)id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_unitOfWork.Categories.GetAll(), "CategoryId", "CategoryId", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_unitOfWork.GetSuppliers(), "SupplierId", "SupplierId", product.SupplierId);
            return View(product);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Products.Update(product);
                    _unitOfWork.Complete();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_unitOfWork.Categories.GetAll(), "CategoryId", "CategoryId", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_unitOfWork.GetSuppliers(), "SupplierId", "SupplierId", product.SupplierId);
            return View(product);
        }


        private bool ProductExists(int id)
        {
            return _unitOfWork.Products.GetAll().Any(e => e.ProductId == id);
        }
    }
}
