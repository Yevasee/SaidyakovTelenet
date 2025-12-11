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
    /// Логика взаимодействия для PageSubscribers.xaml
    /// </summary>
    public partial class PageSubscribers : Page
    {
        List<SUBSCRIBER> listSubscribersForPage = new List<SUBSCRIBER>();
        int countOfRecordsInTotal = 0;
        int countOfRecordsOnPage = 0;

        public User currentUser;
        public User CurrentUser
        {
            get { return currentUser; }
        }
        public PageSubscribers(User user = null)
        {
            InitializeComponent();

            this.DataContext = this;


            if (user == null)
            {
                TextBlockFIO.Text = "гость";
                TextBlockRole.Text = "Гость";
                BtnAdd.Visibility = Visibility.Hidden;
                currentUser = null;
            }
            else
            {
                currentUser = user;
                TextBlockFIO.Text = user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
                TextBlockRole.Text = user.UserRole.RoleName;
            }

            ComboBoxSorter.SelectedIndex = 0;
            ComboBoxFilter.SelectedIndex = 0;
            UpdateSubscribers();
        }
        public void UpdateSubscribers()
        {
            listSubscribersForPage = TelenetDBEntities.GetContext().SUBSCRIBER.ToList();
            countOfRecordsInTotal = listSubscribersForPage.Count;
            TextBlockCountOfRecordsInTotal.Text = countOfRecordsInTotal.ToString();
            ComboBoxSorterMaker();
            ComboBoxFilterMaker();
            TextBoxSearchMaker();

            countOfRecordsOnPage = listSubscribersForPage.Count;
            TextBlockCountOfRecordsOnPage.Text = countOfRecordsOnPage.ToString();

            // Принудительно обновляем видимость кнопок
            if (listViewSubscribers.ItemsSource != null)
            {
                foreach (var item in listViewSubscribers.Items)
                {
                    var container = listViewSubscribers.ItemContainerGenerator.ContainerFromItem(item);
                    if (container != null)
                    {
                        var button = FindVisualChild<Button>(container);
                        if (button != null && button.Name == "BtnChange")
                        {
                            button.Visibility = currentUser == null ? Visibility.Collapsed : Visibility.Visible;
                        }
                    }
                }
            }
        }

        // Вспомогательный метод для поиска в визуальном дереве
        private T FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T result)
                    return result;

                var descendant = FindVisualChild<T>(child);
                if (descendant != null)
                    return descendant;
            }
            return null;
        }

        private void ComboBoxSorter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSubscribers();
        }

        private void ComboBoxFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSubscribers();
        }

        private void TextBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateSubscribers();
        }

        private void ComboBoxSorterMaker()
        {
            switch (ComboBoxSorter.SelectedIndex)
            {
                case 0: listSubscribersForPage = listSubscribersForPage.ToList(); break;
                case 1: listSubscribersForPage = listSubscribersForPage.OrderBy(p => p.subscriberFullName).ToList(); break;
                case 2: listSubscribersForPage = listSubscribersForPage.OrderByDescending(p => p.subscriberFullName).ToList(); break;
            }

            listViewSubscribers.ItemsSource = listSubscribersForPage;
        }

        private void ComboBoxFilterMaker()
        {
            switch (ComboBoxFilter.SelectedIndex)
            {
                case 0: listSubscribersForPage = listSubscribersForPage.ToList(); break;
                case 1: listSubscribersForPage = listSubscribersForPage.Where(p => (p.SUBSCRIBER_STATUS.Equals("Активен"))).ToList(); break;
                case 2: listSubscribersForPage = listSubscribersForPage.Where(p => (p.SUBSCRIBER_STATUS.Equals("Приостановлен"))).ToList(); break;
                case 3: listSubscribersForPage = listSubscribersForPage.Where(p => (p.SUBSCRIBER_STATUS.Equals("Заблокирован"))).ToList(); break;
            }

            listViewSubscribers.ItemsSource = listSubscribersForPage;
        }
        
        private void TextBoxSearchMaker()
        {
            string[] searchText = TextBoxSearch.Text.ToLower().Split(' ');
            for (int i = 0; i < searchText.Length; i++)
            {
                listSubscribersForPage = listSubscribersForPage.Where(p => (p.SUBSCRIBER_ID.ToString().ToLower().Contains(searchText[i])
                                                                   || p.subscriberFullName.ToLower().Contains(searchText[i])
                                                                   || p.SUBSCRIBER_ADDRESS.ToLower().Contains(searchText[i])
                                                                   || p.SUBSCRIBER_CONNECTION_DATE.ToString().ToLower().Contains(searchText[i])
                                                                   || p.SUBSCRIBER_STATUS.ToLower().Contains(searchText[i])
                                                                   )).ToList();
            }

            listViewSubscribers.ItemsSource = listSubscribersForPage;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameBase.Navigate(new PageAddEditSubscriber(null));
        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameBase.Navigate(new PageAddEditSubscriber((sender as Button).DataContext as SUBSCRIBER));
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                TelenetDBEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                listViewSubscribers.ItemsSource = TelenetDBEntities.GetContext().SUBSCRIBER.ToList();
                UpdateSubscribers();
            }
        }

        private void BtnChange_Loaded(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                button.Visibility = currentUser == null ? Visibility.Collapsed : Visibility.Visible;
            }
        }

        private void BtnServiceRequest_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameBase.Navigate(new PageSubscriberServiceRequest((sender as Button).DataContext as SUBSCRIBER));
        }

        private void BtnServiceRequest_Loaded(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                button.Visibility = currentUser == null ? Visibility.Collapsed : Visibility.Visible;
            }
        }
    }
}
