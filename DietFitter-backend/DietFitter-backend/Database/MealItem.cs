using System.Text.Json.Serialization;

namespace DietFitter_backend.Database
{
    public class MealItem
    {
        public int Id { get; set; }
        public string Food { get; set; } = string.Empty;
        public double Grams { get; set; }
        public double ProvidedValue { get; set; }

        public int MealId { get; set; }
        [JsonIgnore] // ✅ Unikamy cyklicznej referencji
        public Meal Meal { get; set; } = null!;
    }
}
/*namespace DietFitter_backend.Database
  {
      public class MealItem
      {
          public int Id { get; set; }  // Identyfikator
          public int MealId { get; set; } // Klucz obcy do `Meal`
          public Meal Meal { get; set; } // Nawigacja do `Meal`
  
          public string Food { get; set; } // Nazwa produktu (np. "Jajka")
          public double Grams { get; set; } // Ilość w gramach
          public double ProvidedValue { get; set; } // Wartość kaloryczna/kcal dostarczona przez produkt
      }
  }
*/