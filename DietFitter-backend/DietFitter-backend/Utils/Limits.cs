namespace DietFitter_backend.Utils;

public class Limits
{
    
    //Daily portion of micro and macro elements in mg there are maximum limits of portion
    // for example I supplement Potas and recommendation portion is 300mg per Day so 
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