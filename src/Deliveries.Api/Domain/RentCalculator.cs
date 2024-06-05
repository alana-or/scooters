namespace Deliveries.Api.Domain;

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
        if (rentDays <= 7)
        {
            double rentTotal = rentDays * 30;
            rentTotal += excessDays < 0 ? ((excessDays * -1) * 30) * 1.20 :
                excessDays > 0 ? excessDays * 50 : 0;
            
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
        if (rentDays <= 15)
        {
            double rentTotal = rentDays * 28;
            rentTotal += excessDays < 0 ? ((excessDays * -1) * 28) * 1.40 :
                excessDays > 0 ? excessDays * 50 : 0;
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
        if (rentDays <= 30)
        {
            double rentTotal = rentDays * 22;
            rentTotal += excessDays > 0 ? excessDays * 50 : 0;
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
        if (rentDays <= 45)
        {
            double rentTotal = rentDays * 20;
            rentTotal += excessDays > 0 ? excessDays * 50 : 0;
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
        if (rentDays <= 50)
        {
            double rentTotal = rentDays * 18;
            rentTotal += excessDays > 0 ? excessDays * 50 : 0;
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
