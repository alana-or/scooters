namespace Deliveries.Domain.RentCalculators;

public class UpToThirtyDaysRentCalculator : RentCalculatorBase
{
    public override double CalculateRent(int rentDays, int excessDays)
    {
        var days = 30;
        var cost = 22;
        
        if (rentDays <= days)
        {
            double rentTotal = rentDays * cost;
            rentTotal += excessDays > 0 ? excessDays * ExtraCostPerDay : 0;
            return rentTotal;
        }
        else
        {
            return base.CalculateRent(rentDays, excessDays);
        }
    }
}
