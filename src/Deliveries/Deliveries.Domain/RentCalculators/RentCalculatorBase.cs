namespace Deliveries.Domain.RentCalculators;

public interface IRentCalculator
{
    IRentCalculator SetNextHandler(IRentCalculator handler);
    double CalculateRent(int rentDays, int excessDays);
}

public abstract class RentCalculatorBase : IRentCalculator
{
    private IRentCalculator _nextHandler;
    public int ExtraCostPerDay = 50;

    public IRentCalculator SetNextHandler(IRentCalculator nextHandler)
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
