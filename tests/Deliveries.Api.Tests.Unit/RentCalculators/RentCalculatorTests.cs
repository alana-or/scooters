using Deliveries.Domain.RentCalculators;
using FluentAssertions;

namespace Deliveries.Domain.Tests.Unit.RentCalculators;

public class RentCalculatorTests
{
    [TestCase(50, 330)]
    [TestCase(45, 300)]
    [TestCase(30, 210)]
    [TestCase(15, 120)]
    [TestCase(7, 72)]
    public void CalculateRent_Should_Calculate_RentTotal(int days, double totalRentExpected)
    {
        var startDate = DateTime.Today;
        var finalDate = DateTime.Today;
        var expectedFinalDate = DateTime.Today.AddDays(days);

        var total = new RentCalculator().CalculateRent(startDate, finalDate, expectedFinalDate);

        total.Should().Be(totalRentExpected);
    }
}