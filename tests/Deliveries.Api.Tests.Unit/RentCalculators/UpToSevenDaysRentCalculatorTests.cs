using Deliveries.Domain.RentCalculators;
using FluentAssertions;
using Moq;

namespace Deliveries.Domain.Tests.Unit.RentCalculators;

public class UpToSevenDaysRentCalculatorTests
{
    [Test]
    public void CalculateRent_WhenRentDaysGreaterThanSeven_CallsBaseCalculateRent_TotalIsNotCalculateInThisClass()
    {
        var rent = new UpToSevenDaysRentCalculator();
        var total = rent.CalculateRent(8, 0);
        total.Should().Be(0);
    }

    [TestCase(7, 0, 196)]
    [TestCase(6, -1, 179.2)]
    [TestCase(1, -6, 95.2)]
    public void CalculateRent_Should_Calculate_Total(int rentDays, int excessDays, double totalExpected)
    {
        var rent = new UpToFifteenDaysRentCalculator();

        var total = rent.CalculateRent(rentDays, excessDays);

        total.Should().Be(totalExpected);
    }
}