namespace Deliveries.Domain.RentCalculators;

public class UpToFortyFiveDaysRentCalculator : RentCalculatorBase
{
    public override double CalculateRent(int rentDays, int excessDays)
    {
        var days = 45;
        var cost = 20;

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
