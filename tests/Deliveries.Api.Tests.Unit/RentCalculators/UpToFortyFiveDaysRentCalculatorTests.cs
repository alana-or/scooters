using Deliveries.Domain.RentCalculators;
using FluentAssertions;
using Moq;

namespace Deliveries.Domain.Tests.Unit.RentCalculators;

public class UpToFortyFiveDaysRentCalculatorTests
{
    [Test]
    public void CalculateRent_WhenRentDaysGreaterThanFortyFive_CallsBaseCalculateRent_TotalIsNotCalculateInThisClass()
    {
        var rent = new UpToFortyFiveDaysRentCalculator();
        var total = rent.CalculateRent(46, 0);
        total.Should().Be(0);
    }

    [TestCase(45, 0, 900)]
    [TestCase(44, -1, 880)]
    [TestCase(1, -44, 20)]
    public void CalculateRent_Should_Calculate_Total(int rentDays, int excessDays, double totalExpected)
    {
        var rent = new UpToFortyFiveDaysRentCalculator();

        var total = rent.CalculateRent(rentDays, excessDays);

        total.Should().Be(totalExpected);
    }
}