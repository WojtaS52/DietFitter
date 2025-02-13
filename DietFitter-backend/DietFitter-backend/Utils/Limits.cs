namespace DietFitter_backend.Utils;

public class Limits
{
    public static readonly Dictionary<string,double> DailyPortion = new Dictionary<string, double>
    {
        {"Iron", 18},
        {"Potassium", 4700},
        {"Magnesium", 400},
        {"Sodium", 2300},
        {"Zinc", 11},
        {"VitaminD", 20},
        {"Calories", 2000},
        {"Protein", 50},
        {"Fat", 70},
        {"Carbs", 310},
        {"Calcium", 1000}
    };
}