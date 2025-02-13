using System;
using DietFitter_backend.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.Json;

namespace DietFitter_backend.UnitTests.DTO
{
    [TestClass]
    public class DietRequestTest
    {
        [TestMethod]
        public void DietRequest_ShouldHaveCorrectDefaultValues()
        {

            var request = new DietRequest();

            Assert.AreEqual(string.Empty, request.UserId);
            Assert.AreEqual(0, request.UserWeight);
            Assert.IsNull(request.UserHeight);
            Assert.AreEqual(string.Empty, request.SelectedCondition);
            Assert.IsNull(request.PreferredCategory);
        }

        [TestMethod]
        public void DietRequest_ShouldSetValuesCorrectly()
        {
            var request = new DietRequest
            {
                UserId = "user123",
                UserWeight = 80.5,
                UserHeight = 1.75,
                SelectedCondition = "odchudzanie",
                PreferredCategory = "Wegańskie"
            };
            
            Assert.AreEqual("user123", request.UserId);
            Assert.AreEqual(80.5, request.UserWeight);
            Assert.AreEqual(1.75, request.UserHeight);
            Assert.AreEqual("odchudzanie", request.SelectedCondition);
            Assert.AreEqual("Wegańskie", request.PreferredCategory);
        }

        [TestMethod]
        public void DietRequest_ShouldBeSerializable()
        {
            var request = new DietRequest
            {
                UserId = "user456",
                UserWeight = 70,
                UserHeight = 1.80,
                SelectedCondition = "niedowaga",
                PreferredCategory = "Mięso"
            };

            var json = JsonSerializer.Serialize(request);
            var deserializedRequest = JsonSerializer.Deserialize<DietRequest>(json);
            
            Assert.IsNotNull(deserializedRequest);
            Assert.AreEqual("user456", deserializedRequest.UserId);
            Assert.AreEqual(70, deserializedRequest.UserWeight);
            Assert.AreEqual(1.80, deserializedRequest.UserHeight);
            Assert.AreEqual("niedowaga", deserializedRequest.SelectedCondition);
            Assert.AreEqual("Mięso", deserializedRequest.PreferredCategory);
        }

        [TestMethod]
        public void DietRequest_ShouldBeEqual_WhenSameValues()
        {

            var request1 = new DietRequest
            {
                UserId = "user123",
                UserWeight = 75,
                UserHeight = 1.78,
                SelectedCondition = "cukrzyca",
                PreferredCategory = "Nabiał"
            };

            var request2 = new DietRequest
            {
                UserId = "user123",
                UserWeight = 75,
                UserHeight = 1.78,
                SelectedCondition = "cukrzyca",
                PreferredCategory = "Nabiał"
            };


            Assert.AreEqual(request1.UserId, request2.UserId);
            Assert.AreEqual(request1.UserWeight, request2.UserWeight);
            Assert.AreEqual(request1.UserHeight, request2.UserHeight);
            Assert.AreEqual(request1.SelectedCondition, request2.SelectedCondition);
            Assert.AreEqual(request1.PreferredCategory, request2.PreferredCategory);
        }

        [TestMethod]
        public void DietRequest_ShouldNotBeEqual_WhenDifferentValues()
        {
            var request1 = new DietRequest
            {
                UserId = "user1",
                UserWeight = 70,
                UserHeight = 1.75,
                SelectedCondition = "odchudzanie",
                PreferredCategory = "Wegańskie"
            };

            var request2 = new DietRequest
            {
                UserId = "user2",
                UserWeight = 85,
                UserHeight = 1.90,
                SelectedCondition = "niedowaga",
                PreferredCategory = "Mięso"
            };

            Assert.AreNotEqual(request1.UserId, request2.UserId);
            Assert.AreNotEqual(request1.UserWeight, request2.UserWeight);
            Assert.AreNotEqual(request1.UserHeight, request2.UserHeight);
            Assert.AreNotEqual(request1.SelectedCondition, request2.SelectedCondition);
            Assert.AreNotEqual(request1.PreferredCategory, request2.PreferredCategory);
        }
    }
}
