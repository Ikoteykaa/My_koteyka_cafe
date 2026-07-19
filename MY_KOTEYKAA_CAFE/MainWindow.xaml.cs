using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MY_KOTEYKAA_CAFE
{
    public class Item
    {
        public string Description { get; set; }
        public double Price { get; set; }
    }

    public partial class MainWindow : Window
    {
        private ObservableCollection<Item> itemsList = new ObservableCollection<Item>();
        private double tipAmount = 0;
        private const int MAX_ITEMS = 5;

        public MainWindow()
        {
            InitializeComponent();
            dgItems.ItemsSource = itemsList;
            UpdateBillDisplay();
        }

        private void UpdateBillDisplay()
        {
            double netTotal = itemsList.Sum(i => i.Price);
            double gst = netTotal * 0.05;
            double total = netTotal + gst + tipAmount;

            lblNetTotal.Text = $"${netTotal:F2}";
            lblTip.Text = $"${tipAmount:F2}";
            lblGst.Text = $"${gst:F2}";
            lblTotal.Text = $"${total:F2}";
        }
    }
}