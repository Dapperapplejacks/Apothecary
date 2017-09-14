using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data.Entity;
using System.Windows.Input;

namespace Apothecary
{
    public delegate void OilChangeHandler();

    public class EditDescriptorVM : TabVMBase
    {
        public EditDescriptorVM(CollectionViewSource viewSource) : base(viewSource)
        {

        }

        public bool CanSave()
        {
            return true;
        }

        public ICommand Save
        {
            get { return new RelayCommand(SaveContextChanges, CanSave); }
        }
    }
}
