namespace Deliveries.Domain.RentCalculators;

public class UpToFifteenDaysRentCalculator : RentCalculatorBase
{
    public override double CalculateRent(int rentDays, int excessDays)
    {
        var days = 15;
        var cost = 28;
        var penalty = 0.40;

        if (rentDays <= days)
        {
            return rentDays * cost + 
                (returnedEarly() ? 
                    calculatePenalty() : 
                    returnedLater() ?
                        calculateExtraCost(excessDays) : 
                        0);
        }
        else
        {
            return base.CalculateRent(rentDays, excessDays);
        }

        bool returnedEarly()
        {
            return excessDays < 0;
        }

        bool returnedLater()
        {
            return excessDays > 0;

        }

        int calculateExtraCost(int excessDays)
        {
            return excessDays * ExtraCostPerDay;
        }

        double calculatePenalty()
        {
            return excessDays * -1 * cost * penalty;
        }
    }
}
