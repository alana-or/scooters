using Deliveries.Domain.RentCalculators;
using FluentAssertions;
using Moq;

namespace Deliveries.Domain.Tests.Unit.RentCalculators;

public class UpToThirtyDaysRentCalculatorTests
{
    [Test]
    public void CalculateRent_WhenRentDaysGreaterThanThirty_CallsBaseCalculateRent_TotalIsNotCalculateInThisClass()
    {
        var rent = new UpToThirtyDaysRentCalculator();
        var total = rent.CalculateRent(31, 0);
        total.Should().Be(0);
    }

    [TestCase(30, 0, 660)]
    [TestCase(29, -1, 638)]
    [TestCase(1, -29, 22)]
    public void CalculateRent_Should_Calculate_Total(int rentDays, int excessDays, double totalExpected)
    {
        var rent = new UpToThirtyDaysRentCalculator();

        var total = rent.CalculateRent(rentDays, excessDays);

        total.Should().Be(totalExpected);
    }
}