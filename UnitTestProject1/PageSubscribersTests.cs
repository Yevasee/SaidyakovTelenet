using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaidyakovTelenet;
using System;
using System.Collections.Generic;

namespace UnitTestProject1
{
    [TestClass]
    public class PageSubscribersTests
    {
        [TestMethod]
        public void ComboBoxFilterMaker_FilterByActiveStatus_ReturnsOnlyActiveSubscribers()
        {
            var page = new PageSubscribers();
            var testData = new List<SUBSCRIBER>
        {
            new SUBSCRIBER { SUBSCRIBER_STATUS = "Активен" },
            new SUBSCRIBER { SUBSCRIBER_STATUS = "Заблокирован" },
            new SUBSCRIBER { SUBSCRIBER_STATUS = "Активен" }
        };

            page.DataContext = testData;

            page.ComboBoxFilterMaker();
        }

        [TestMethod]
        public void SubscriberFullName_CombinesNamesCorrectly_ReturnTrue()
        {
            // Arrange
            var subscriber = new SUBSCRIBER
            {
                SUBSCRIBER_LASTNAME = "Иванов",
                SUBSCRIBER_FIRSTNAME = "Иван",
                SUBSCRIBER_PATRONYMIC = "Иванович"
            };

            // Act & Assert
            Assert.AreEqual("Иванов Иван Иванович", subscriber.subscriberFullName);
        }
    }
}
