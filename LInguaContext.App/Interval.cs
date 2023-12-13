using LinguaContext.Models;

namespace LInguaContext.App;

public static class Interval
{
    public static int ComputeInterval(PersonalFactors factors, ref double easeFactor, double interval, double delay, int result)
    {
        int interval1 = Convert.ToInt32(Math.Round(interval * factors.FailIntervalModifier));
        if (result == 1)
        {
            easeFactor = Math.Max(1.3, easeFactor - 0.2);
            return interval1;
        }
        else
        {
            int interval2 = Math.Max(
                Convert.ToInt32(Math.Round((interval + delay / 4) * factors.HardIntervalModifier * factors.IntervalModifier)),
                interval1 + 1
            );
            if (result == 2)
            {
                easeFactor = Math.Max(1.3, easeFactor - 0.15);
                return interval2;
            }
            else
            {
                int interval3 = Math.Max(
                    Convert.ToInt32(Math.Round((interval + delay / 2) * easeFactor * factors.IntervalModifier)),
                    interval2 + 1
                    );
                if (result == 3)
                {
                    return interval3;
                }
                else
                {
                    int interval4 = Math.Max(
                        Convert.ToInt32(Math.Round((interval + delay) * easeFactor * factors.IntervalModifier * factors.EasyIntervalModifier)),
                        interval3 + 1
                        );
                    easeFactor = Math.Max(1.3, easeFactor + 0.15);
                    return interval4;
                }
            }
        }
    }
}
