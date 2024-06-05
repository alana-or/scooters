using FluentAssertions;

namespace Deliveries.Domain.Tests.Unit;

public class DeliveryPersonRentalTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Rent_Should_Calculate_Dates()
    {
        var rent = new DeliveryPersonRental(
            new Guid(), 2024, "model",
            "licencePlate", 
            new DeliveryPerson("Name","image", "123", "123", 'A', DateTime.Now));

        var expectedDate = DateTime.Today.AddDays(5);

        rent.CreateRental(expectedDate);
        
        rent.Create.Should().Be(DateTime.Today);
        rent.Start.Should().Be(DateTime.Today.AddDays(1));
        rent.EndExpected.Should().Be(expectedDate);
    }

    [Test]
    public void Rent_Should_Throw_Exception_When_CNH_Not_Type_A()
    {
        var rent = new DeliveryPersonRental(
            new Guid(), 2024, "model",
            "licencePlate",
            new DeliveryPerson("Name", "image", "123", "123", 'B', DateTime.Now));

        var expectedDate = DateTime.Today.AddDays(5);

        var act = () => rent.CreateRental(expectedDate);
        
        act.Should().Throw<Exception>()
            .WithMessage("CNH type A only");
    }

    [Test]
    public void CalculateRent_Should_Calculate_RentTotal_With_UpToSevenDaysRent()
    {
        var rent = new DeliveryPersonRental(
            new Guid(), 2024, "model",
            "licencePlate",
            new DeliveryPerson("Name", "image", "123", "123", 'A', DateTime.Now));

        var expectedDate = DateTime.Today.AddDays(8);

        rent.CreateRental(expectedDate);
        
        rent.CalculateRent();

        rent.RentTotal.Should().Be(252);
    }

    [Test]
    public void CalculateRent_Should_Calculate_RentTotal_With_UpToFifteenDaysRent()
    {
        var rent = new DeliveryPersonRental(
            new Guid(), 2024, "model",
            "licencePlate",
            new DeliveryPerson("Name", "image", "123", "123", 'A', DateTime.Now));

        var expectedDate = DateTime.Today.AddDays(16);

        rent.CreateRental(expectedDate);

        rent.CalculateRent();

        rent.RentTotal.Should().Be(540);
    }

    [Test]
    public void CalculateRent_Should_Calculate_RentTotal_With_UpToThirtyDaysRent()
    {
        var rent = new DeliveryPersonRental(
            new Guid(), 2024, "model",
            "licencePlate",
            new DeliveryPerson("Name", "image", "123", "123", 'A', DateTime.Now));

        var expectedDate = DateTime.Today.AddDays(31);

        rent.CreateRental(expectedDate);

        rent.CalculateRent();

        rent.RentTotal.Should().Be(1080);
    }

    [Test]
    public void CalculateRent_Should_Calculate_RentTotal_With_UpToFortyFiveDaysRent()
    {
        var rent = new DeliveryPersonRental(
            new Guid(), 2024, "model",
            "licencePlate",
            new DeliveryPerson("Name", "image", "123", "123", 'A', DateTime.Now));

        var expectedDate = DateTime.Today.AddDays(46);

        rent.CreateRental(expectedDate);

        rent.CalculateRent();

        rent.RentTotal.Should().Be(1620);
    }

    [Test]
    public void CalculateRent_Should_Calculate_RentTotal_With_UpToFiftyDaysRent()
    {
        var rent = new DeliveryPersonRental(
            new Guid(), 2024, "model",
            "licencePlate",
            new DeliveryPerson("Name", "image", "123", "123", 'A', DateTime.Now));

        var expectedDate = DateTime.Today.AddDays(51);

        rent.CreateRental(expectedDate);

        rent.CalculateRent();

        rent.RentTotal.Should().Be(1800);
    }
}