using Deliveries.Domain.RentCalculators;
using FluentAssertions;
using Moq;

namespace Deliveries.Domain.Tests.Unit.RentCalculators;

public class UpToFifteenDaysRentCalculatorTests
{
    [Test]
    public void CalculateRent_WhenRentDaysGreaterThanFifteen_CallsBaseCalculateRent_TotalIsNotCalculateInThisClass()
    {
        var rent = new UpToFifteenDaysRentCalculator();
        var total = rent.CalculateRent(20, 0);
        total.Should().Be(0);
    }

    [TestCase(15, 0, 420)]
    [TestCase(14, -1, 403.2)]
    [TestCase(1, -14, 184.8)]
    public void CalculateRent_Should_Calculate_Total(int rentDays, int excessDays, double totalExpected)
    {
        var rent = new UpToFifteenDaysRentCalculator();

        var total = rent.CalculateRent(rentDays, excessDays);

        total.Should().Be(totalExpected);
    }
}