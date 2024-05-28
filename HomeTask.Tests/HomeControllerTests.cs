using Introduction_HomeTask.Controllers;
using Introduction_HomeTask.Models;
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
    public class HomeControllerTests
    {
        private readonly Mock<ILogger<HomeController>> _loggerMock;
        public HomeControllerTests()
        {
            _loggerMock = new Mock<ILogger<HomeController>>();
        }

        [Fact]
        public void Index_ReturnsAPlainViewResult()
        {
            //Arrange
            var controller = new HomeController(_loggerMock.Object);

            //Act
            var result = controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }
    }
}
