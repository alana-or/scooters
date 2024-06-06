namespace Deliveries.Domain.RentCalculators;

public class RentCalculator
{
    private IRentCalculator _handler;

    public RentCalculator()
    {
        _handler = GetConfigureRentChain();
    }

    private IRentCalculator GetConfigureRentChain()
    {
        var upToSevenDaysHandler = new UpToSevenDaysRentCalculator();
        var upToFiftyDaysHandler = new UpToFiftyDaysRentCalculator();

        upToSevenDaysHandler
            .SetNextHandler(new UpToFifteenDaysRentCalculator())
            .SetNextHandler(new UpToThirtyDaysRentCalculator())
            .SetNextHandler(new UpToFortyFiveDaysRentCalculator())
            .SetNextHandler(upToFiftyDaysHandler);

        return upToSevenDaysHandler;
    }

    public double CalculateRent(DateTime startDate, DateTime finalDate, DateTime expectedFinalDate)
    {
        var rentDays = Math.Max((finalDate - startDate).Days, 1);
        var excessDays = (finalDate - expectedFinalDate).Days;

        return _handler.CalculateRent(rentDays, excessDays);
    }
}