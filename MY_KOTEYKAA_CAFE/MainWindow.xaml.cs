using System.Collections.ObjectModel;
using System.IO;
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
    
    private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (itemsList.Count >= MAX_ITEMS)
            {
                MessageBox.Show("Bill is full (Max 5 items).");
                return;
            }

            string desc = txtDesc.Text.Trim();

            if (desc.Length < 3 || desc.Length > 20)
            {
                MessageBox.Show("Incorrect description.");
                return;
            }

            if (!double.TryParse(txtPrice.Text, out double price) || price <= 0)
            {
                MessageBox.Show("Incorrect price.");
                return;
            }

            itemsList.Add(new Item
            {
                Description = desc,
                Price = price
            });

            txtDesc.Clear();
            txtPrice.Clear();

            UpdateBillDisplay();
        }
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (itemsList.Count == 0)
            {
                MessageBox.Show("Bill is empty.");
                return;
            }

            if (dgItems.SelectedItem is Item selectedItem)
            {
                itemsList.Remove(selectedItem);
                UpdateBillDisplay();
            }
        }
        private void BtnApplyTip_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(txtTip.Text, out double value))
                return;

            if (rbPercent.IsChecked == true)
            {
                double sum = itemsList.Sum(i => i.Price);
                tipAmount = sum * value / 100;
            }
            else
            {
                tipAmount = value;
            }

            UpdateBillDisplay();
        }

        private void BtnNoTip_Click(object sender, RoutedEventArgs e)
        {
            tipAmount = 0;
            txtTip.Clear();
            UpdateBillDisplay();
        }
        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            if (itemsList.Count == 0)
                return;

            itemsList.Clear();
            tipAmount = 0;

            UpdateBillDisplay();
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (itemsList.Count == 0)
            {
                MessageBox.Show("Bill is empty.");
                return;
            }

            string fileName = txtFileName.Text.Trim();
            if (!IsValidFileName(fileName))
            {
                MessageBox.Show("Incorrect filename.");
                return;
            }

            fileName += ".txt";

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                double sum = 0;

                writer.WriteLine("===== BILL =====");
                writer.WriteLine();

                foreach (var item in itemsList)
                {
                    writer.WriteLine($"{item.Description} - {item.Price:F2}");
                    sum += item.Price;
                }

                writer.WriteLine();
                writer.WriteLine($"Subtotal: {sum:F2}");
                writer.WriteLine($"Tip: {tipAmount:F2}");
                writer.WriteLine($"Total: {(sum + tipAmount):F2}");
            }

        }
        private bool IsValidFileName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Length < 1 || name.Length > 10)
                return false;

            char[] forbidden = { '\\', '/', ':', '*', '?', '"', '<', '>', '|' };

            return name.IndexOfAny(forbidden) == -1;
        }
    }
}