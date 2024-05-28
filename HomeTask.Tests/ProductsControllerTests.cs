using Introduction_HomeTask.Configurations;
using Introduction_HomeTask.Controllers;
using Introduction_HomeTask.Models;
using Introduction_HomeTask.Pagination;
using Introduction_HomeTask.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace HomeTask.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IOptions<ProductOptions>> _options;
        public ProductsControllerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _options = new Mock<IOptions<ProductOptions>>();

            var _configData = new ProductOptions { ConnectionString = "TestConnectionString" , MaxAmountOfProducts = 1};
            _options.Setup(o => o.Value).Returns(_configData);

            _unitOfWorkMock.Setup(u => u.Products.GetAll()).Returns(GetTestProducts());
            _unitOfWorkMock.Setup(u => u.Categories.GetAll()).Returns(GetTestCategories());
            _unitOfWorkMock.Setup(u => u.GetSuppliers()).Returns(GetTestSuppliers());
        }

        [Fact]
        public void Index_ReturnsViewResult_WithProductsAsync() 
        {
            //Arrange
            _unitOfWorkMock.Setup(u => u.Products.GetAllIncludingSupplierAndCategories()).Returns(GetTestProducts());
            
            var controller = new ProductsController(_unitOfWorkMock.Object, _options.Object);

            //Act
            var result = controller.Index(1);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<PaginatedList<Product>>(viewResult.ViewData.Model);
            Assert.Single(model);
        }

        [Fact]
        public void CreatePost_WhenModelStateIsValid_ReturnsRedirect()
        {
            //Arrange
            var data = new List<Product>();
            _unitOfWorkMock.Setup(u => u.Products.Add(It.IsAny<Product>())).Callback((Product t) => data.Add(t));
            _unitOfWorkMock.Setup(u => u.Complete()).Returns(1);

            var controller = new ProductsController(_unitOfWorkMock.Object, _options.Object);
            var product = GetTestProducts().First();

            //Act
            var result = controller.Create(product);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Single(data);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public void EditPost_WhenModelStateIsValid_ReturnsRedirect()
        {
            //Arrange
            var data = new List<Product>() { new Product { ProductId = 1, ProductName = "Test", Discontinued = false} };
            _unitOfWorkMock.Setup(u => u.Products.Update(It.IsAny<Product>())).Callback((Product t) => data[0] = t);
            _unitOfWorkMock.Setup(u => u.Complete()).Returns(1);

            var controller = new ProductsController(_unitOfWorkMock.Object, _options.Object);
            var product = new Product { ProductId = 1, ProductName = "Updated Product", Discontinued = false };

            //Act
            var result = controller.Edit(product.ProductId, product);

            //Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Updated Product", data[0].ProductName);
            Assert.Null(redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }
        [Fact]
        public void EditPost_WhenModelStateIsInValid_ReturnsNotFoundResult()
        {
            //Arrange
            var data = new List<Product>() { new Product { ProductId = 1, ProductName = "Test", Discontinued = false } };
            _unitOfWorkMock.Setup(u => u.Products.Update(It.IsAny<Product>())).Callback((Product t) => data[0] = t);
            _unitOfWorkMock.Setup(u => u.Complete()).Returns(1);

            var controller = new ProductsController(_unitOfWorkMock.Object, _options.Object);
            var product = new Product { ProductId = 1, ProductName = "Updated Product", Discontinued = false };

            //Act
            var result = controller.Edit(2, product);

            //Assert
            var redirectToActionResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void EditPost_WhenModelStateIsInValid_ReturnsView()
        {
            //Arrange
            var data = new List<Product>() { new Product { ProductId = 1, ProductName = "Test", Discontinued = false } };
            _unitOfWorkMock.Setup(u => u.Products.Update(It.IsAny<Product>())).Callback((Product t) => data[0] = t);
            _unitOfWorkMock.Setup(u => u.Complete()).Returns(1);

            var controller = new ProductsController(_unitOfWorkMock.Object, _options.Object);
            var product = new Product { ProductId = 1, Discontinued = false };

            controller.ModelState.AddModelError("ProductName", "Required");

            //Act
            var result = controller.Edit(product.ProductId, product);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Product>(viewResult.ViewData.Model);
            Assert.Equal(product, model);
        }

        private IQueryable<Product> GetTestProducts()
        {
            var products = new List<Product>();
            products.Add(new Product()
            {
                ProductId = 1,
                ProductName = "Test",
                Discontinued = false,

            });
;
            return products.AsQueryable<Product>();
        }

        private IEnumerable<Category> GetTestCategories()
        {
            var categories = new List<Category>();
            categories.Add(new Category()
            {
                CategoryId = 1,
                CategoryName = "Test",
                Description = "Test"

            });
            categories.Add(new Category()
            {
                CategoryId = 2,
                CategoryName = "Test2",
                Description = "Test2"

            });
            return categories;
        }

        private IEnumerable<Supplier> GetTestSuppliers()
        {
            var suppliers = new List<Supplier>();
            suppliers.Add(new Supplier()
            {
                SupplierId = 1,
                CompanyName = "Test",

            });
            suppliers.Add(new Supplier()
            {
                SupplierId = 2,
                CompanyName = "Test2",

            });

            return suppliers;
        }
    }
}
