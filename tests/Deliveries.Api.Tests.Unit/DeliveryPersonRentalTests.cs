using FluentAssertions;
using Moq;

namespace Deliveries.Domain.Tests.Unit;

public class DeliveryPersonRentalTests
{

    [Test]
    public void CreateRental_ThrowsException_WhenCNHTypeIsNotA()
    {
        var rental = new DeliveryPersonRental(
            new Guid(), 2024, "model",
            "licencePlate",
            new DeliveryPerson("Name", "image", "123", "123", 'B', DateTime.Now));

        var act = () => rental.CreateRental(DateTime.Today.AddDays(2));

        act.Should().Throw<Exception>();
    }

    [Test]
    public void CreateRental_DoesNotThrowException_WhenCNHTypeIsA()
    {
        var rental = new DeliveryPersonRental(
            new Guid(), 2024, "model",
            "licencePlate",
            new DeliveryPerson("Name", "image", "123", "123", 'A', DateTime.Now));

        var act = () => rental.CreateRental(DateTime.Today.AddDays(2));

        act.Should().NotThrow<Exception>();
    }

    [TestCase(7, 210)]
    [TestCase(15, 420)]
    [TestCase(30, 660)]
    [TestCase(45, 900)]
    [TestCase(50, 900)]
    public void CreateRental_Should_Calculate_RentTotal(int days, int totalRentExpected)
    {
        const int startDayDiference = 1;

        var rental = new DeliveryPersonRental(
            new Guid(), 2024, "model",
            "licencePlate",
            new DeliveryPerson("Name", "image", "123", "123", 'A', DateTime.Now));

        var expectedDate = DateTime.Today.AddDays(days + startDayDiference);

        rental.CreateRental(expectedDate);

        rental.ExpectedEnd.Should().Be(expectedDate);
        rental.End.Should().Be(DateTime.MinValue);
        rental.RentTotal.Should().Be(totalRentExpected);
    }

    [Test]
    public void ReturnRentedScooter()
    {
        var rental = new DeliveryPersonRental(
            new Guid(), 2024, "model",
            "licencePlate",
            new DeliveryPerson("Name", "image", "123", "123", 'A', DateTime.Now));

        rental.Start = DateTime.Today;
        rental.ExpectedEnd = DateTime.Today.AddDays(7); 

        rental.ReturnRentedScooter();

        rental.End.Should().Be(DateTime.Today);
        rental.RentTotal.Should().Be(72);
    }
}