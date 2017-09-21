using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.ComponentModel;

namespace Apothecary
{
    class OilsViewModel : INotifyPropertyChanged
    {

        private Model1Container context = new Model1Container();
        public ObservableCollection<OilVM> Oils {get; set;}

        public OilsViewModel()
        {
            LoadOils();
            
        }

        public void LoadOils()
        {
            context.EssentialOils.Load();
            foreach (EssentialOil oil in context.EssentialOils)
            {
                Oils.Add(new OilVM { EssentialOil = oil, IsNew = false });
            }
            RaisePropertyChanged("Oils");

        }

        public void Dispose()
        {
            context.Dispose();
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }

        public void RemoveOil(EssentialOil oil)
        {
            context.EssentialOils.Local.Remove(oil);
            Oils.Remove(oil);
            context.SaveChanges();
        }

        public void AddOil(EssentialOil oil)
        {
            context.EssentialOils.Local.Add(oil);
            Oils.Add(oil);
            context.SaveChanges();
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
