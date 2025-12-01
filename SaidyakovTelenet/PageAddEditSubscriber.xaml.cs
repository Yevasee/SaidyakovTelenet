using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SaidyakovTelenet
{
    /// <summary>
    /// Логика взаимодействия для PageAddEdit.xaml
    /// </summary>
    public partial class PageAddEditSubscriber : Page
    {
        private SUBSCRIBER _currentSubscriber = new SUBSCRIBER();
        public PageAddEditSubscriber(SUBSCRIBER SelectedSubscriber)
        {
            InitializeComponent();
            if (SelectedSubscriber != null)
            {
                _currentSubscriber = SelectedSubscriber;
            }
            else
            {
                TextBoxSubscriberId.Foreground = new SolidColorBrush(SystemColors.ControlColor);
                TextBoxSubscriberId.Background = new SolidColorBrush(SystemColors.ControlColor);
                _currentSubscriber.SUBSCRIBER_LASTNAME = "Иванов";
                _currentSubscriber.SUBSCRIBER_FIRSTNAME = "Иван";
                _currentSubscriber.SUBSCRIBER_PATRONYMIC = "Иванович";
                _currentSubscriber.SUBSCRIBER_ADDRESS = "Стройбург, ул. Рабочих, 42";
                _currentSubscriber.SUBSCRIBER_CONNECTION_DATE = DateTime.Today;
                _currentSubscriber.SUBSCRIBER_STATUS = "Активен";
            }
            ComboBoxSubscriberTariff_Load();
            ComboBoxSubscriberTariff_Select();
            switch (_currentSubscriber.SUBSCRIBER_STATUS)
            {
                case "Активен": ComboBoxStatusOfSubscriber.SelectedIndex = 0; break;
                case "Приостановлен": ComboBoxStatusOfSubscriber.SelectedIndex = 1; break;
                case "Заблокирован": ComboBoxStatusOfSubscriber.SelectedIndex = 2; break;
            }
            DataContext = _currentSubscriber;
        }

        private void ComboBoxStatusOfSubscriber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            switch (ComboBoxStatusOfSubscriber.SelectedIndex)
            {
                case 0: _currentSubscriber.SUBSCRIBER_STATUS = "Активен"; break;
                case 1: _currentSubscriber.SUBSCRIBER_STATUS = "Приостановлен"; break;
                case 2: _currentSubscriber.SUBSCRIBER_STATUS = "Заблокирован"; break;
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(_currentSubscriber.SUBSCRIBER_LASTNAME))
            {
                errors.AppendLine("Укажите фамилию абонента");
            }
            if (string.IsNullOrWhiteSpace(_currentSubscriber.SUBSCRIBER_FIRSTNAME))
            {
                errors.AppendLine("Укажите имя абонента");
            }
            if (string.IsNullOrWhiteSpace(_currentSubscriber.SUBSCRIBER_ADDRESS))
            {
                errors.AppendLine("Укажите адрес абонента");
            }
            if (string.IsNullOrWhiteSpace(TextBoxSubscriberConnectionDate.Text))
            {
                errors.AppendLine("Укажите дату подключения");
            }
            if (ComboBoxStatusOfSubscriber.SelectedItem == null)
            {
                errors.AppendLine("Укажите статус абонента");
            }
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if (_currentSubscriber.SUBSCRIBER_ID == 0)
            {
                TelenetDBEntities.GetContext().SUBSCRIBER.Add(_currentSubscriber);
            }


            try
            {
                TelenetDBEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранена");
                Manager.FrameBase.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            var _subscriber = (sender as Button).DataContext as SUBSCRIBER;

            var _subscriberStatus = _subscriber.SUBSCRIBER_STATUS;

            if (_subscriberStatus.Equals("Активен") || _subscriberStatus.Equals("Приостановлен"))
                MessageBox.Show("Невозможно выполнить удаление, так как тариф используется абонентом");
            else
            {
                var _subscriberTariff = TelenetDBEntities.GetContext().TARIFF.ToList();
                var _subscriberPhone = TelenetDBEntities.GetContext().PHONE_NUMBER.ToList();
                var _subscriberServiceRequest = TelenetDBEntities.GetContext().SERVICE_REQUEST.ToList();
                var _subscriberPayment = TelenetDBEntities.GetContext().PAYMENT.ToList();
                _subscriberTariff = _subscriberTariff.
                    Where(p => p.TARIFF_ID == _subscriber.SUBSCRIBER_TARIFF_ID).ToList();
                _subscriberPhone = _subscriberPhone.
                    Where(p => p.SUBSCRIBER_ID == _subscriber.SUBSCRIBER_ID).ToList();
                _subscriberServiceRequest = _subscriberServiceRequest.
                    Where(p => p.SUBSCRIBER_ID == _subscriber.SUBSCRIBER_ID).ToList();
                _subscriberPayment = _subscriberPayment.
                    Where(p => p.SUBSCRIBER_ID == _subscriber.SUBSCRIBER_ID).ToList();

                if (_subscriberTariff.Count > 0 || _subscriberPhone.Count > 0 || _subscriberServiceRequest.Count > 0 || _subscriberPayment.Count > 0)
                    MessageBox.Show("Невозможно выполнить удаление, так как присутствуют связанные записи");
                else
                {
                    if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!",
                        MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            TelenetDBEntities.GetContext().SUBSCRIBER.Remove(_subscriber);
                            TelenetDBEntities.GetContext().SaveChanges();

                            MessageBox.Show("Информация удалена!");
                            Manager.FrameBase.GoBack();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString());
                        }
                    }
                }
            }
        }

        private void ComboBoxSubscriberTariff_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ComboBoxSubscriberTariff.SelectedItem.ToString().Equals("Не указан"))
            {
                var _tariffs = TelenetDBEntities.GetContext().TARIFF.ToList();
                _tariffs = _tariffs.Where(t => t.TARIFF_NAME.Equals(ComboBoxSubscriberTariff.SelectedItem.ToString())).ToList();
                _currentSubscriber.SUBSCRIBER_TARIFF_ID = _tariffs.First().TARIFF_ID;
            } else
            {
                _currentSubscriber.SUBSCRIBER_TARIFF_ID = null;
            }
        }
        private void ComboBoxSubscriberTariff_Load()
        {
            List<string> tariffNames = TelenetDBEntities.GetContext().TARIFF.Select(t => t.TARIFF_NAME).ToList();
            tariffNames.Add("Не указан");

            ComboBoxSubscriberTariff.ItemsSource = tariffNames;
        }

        private void ComboBoxSubscriberTariff_Select()
        {
            if (!string.IsNullOrEmpty(_currentSubscriber._SUBSCRIBER_TARIFF_NAME))
            {
                ComboBoxSubscriberTariff.SelectedItem = _currentSubscriber._SUBSCRIBER_TARIFF_NAME;
            }
            else
            {
                ComboBoxSubscriberTariff.SelectedItem = "Не указан";
            }
        }
    }
}
