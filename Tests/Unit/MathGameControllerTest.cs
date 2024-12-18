using System;
using Xunit;
using backend.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace backend.Tests.Controllers
{
    public class MathGameControllerTests
    {
        private readonly MathGameController _controller;

        public MathGameControllerTests()
        {
            _controller = new MathGameController();
        }

        [Fact]
        public void GetFormula_EasyDifficulty_ReturnsValidResult()
        {
            // Act
            var result = _controller.GetFormula("easy") as OkObjectResult;
            
            // Assert
            Assert.NotNull(result);
            
            // Use reflection to safely access properties
            var resultValue = result.Value;
            var formulaProperty = resultValue.GetType().GetProperty("formula");
            var solutionProperty = resultValue.GetType().GetProperty("solution");
            
            Assert.NotNull(formulaProperty);
            Assert.NotNull(solutionProperty);

            string formula = formulaProperty.GetValue(resultValue)?.ToString();
            int solution = Convert.ToInt32(solutionProperty.GetValue(resultValue));

            // Verify formula components
            string[] parts = formula.Split(' ');
            Assert.Equal(3, parts.Length);
            
            Assert.True(int.TryParse(parts[0], out int num1));
            Assert.True(int.TryParse(parts[2], out int num2));
            Assert.Contains(parts[1], new[] { "+", "-", "*" });

            // Verify solution calculation
            int expectedSolution = parts[1] switch
            {
                "+" => num1 + num2,
                "-" => num1 - num2,
                "*" => num1 * num2,
                _ => 0
            };
            Assert.Equal(expectedSolution, solution);
        }

        [Fact]
        public void GetFormula_MediumDifficulty_ReturnsValidResult()
        {
            // Act
            var result = _controller.GetFormula("medium") as OkObjectResult;
            
            // Assert
            Assert.NotNull(result);
            
            // Use reflection to safely access properties
            var resultValue = result.Value;
            var formulaProperty = resultValue.GetType().GetProperty("formula");
            var solutionProperty = resultValue.GetType().GetProperty("solution");
            
            Assert.NotNull(formulaProperty);
            Assert.NotNull(solutionProperty);

            string formula = formulaProperty.GetValue(resultValue)?.ToString();
            int solution = Convert.ToInt32(solutionProperty.GetValue(resultValue));

            // Verify formula components
            string[] parts = formula.Split(' ');
            Assert.Equal(3, parts.Length);
            
            Assert.True(int.TryParse(parts[0], out int num1));
            Assert.True(int.TryParse(parts[2], out int num2));
            Assert.Contains(parts[1], new[] { "+", "-", "*" });

            // Verify numbers are within medium range
            Assert.InRange(num1, 10, 50);
            Assert.InRange(num2, 10, 50);

            // Verify solution calculation
            int expectedSolution = parts[1] switch
            {
                "+" => num1 + num2,
                "-" => num1 - num2,
                "*" => num1 * num2,
                _ => 0
            };
            Assert.Equal(expectedSolution, solution);
        }

        [Fact]
        public void GetFormula_HardDifficulty_ReturnsValidResult()
        {
            // Act
            var result = _controller.GetFormula("hard") as OkObjectResult;
            
            // Assert
            Assert.NotNull(result);
            
            // Use reflection to safely access properties
            var resultValue = result.Value;
            var formulaProperty = resultValue.GetType().GetProperty("formula");
            var solutionProperty = resultValue.GetType().GetProperty("solution");
            
            Assert.NotNull(formulaProperty);
            Assert.NotNull(solutionProperty);

            string formula = formulaProperty.GetValue(resultValue)?.ToString();
            int solution = Convert.ToInt32(solutionProperty.GetValue(resultValue));

            // Verify multi-step formula structure
            Assert.NotNull(formula);
            
            // Check for correct number of opening and closing parentheses
            int openParens = formula.Count(c => c == '(');
            int closeParens = formula.Count(c => c == ')');
            Assert.Equal(2, openParens);
            Assert.Equal(2, closeParens);

            // Verify solution exists
            Assert.IsType<int>(solution);
        }

        [Fact]
        public void GetFormula_InvalidDifficulty_ReturnsBadRequest()
        {
            // Act
            var result = _controller.GetFormula("impossible") as BadRequestObjectResult;
            
            // Assert
            Assert.NotNull(result);
            Assert.Equal("Invalid difficulty. Choose 'easy', 'medium', or 'hard'.", result.Value);
        }

        [Theory]
        [InlineData(5, 3, "+", 8)]
        [InlineData(10, 4, "-", 6)]
        [InlineData(6, 7, "*", 42)]
        public void CalculateNextValue_VariousOperations_ReturnsCorrectResult(
            int currentValue, 
            int nextValue, 
            string operation, 
            int expectedResult)
        {
            var method = typeof(MathGameController).GetMethod("CalculateNextValue", 
                BindingFlags.NonPublic | BindingFlags.Instance);
            
            var result = method.Invoke(_controller, new object[] { currentValue, nextValue, operation });
            
            Assert.Equal(expectedResult, result);
        }
    }
}