using System;
using Xunit;
using backend.Controllers;
using backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace backend.Tests.Controllers
{
    public class AimTrainerControllerTests
    {
        private readonly AimTrainerController _controller;

        public AimTrainerControllerTests()
        {
            _controller = new AimTrainerController();
        }

        [Fact]
        public void GetTarget_ValidDimensions_ReturnsTargetWithinBounds()
        {
            // Arrange
            var maxTarget = new Target { x = 500, y = 400 };

            // Act
            var result = _controller.GetTarget(maxTarget) as OkObjectResult;
            
            // Assert
            Assert.NotNull(result);
            
            var target = result.Value as Target;
            Assert.NotNull(target);

            // Verify x coordinate is within bounds
            Assert.InRange(target.x, 0, maxTarget.x - 50);
            
            // Verify y coordinate is within bounds
            Assert.InRange(target.y, 0, maxTarget.y - 50);
        }

        [Theory]
        [InlineData(0, 100)]
        [InlineData(100, 0)]
        [InlineData(-50, 100)]
        [InlineData(100, -50)]
        public void GetTarget_InvalidDimensions_ReturnsBadRequest(int x, int y)
        {
            // Arrange
            var maxTarget = new Target { x = x, y = y };

            // Act
            var result = _controller.GetTarget(maxTarget) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Invalid dimensions provided.", result.Value);
        }

        [Fact]
        public void GetTarget_MultipleCallsProduceDifferentTargets()
        {
            // Arrange
            var maxTarget = new Target { x = 500, y = 400 };
            var generatedTargets = new HashSet<(int, int)>();

            // Act & Assert
            for (int i = 0; i < 100; i++)
            {
                var result = _controller.GetTarget(maxTarget) as OkObjectResult;
                Assert.NotNull(result);

                var target = result.Value as Target;
                Assert.NotNull(target);

                // Ensure each generated target is unique
                var targetPosition = (target.x, target.y);
                generatedTargets.Add(targetPosition);
            }

            // Verify multiple unique positions were generated
            Assert.True(generatedTargets.Count > 1, "Multiple unique target positions should be generated");
        }

        [Fact]
        public void GetTarget_EnsuresTargetSizeConsideration()
        {
            // Arrange
            const int targetSize = 50;
            var maxTarget = new Target { x = 100, y = 100 };

            // Act
            var result = _controller.GetTarget(maxTarget) as OkObjectResult;
            
            // Assert
            Assert.NotNull(result);
            
            var target = result.Value as Target;
            Assert.NotNull(target);

            // Verify x coordinate allows for full target size
            Assert.True(target.x + targetSize <= maxTarget.x, 
                "Target x coordinate should allow full target width");
            
            // Verify y coordinate allows for full target size
            Assert.True(target.y + targetSize <= maxTarget.y, 
                "Target y coordinate should allow full target height");
        }
    }
}