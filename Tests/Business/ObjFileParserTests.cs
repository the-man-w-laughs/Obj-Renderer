using Business.Contracts.Parser;
using Business.Contracts.Utils;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Parser;
using SfmlPresentation.Utils;
using System;
using System.Drawing;
using System.Numerics;
using Xunit;

namespace Tests.Parser
{
    public class PointCalculatorTests
    {        
        public PointCalculatorTests()
        {            
        }    
        
        [Fact]
        public void CalculateZ()
        {
            // Arrange
            IPointCalculator pointCalculator = new PointCalculator(new Vector3[] { 
                new Vector3(1, 2, 3), new Vector3(4, 5, 6), new Vector3(71, 81, 91) });

            // Act            
            var z = pointCalculator.CalculatePointOnPlane(1, 2).Z;
            // Assert
            Assert.Equal(3, z);
        }
    }
}