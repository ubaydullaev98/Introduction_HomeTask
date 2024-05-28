using Introduction_HomeTask.Controllers;
using Introduction_HomeTask.Models;
using Introduction_HomeTask.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeTask.Tests
{
    public class CategoriesControllerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<CategoriesController>> _loggerMock;
        public CategoriesControllerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CategoriesController>>();
        }
        [Fact]
        public void Index_ReturnsViewResult_WithCategories() 
        {
            //Arrange
            _unitOfWorkMock.Setup(u => u.Categories.GetAll()).Returns(GetTestCategories()).Verifiable();
            var controller = new CategoriesController(_unitOfWorkMock.Object, _loggerMock.Object);

            //Act
            var result = controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Category>>(viewResult.ViewData.Model);
            Assert.Equal(2, model.Count());
            _unitOfWorkMock.Verify();
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
    }
}
