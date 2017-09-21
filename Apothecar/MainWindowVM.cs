using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Apothecary
{
    class MainWindowVM : INotifyPropertyChanged
    {
        public OilsViewModel oilsVM; 

        public MainWindowVM()
        {
            oilsVM = new OilsViewModel();
            RaisePropertyChanged("")
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void OnClosing()
        {

        }

        
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                RefreshContext();
            }
        }

        private void RefreshContext()
        {
            System.Windows.Data.CollectionViewSource essentialOilViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("essentialOilViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // essentialOilViewSource.Source = [generic data source]

            oilsVM.LoadOils();
            essentialOilViewSource.Source = oilsVM.Oils;
        }


        #region List Tab

        private void RefreshClick(object sender, RoutedEventArgs e)
        {
            RefreshContext();
        }

        #endregion

        #region Add/Edit Oils Tab Methods

        private void RefreshOils()
        {
            foreach (var oil in oilsVM.Oils.ToList())
            {
                if (oil == null || oil.Name == "")
                {
                    oilsVM.RemoveOil(oil);
                }
            }
            this.essentialOilDataGrid1.CommitEdit();
            this.essentialOilDataGrid1.CommitEdit();

            this.essentialOilDataGrid.Items.Refresh();
            this.essentialOilDataGrid1.Items.Refresh();
            this.descriptorsDataGrid.Items.Refresh();

            UpdateOilComboBox();
        }

        private void RefreshDescriptors()
        {
            foreach (var descriptor in context.Descriptors.Local.ToList())
            {
                if (descriptor.EssentialOil == null || descriptor.Content == "" || descriptor.Content == null)
                {
                    context.Descriptors.Remove(descriptor);
                }
            }
            this.descriptorsDataGrid.CommitEdit();
            this.descriptorsDataGrid.CommitEdit();

            context.SaveChanges();
            this.essentialOilDataGrid.Items.Refresh();
            this.essentialOilDataGrid1.Items.Refresh();
            this.descriptorsDataGrid.Items.Refresh();

            UpdateOilComboBox();
        }

        private void SaveEditClick(object sender, RoutedEventArgs e)
        {
            RefreshDescriptors();
            RefreshOils();
            RefreshContext();
        }
         
        #endregion

        #region Edit Comboes Tab

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





        private void UpdateOilComboBox()
        {
            this.essentialOilComboBox.Items.Clear();
            foreach (var item in this.essentialOilDataGrid1.Items)
            {
                this.essentialOilComboBox.Items.Add(item);
            }
        }

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

                context.Comboes.Local.Add(combo);
                context.Comboes.Local.Add(combo2);
                context.SaveChanges();
                //this.comboDataGrid.Items.Add();
            }
            catch (Exception ex)
            {
                //Already exists
            }
            //this.comboDataGrid.Items.Refresh();
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
                    value, index) => ((value.EssentialOilId1 == oil1.Id) && (value.EssentialOilId2 == oil2.Id))
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

        private void comboDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DeleteCompatibleOilButton.IsEnabled = true;
        }

        #endregion

        private void essentialOilDataGrid_Selected(object sender, RoutedEventArgs e)
        {
            //Object checkbox = ((DataGrid)sender).

            return;
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

    public class EssentialOilConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((EssentialOil)value).Name;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    }
}
