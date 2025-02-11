using System;
using DietFitter_backend.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace DietFitter_backend.UnitTests.DTO
{
    [TestClass]
    public class MealItemDtoTest
    {
        [TestMethod]
        public void MealItemDto_ShouldHaveCorrectDefaultValues()
        {
            var mealItem = new MealItemDto();
            
            Assert.AreEqual(string.Empty, mealItem.Food);
            Assert.AreEqual(0, mealItem.Grams);
            Assert.AreEqual(0, mealItem.ProvidedValue);
        }

        [TestMethod]
        public void MealItemDto_ShouldSetValuesCorrectly()
        {
            var mealItem = new MealItemDto
            {
                Food = "Chicken Breast",
                Grams = 150,
                ProvidedValue = 300
            };
            
            Assert.AreEqual("Chicken Breast", mealItem.Food);
            Assert.AreEqual(150, mealItem.Grams);
            Assert.AreEqual(300, mealItem.ProvidedValue);
        }

        [TestMethod]
        public void MealItemDto_ShouldBeSerializable()
        {
            var mealItem = new MealItemDto
            {
                Food = "Rice",
                Grams = 100,
                ProvidedValue = 130
            };
            
            var json = JsonSerializer.Serialize(mealItem);
            var deserializedMealItem = JsonSerializer.Deserialize<MealItemDto>(json);
            
            Assert.IsNotNull(deserializedMealItem);
            Assert.AreEqual("Rice", deserializedMealItem.Food);
            Assert.AreEqual(100, deserializedMealItem.Grams);
            Assert.AreEqual(130, deserializedMealItem.ProvidedValue);
        }

        [TestMethod]
        public void MealItemDto_ShouldBeEqual_WhenSameValues()
        {
            var item1 = new MealItemDto
            {
                Food = "Salmon",
                Grams = 200,
                ProvidedValue = 250
            };

            var item2 = new MealItemDto
            {
                Food = "Salmon",
                Grams = 200,
                ProvidedValue = 250
            };
            
            Assert.AreEqual(item1.Food, item2.Food);
            Assert.AreEqual(item1.Grams, item2.Grams);
            Assert.AreEqual(item1.ProvidedValue, item2.ProvidedValue);
        }

        [TestMethod]
        public void MealItemDto_ShouldNotBeEqual_WhenDifferentValues()
        {
            var item1 = new MealItemDto { Food = "Fish", Grams = 200, ProvidedValue = 250 };
            var item2 = new MealItemDto { Food = "Chicken", Grams = 150, ProvidedValue = 200 };
            
            Assert.AreNotEqual(item1.Food, item2.Food);
            Assert.AreNotEqual(item1.Grams, item2.Grams);
            Assert.AreNotEqual(item1.ProvidedValue, item2.ProvidedValue);
        }
    }
}
