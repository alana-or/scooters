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
        int rentDays = CalculateRentDays(startDate, finalDate);
        int excessDays = CalculateExcessDays(finalDate, rentDays, expectedFinalDate);

        return _handler.CalculateRent(rentDays, excessDays);
    }

    private int CalculateExcessDays(DateTime finalDate, int rentDays, DateTime expectedFinalDate)
    {
        int differenceExpected = (finalDate - expectedFinalDate).Days;
        return rentDays == 0 ? differenceExpected + 1 : differenceExpected;
    }

    private int CalculateRentDays(DateTime startDate, DateTime finalDate)
    {
        return Math.Max((finalDate - startDate).Days, 1);
    }
}