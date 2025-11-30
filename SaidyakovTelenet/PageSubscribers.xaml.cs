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
        public PageSubscribers()
        {
            InitializeComponent();
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

        public void ComboBoxFilterMaker()
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
        
        public void TextBoxSearchMaker()
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameBase.Navigate(new PageAddEdit(null));
        }

        private void ChangeBtn_Click(object sender, RoutedEventArgs e)
        {
            Manager.FrameBase.Navigate(new PageAddEdit((sender as Button).DataContext as SUBSCRIBER));
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
    }
}
