using System;
using DietFitter_backend.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace DietFitter_backend.UnitTests.DTO
{
    [TestClass]
    public class UserDietDtoTests
    {
        [TestMethod]
        public void UserDietDto_ShouldHaveCorrectDefaultValues()
        {

            var dto = new UserDietDto();
            Assert.AreEqual(string.Empty, dto.Food);
            Assert.AreEqual(0, dto.Grams);
            Assert.AreEqual(0, dto.ProvidedValue);
            Assert.AreEqual(string.Empty, dto.MealType);
        }

        [TestMethod]
        public void UserDietDto_ShouldSetValuesCorrectly()
        {

            var dto = new UserDietDto
            {
                Food = "Chicken",
                Grams = 150,
                ProvidedValue = 200,
                MealType = "Dinner"
            };


            Assert.AreEqual("Chicken", dto.Food);
            Assert.AreEqual(150, dto.Grams);
            Assert.AreEqual(200, dto.ProvidedValue);
            Assert.AreEqual("Dinner", dto.MealType);
        }

        [TestMethod]
        public void UserDietDto_ShouldBeSerializable()
        {

            var dto = new UserDietDto
            {
                Food = "Rice",
                Grams = 100,
                ProvidedValue = 130,
                MealType = "Lunch"
            };


            var json = JsonSerializer.Serialize(dto);
            var deserializedDto = JsonSerializer.Deserialize<UserDietDto>(json);


            Assert.IsNotNull(deserializedDto);
            Assert.AreEqual("Rice", deserializedDto.Food);
            Assert.AreEqual(100, deserializedDto.Grams);
            Assert.AreEqual(130, deserializedDto.ProvidedValue);
            Assert.AreEqual("Lunch", deserializedDto.MealType);
        }

        [TestMethod]
        public void UserDietDto_ShouldBeEqual_WhenSameValues()
        {

            var dto1 = new UserDietDto
            {
                Food = "Fish",
                Grams = 200,
                ProvidedValue = 250,
                MealType = "Dinner"
            };

            var dto2 = new UserDietDto
            {
                Food = "Fish",
                Grams = 200,
                ProvidedValue = 250,
                MealType = "Dinner"
            };


            Assert.AreEqual(dto1.Food, dto2.Food);
            Assert.AreEqual(dto1.Grams, dto2.Grams);
            Assert.AreEqual(dto1.ProvidedValue, dto2.ProvidedValue);
            Assert.AreEqual(dto1.MealType, dto2.MealType);
        }

        [TestMethod]
        public void UserDietDto_ShouldNotBeEqual_WhenDifferentValues()
        {
     
            var dto1 = new UserDietDto { Food = "Fish", Grams = 200, ProvidedValue = 250, MealType = "Dinner" };
            var dto2 = new UserDietDto { Food = "Chicken", Grams = 150, ProvidedValue = 200, MealType = "Lunch" };


            Assert.AreNotEqual(dto1.Food, dto2.Food);
            Assert.AreNotEqual(dto1.Grams, dto2.Grams);
            Assert.AreNotEqual(dto1.ProvidedValue, dto2.ProvidedValue);
            Assert.AreNotEqual(dto1.MealType, dto2.MealType);
        }
    }
}
