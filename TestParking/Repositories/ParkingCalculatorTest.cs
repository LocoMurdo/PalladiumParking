using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestParking.Repositories
{
    public class ParkingCalculatorTest
    {
        [Fact]
        public void ShouldCalculateParkingCorrectly()
        {
            // Arrange
            decimal rate = 1000;
            int hours = 3;

            // Act
            var total = rate * hours;

            // Assert
            Assert.Equal(3000, total);
        }
    }
}
