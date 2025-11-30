using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaidyakovTelenet;
using System;
using System.Windows;

namespace UnitTestProject1
{
    [TestClass]
    public class PageAddEditTests
    {
        [TestMethod]
        public void SaveButton_Click_WithEmptyLastName_ShowsErrorMessage()
        {
            var page = new PageAddEdit(null);
            var subscriber = new SUBSCRIBER
            {
                SUBSCRIBER_FIRSTNAME = "Test",
                SUBSCRIBER_ADDRESS = "Test Address",
                SUBSCRIBER_CONNECTION_DATE = DateTime.Today
            };
            page.DataContext = subscriber;

            page.ButtonSave_Click(null, new RoutedEventArgs());
        }

        [TestMethod]
        public void SaveButton_Click_WithValidData_SavesSubscriber()
        {
            var page = new PageAddEdit(null);
            var subscriber = new SUBSCRIBER
            {
                SUBSCRIBER_LASTNAME = "Test",
                SUBSCRIBER_FIRSTNAME = "Test",
                SUBSCRIBER_ADDRESS = "Test Address",
                SUBSCRIBER_CONNECTION_DATE = DateTime.Today,
                SUBSCRIBER_STATUS = "Активен"
            };
            page.DataContext = subscriber;

            page.ButtonSave_Click(null, new RoutedEventArgs());
        }  

        [TestMethod]
        public void DeleteButton_Click_ActiveSubscriber_ShowsWarning()
        {
            var activeSubscriber = new SUBSCRIBER { SUBSCRIBER_STATUS = "Активен" };
            var page = new PageAddEdit(activeSubscriber);

            page.ButtonDelete_Click(null, new RoutedEventArgs());
        }
    }
}
