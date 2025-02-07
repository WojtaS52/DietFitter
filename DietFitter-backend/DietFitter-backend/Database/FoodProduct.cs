namespace DietFitter_backend.Database
{
    public class FoodProduct
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Iron { get; set; }
        public double Potassium { get; set; }
        public double Magnesium { get; set; }
        public double Sodium { get; set; }
        public double Zinc { get; set; }
        public double VitaminD { get; set; }
        public double GlycemicIndex { get; set; }
        public double Calories { get; set; }    //podane na 100g
        public double Protein { get; set; }     //podane na 100g
        public double Fat { get; set; }         //podane na 100g
        public double Carbs { get; set; }      //podane na 100g
        public double Calcium { get; set; } 
        public string Category { get; set; } = string.Empty;
    }

}