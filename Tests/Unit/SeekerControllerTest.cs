using backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using System.Collections.Generic;

namespace Tests.Unit
{
    public class SeekerControllerTest
    {
        [Fact]
        public void GetCell_ReturnsListOfTwoIntegers()
        {
            // Arrange
            var controller = new SeekerController();

            var actionResult = controller.GetCell();
            var result = actionResult.Result as OkObjectResult;
            var cell = result?.Value as List<int>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(cell);
            Assert.Equal(2, cell.Count);
        }

        [Fact]
        public void GetCell_ReturnsIntegersBetween0And9()
        {
            // Arrange
            var controller = new SeekerController();

            // Act
            var actionResult = controller.GetCell();
            var result = actionResult.Result as OkObjectResult;
            var cell = result?.Value as List<int>;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(cell);
            Assert.InRange(cell[0], 0, 9);
            Assert.InRange(cell[1], 0, 9);
        }
    }
}