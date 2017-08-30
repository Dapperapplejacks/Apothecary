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

        //private Model1Container context = new Model1Container();
        public ListViewModel listVM;
        public EditDescriptorVM editVM;
        public ComboEditVM comboVM;

        public MainWindow()
        {
            InitializeComponent();
            System.Windows.Data.CollectionViewSource essentialOilViewSource =
                ((System.Windows.Data.CollectionViewSource)(this.FindResource("essentialOilViewSource")));

            listVM = new ListViewModel(essentialOilViewSource);
            editVM = new EditDescriptorVM(essentialOilViewSource);
            comboVM = new ComboEditVM(essentialOilViewSource);

            this.ListTab.DataContext = listVM;
            this.AddEditOilsTab.DataContext = editVM;
            this.EditComboesTab.DataContext = comboVM;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            //this.context.Dispose();
        }
        /*
        private void LoadContext()
        {
            System.Windows.Data.CollectionViewSource essentialOilViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("essentialOilViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // essentialOilViewSource.Source = [generic data source]
            context = new Model1Container();

            context.EssentialOils.Load();
            essentialOilViewSource.Source = context.EssentialOils.Local;
            UpdateOilGrids();
        }

        private void UpdateOilComboBox()
        {
            this.essentialOilComboBox.Items.Clear();
            foreach (var item in this.essentialOilDataGrid1.Items)
            {
                this.essentialOilComboBox.Items.Add(item);
            }
        }

        private void UpdateOilGrids()
        {
            foreach (var descriptor in context.Descriptors.Local.ToList())
            {
                if (descriptor.EssentialOil == null || descriptor.Content == "" || descriptor.Content == null)
                {
                    context.Descriptors.Remove(descriptor);
                }
            }

            context.SaveChanges();
            this.essentialOilDataGrid.Items.Refresh();
            this.essentialOilDataGrid1.Items.Refresh();
            this.descriptorsDataGrid.Items.Refresh();

            UpdateOilComboBox();
        }


        #region Add/Edit Oils Tab Methods

        private void SaveNewOilsDescriptors_Click(object sender, RoutedEventArgs e)
        {
            UpdateOilGrids();

        }
        #endregion

        #region Edit Comboes        

        private void AddCompatibleOil_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EssentialOil selectedOil1 = this.essentialOilDataGrid2.SelectedItem as EssentialOil;
                EssentialOil selectedOil2 = this.essentialOilComboBox.SelectedItem as EssentialOil;

                EssentialOil oil1 = context.EssentialOils.Local.Where<EssentialOil>(
                    (value, index) => value.Name.Equals(selectedOil1.Name)).First();
                EssentialOil oil2 = context.EssentialOils.Local.Where<EssentialOil>(
                    (value, index) => value.Name.Equals(selectedOil2.Name)).First();

                if (oil1.Id == oil2.Id)
                {
                    return;
                }

                Combo combo = new Combo();
                combo.EssentialOil1 = oil1;
                combo.EssentialOil2 = oil2;
                combo.EssentialOilId1 = oil1.Id;
                combo.EssentialOilId2 = oil2.Id;

                Combo combo2 = new Combo();
                combo2.EssentialOil1 = oil2;
                combo2.EssentialOil2 = oil1;
                combo2.EssentialOilId1 = oil2.Id;
                combo2.EssentialOilId2 = oil1.Id;

                try
                {
                    context.Comboes.Local.Add(combo);
                    context.Comboes.Local.Add(combo2);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    //Already exists, remove comboes from local
                    context.Comboes.Local.Remove(combo);
                    context.Comboes.Local.Remove(combo2); 
                    context.SaveChanges();
                }
                
            }
            catch (Exception ex)
            {
                //Already exists reload context
            }
            
        }

        private void DeleteCompatibleOilButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Combo selectedCombo = this.comboDataGrid.SelectedItem as Combo;

                EssentialOil oil1 = context.EssentialOils.Local.Where<EssentialOil>(
                    (value, index) => value.Name.Equals(selectedCombo.EssentialOil1.Name)).First();
                EssentialOil oil2 = context.EssentialOils.Local.Where<EssentialOil>(
                    (value, index) => value.Name.Equals(selectedCombo.EssentialOil2.Name)).First();

                Combo[] comboes = context.Comboes.Local.Where<Combo>((
                    value, index) => ((value.EssentialOilId1==oil1.Id) && (value.EssentialOilId2==oil2.Id))
                    || ((value.EssentialOilId1 == oil2.Id) && (value.EssentialOilId2 == oil1.Id))).ToArray<Combo>();
                
                context.Comboes.Local.Remove(comboes[0]);
                context.Comboes.Local.Remove(comboes[1]);
                context.SaveChanges();
                this.comboDataGrid.Items.Refresh();
            }
            catch (Exception ex)
            {
                //Not selected
            }
        }

        #endregion

        private void comboDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DeleteCompatibleOilButton.IsEnabled = true;
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            LoadContext();
            UpdateOilGrids();
        }

       
        */
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
