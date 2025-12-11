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
    /// Логика взаимодействия для PageSubscriberServiceRequest.xaml
    /// </summary>
    public partial class PageSubscriberServiceRequest : Page
    {
        List<SERVICE_REQUEST> listServiceRequestsForListView = new List<SERVICE_REQUEST>();
        int countOfRecordsInTotal = 0;
        int countOfRecordsOnPage = 0;

        private SUBSCRIBER _currentSubscriber = new SUBSCRIBER();
        public PageSubscriberServiceRequest(SUBSCRIBER SelectedSubscriber)
        {
            InitializeComponent();
            _currentSubscriber = SelectedSubscriber;
            UpdateSubscribers();
            DataContext = _currentSubscriber;
        }
        public void UpdateSubscribers()
        {
            listServiceRequestsForListView = TelenetDBEntities.GetContext().SERVICE_REQUEST.Where(s => s.SUBSCRIBER_ID == _currentSubscriber.SUBSCRIBER_ID).ToList();
            countOfRecordsInTotal = listServiceRequestsForListView.Count;
            //TextBlockCountOfRecordsInTotal.Text = countOfRecordsInTotal.ToString();
            //ComboBoxSorterMaker();
            //ComboBoxFilterMaker();
            //TextBoxSearchMaker();

            countOfRecordsOnPage = listServiceRequestsForListView.Count;
            //TextBlockCountOfRecordsOnPage.Text = countOfRecordsOnPage.ToString();
            listViewServiceRequests.ItemsSource = listServiceRequestsForListView;
        }

    }
}
