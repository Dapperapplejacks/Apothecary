using System;
using System.Data.Entity;
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
using System.Collections.ObjectModel;

namespace Apothecary
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Model1Container context = new Model1Container();

        //List Tab

        //Add/Edit tab
        private bool oilSelected;
        private string name;
        private Descriptor[] descriptors;
        //Edit comboes tab



        //private ExcelHandler excelHandler;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddNewEntryClick(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource essentialOilViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("essentialOilViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // essentialOilViewSource.Source = [generic data source]

            context.EssentialOils.Load();
            essentialOilViewSource.Source = context.EssentialOils.Local;
            System.Windows.Data.CollectionViewSource descriptorViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("descriptorViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // descriptorViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource comboViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("comboViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // comboViewSource.Source = [generic data source]
        }

        private void DeleteComboButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var oil in context.EssentialOils.Local.ToList())
            {
                if (oil == null)
                {
                    context.EssentialOils.Remove(oil);
                }
            }
            context.SaveChanges();

            //Refresh items in container
            
        }

        #region Add/Edit Oils Tab Methods

        private void EnterNewOilTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            name = e.Changes.ToString();
        }

        private void SelectExistingOilComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //selected = e.AddedItems
        }

        private void AddDescriptorButton_Click(object sender, RoutedEventArgs e)
        {
            Descriptor desc = new Descriptor();
            desc.Content = this.EnterDescriptorTextBox.Text;
            this.EnterDescriptorTextBox.Text = "Enter Descriptor";
            //this.DescriptorsListView.Items.Add(desc);
            //this.DescriptorsListView.Items.Refresh();
        }

        private void AddOilButton_Click(object sender, RoutedEventArgs e)
        {
            if (name != null)
            {
                EssentialOil oil = new EssentialOil();
                oil.Name = name;

                context.EssentialOils.Add(oil);
            }
            else if (oilSelected)
            {

            }
            else
            {
                MessageBox.Show("Please enter new Oil name or select an existing one.", "Error");
            }
        }

        private void DeleteOilButton_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion


        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            this.context.Dispose();
        }

       

    }

    public class DescriptorConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string ret = "";
            ObservableCollection<Descriptor> coll = (ObservableCollection<Descriptor>)value;
            //value = (ObservableCollection<Descriptor>)value;
            string[] strArray = new string[coll.Count];

            for (int i = 0; i < strArray.Length; i++)
            {
                strArray[i] = coll[i].Content;
            }

            ret = String.Join(", ", strArray);
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
