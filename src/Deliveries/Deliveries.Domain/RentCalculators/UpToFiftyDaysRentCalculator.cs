namespace Deliveries.Domain.RentCalculators;

public class UpToFiftyDaysRentCalculator : RentCalculatorBase
{
    public override double CalculateRent(int rentDays, int excessDays)
    {
        var days = 50;
        var cost = 18;

        double rentTotal = rentDays <= days ? rentDays * cost : days * cost;
        rentTotal += excessDays > 0 ? excessDays * ExtraCostPerDay : 0;
        return rentTotal;
    }
}
