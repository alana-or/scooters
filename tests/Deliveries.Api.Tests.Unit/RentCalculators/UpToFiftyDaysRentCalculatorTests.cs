using Deliveries.Domain.RentCalculators;
using FluentAssertions;

namespace Deliveries.Domain.Tests.Unit.RentCalculators;

public class UpToFiftyDaysRentCalculatorTests
{
    [TestCase(51, 1, 950)]
    [TestCase(50, 0, 900)]
    [TestCase(49, 0, 882)]
    [TestCase(1, 0, 18)]
    public void CalculateRent_Should_Calculate_Total(int rentDays, int excessDays, int totalExpected)
    {
        var rent = new UpToFiftyDaysRentCalculator();

        var total = rent.CalculateRent(rentDays, excessDays);

        total.Should().Be(totalExpected);
    }
}