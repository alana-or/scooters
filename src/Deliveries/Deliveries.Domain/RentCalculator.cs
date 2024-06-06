namespace Deliveries.Domain;

public interface IRentCalculatorHandler
{
    IRentCalculatorHandler SetNextHandler(IRentCalculatorHandler handler);
    double CalculateRent(int rentDays, int differenceExpected);
}

public abstract class RentCalculatorHandlerBase : IRentCalculatorHandler
{
    private IRentCalculatorHandler _nextHandler;

    public IRentCalculatorHandler SetNextHandler(IRentCalculatorHandler nextHandler)
    {
        _nextHandler = nextHandler;
        return nextHandler;
    }

    public virtual double CalculateRent(int rentDays, int excessDays)
    {
        if (_nextHandler != null)
        {
            return _nextHandler.CalculateRent(rentDays, excessDays);
        }

        return 0;
    }
}

public class UpToSevenDaysRentCalculatorHandler : RentCalculatorHandlerBase
{
    public override double CalculateRent(int rentDays, int excessDays)
    {
        var days = 7;
        var cost = 30;
        var extraCostPerDay = 50;
        var penalty = 1.20;

        if (rentDays <= days)
        {
            double rentTotal = rentDays * cost;
            rentTotal += excessDays < 0 ? excessDays * -1 * cost * penalty :
                excessDays > 0 ? excessDays * extraCostPerDay : 0;

            return rentTotal;
        }
        else
        {
            return base.CalculateRent(rentDays, excessDays);
        }
    }
}

public class UpToFifteenDaysRentCalculatorHandler : RentCalculatorHandlerBase
{
    public override double CalculateRent(int rentDays, int excessDays)
    {
        var days = 15;
        var cost = 28;
        var penalty = 1.40;
        var extraCostPerDay = 50;

        if (rentDays <= days)
        {
            double rentTotal = rentDays * cost;
            rentTotal += excessDays < 0 ? excessDays * -1 * cost * penalty :
                excessDays > 0 ? excessDays * extraCostPerDay : 0;
            return rentTotal;
        }
        else
        {
            return base.CalculateRent(rentDays, excessDays);
        }
    }
}

public class UpToThirtyDaysRentCalculatorHandler : RentCalculatorHandlerBase
{
    public override double CalculateRent(int rentDays, int excessDays)
    {
        var days = 30;
        var cost = 22;
        var extraCostPerDay = 50;

        if (rentDays <= days)
        {
            double rentTotal = rentDays * cost;
            rentTotal += excessDays > 0 ? excessDays * extraCostPerDay : 0;
            return rentTotal;
        }
        else
        {
            return base.CalculateRent(rentDays, excessDays);
        }
    }
}

public class UpToFortyFiveDaysRentCalculatorHandler : RentCalculatorHandlerBase
{
    public override double CalculateRent(int rentDays, int excessDays)
    {
        var days = 45;
        var cost = 20;
        var extraCostPerDay = 50;

        if (rentDays <= days)
        {
            double rentTotal = rentDays * cost;
            rentTotal += excessDays > 0 ? excessDays * extraCostPerDay : 0;
            return rentTotal;
        }
        else
        {
            return base.CalculateRent(rentDays, excessDays);
        }
    }
}

public class UpToFiftyDaysRentCalculatorHandler : RentCalculatorHandlerBase
{
    public override double CalculateRent(int rentDays, int excessDays)
    {
        var days = 50;
        var cost = 18;
        var extraCostPerDay = 50;

        if (rentDays <= days)
        {
            double rentTotal = rentDays * cost;
            rentTotal += excessDays > 0 ? excessDays * extraCostPerDay : 0;
            return rentTotal;
        }
        else
        {
            return base.CalculateRent(rentDays, excessDays);
        }
    }
}

public class RentCalculator
{
    private IRentCalculatorHandler _handler;

    public RentCalculator()
    {
        var upToSevenDaysHandler = new UpToSevenDaysRentCalculatorHandler();
        var upToFifteenDaysHandler = new UpToFifteenDaysRentCalculatorHandler();
        var upToThirtyDaysHandler = new UpToThirtyDaysRentCalculatorHandler();
        var upToFortyFiveDaysHandler = new UpToFortyFiveDaysRentCalculatorHandler();
        var upToFiftyDaysHandler = new UpToFiftyDaysRentCalculatorHandler();

        upToSevenDaysHandler
            .SetNextHandler(upToFifteenDaysHandler)
            .SetNextHandler(upToThirtyDaysHandler)
            .SetNextHandler(upToFortyFiveDaysHandler)
            .SetNextHandler(upToFiftyDaysHandler);

        _handler = upToSevenDaysHandler;
    }

    public double CalculateRent(int rentDays, int excessDays)
    {
        return _handler.CalculateRent(rentDays, excessDays);
    }
}
