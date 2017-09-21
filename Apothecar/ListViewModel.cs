using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Apothecary
{
    public class ListViewModel : TabVMBase
    {
        private ObservableCollection<Descriptor> _descriptors;

        public ObservableCollection<Descriptor> Descriptors
        {
            get { return _descriptors; }
            set { _descriptors = value; }
        }

        public ListViewModel(CollectionViewSource viewSource) : base(viewSource)
        {
            ComboEditVM.ComboChangeEvent += UpdateComboList;
        }

        private void UpdateComboList()
        {

        }

       
    }
}
