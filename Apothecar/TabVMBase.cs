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
    class TabVMBase : IDisposable
    {
        Model1Container _context;
        CollectionViewSource _viewSource;

        public CollectionViewSource ViewSource
        {
            get { return _viewSource; }
            set { _viewSource = value; }
        }

        public TabVMBase(CollectionViewSource viewSource)
        {
            _context = new Model1Container();
            _context.EssentialOils.Load();

            _viewSource = viewSource;
            _viewSource.Source = _context.EssentialOils.Local;
        }

        void SaveContextChanges()
        {
            //Check if there are any empty descriptors and get rid of them
            foreach (var descriptor in _context.Descriptors.Local.ToList())
            {
                if (descriptor.EssentialOil == null || descriptor.Content == "" || descriptor.Content == null)
                {
                    _context.Descriptors.Remove(descriptor);
                }
            }
            //Same with oils
            foreach (var oil in _context.EssentialOils.Local.ToList())
            {
                if (oil == null || oil.Name == "")
                {
                    _context.EssentialOils.Remove(oil);
                }
            }

            //Save the changes to the context, which will update database
            _context.SaveChanges();
        }

        bool CanSave()
        {
            return true;
        }

        public ICommand Save
        {
            get { return new RelayCommand(SaveContextChanges, CanSave); }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
