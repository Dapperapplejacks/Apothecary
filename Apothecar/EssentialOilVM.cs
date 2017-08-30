using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apothecary
{
    class EssentialOilVM : INotifyPropertyChanged
    {
        #region Properties
        EssentialOil _oil;
        ObservableCollection<Descriptor> _descriptors;
        ObservableCollection<Descriptor> _comboes;

        public EssentialOil Oil
        {
            get { return _oil; }
            set { _oil = value; }
        }

        public ObservableCollection<Descriptor> Descriptors
        {
            get { return _oil.Descriptors; }
            set
            {
                _oil.Descriptors = value;
                RaisePropertyChanged("Descriptors");
            }
        }

        public ObservableCollection<Combo> Comboes
        {
            get { return _oil.Comboes; }
            set
            {
                _oil.Comboes = value;
                RaisePropertyChanged("Comboes");
            }
        }

        public int Id
        {
            get { return _oil.Id; }
            set
            {
                _oil.Id = value;
                RaisePropertyChanged("Id");
            }
        }

        public string Name
        {
            get { return _oil.Name; }
            set
            {
                _oil.Name = value;
                RaisePropertyChanged("Name");
            }
        }

        private bool isNew = true;
        public bool IsNew
        {
            get { return isNew; }
            set
            {
                isNew = value;
                RaisePropertyChanged("IsNew");
            }
        }
        private bool isSelected = false;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged("IsSelected");
            }
        }
        private bool isDeleted = false;

        public bool IsDeleted
        {
            get { return isDeleted; }
            set
            {
                isDeleted = value;
                RaisePropertyChanged("IsDeleted");
            }
        }

        #endregion

        #region Constructor
        public EssentialOilVM()
        {
            _oil = new EssentialOil();
        }

        #endregion



        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
