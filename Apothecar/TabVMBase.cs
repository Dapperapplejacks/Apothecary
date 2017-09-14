using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Data.Entity;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Apothecary
{
    public class TabVMBase : IDisposable
    {
        
        public delegate void ModelChangeHandler();
        public static event ModelChangeHandler ModelChangeEvent;
        
        protected Model1Container _context;
        private CollectionViewSource _viewSource;

        public CollectionViewSource ViewSource
        {
            get { return _viewSource; }
            set { _viewSource = value; }
        }

        /// <summary>
        /// FROM EDIT DESCRIPTOR VM
        /// </summary>

        private EssentialOilVM _comboBoxOil;
        private EssentialOilVM _selectedOil1;
        private Combo _selectedCombo;

        public ObservableCollection<EssentialOil> _comboBoxOils;


        public EssentialOil ComboBoxOil
        {
            get
            {
                if (_comboBoxOil == null)
                {
                    return null;
                }
                return _comboBoxOil.Oil;
            }
            set { _comboBoxOil = new EssentialOilVM(value); }
        }

        public EssentialOil SelectedOil1
        {
            get
            {
                if (_selectedOil1 == null)
                {
                    return null;
                }
                return _selectedOil1.Oil;
            }
            set { _selectedOil1 = new EssentialOilVM(value); }
        }

        public Combo SelectedCombo
        {
            get {return _selectedCombo; }
            set {_selectedCombo = value;}
        }

        public ObservableCollection<EssentialOil> ComboBoxOils
        {
            get { return _comboBoxOils; }
            set { _comboBoxOils = value; }
        }


        ///////////////////////////////////////////////////////////////////////////
        



        public TabVMBase(CollectionViewSource viewSource)
        {
            _context = new Model1Container();
            _context.EssentialOils.Load();

            _viewSource = viewSource;
            _viewSource.Source = _context.EssentialOils.Local;

            _comboBoxOils = new ObservableCollection<EssentialOil>();
            ReloadComboBox();
        }

        private void ReloadComboBox()
        {
            _comboBoxOils.Clear();

            foreach (EssentialOil oil in _context.EssentialOils)
            {
                _comboBoxOils.Add(oil);
            }
        }

        protected void SaveContextChanges()
        {
            //_context.EssentialOils.Local = (ObservableCollection<EssentialOil>)_viewSource.Source;

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

            if (TabVMBase.ModelChangeEvent != null)
            {
                ModelChangeEvent();
            }
        }

        public bool CanSave()
        {
            return true;
        }

        public ICommand Save
        {
            get { return new RelayCommand(SaveContextChanges, CanSave); }
        }

        private void AddExecute()
        {
            Combo combo1 = new Combo();
            Combo combo2 = new Combo();
            try
            {

                if (_selectedOil1.Id == _comboBoxOil.Id)
                {
                    return;
                }

                combo1.EssentialOil1 = _selectedOil1.Oil;
                combo1.EssentialOilId1 = _selectedOil1.Id;
                combo1.EssentialOil2 = _comboBoxOil.Oil;
                combo1.EssentialOilId2 = _comboBoxOil.Id;

                combo2.EssentialOil1 = _comboBoxOil.Oil;
                combo2.EssentialOilId1 = _comboBoxOil.Id;
                combo2.EssentialOil2 = _selectedOil1.Oil;
                combo2.EssentialOilId2 = _selectedOil1.Id;
            }
            catch (Exception)
            {
                //Essential Oil not selected for either combobox or from list
            }

            try
            {
                _context.Comboes.Local.Add(combo1);
                _context.Comboes.Local.Add(combo2);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                //Already exists, remove comboes from local
                _context.Comboes.Local.Remove(combo1);
                _context.Comboes.Local.Remove(combo2);
                _context.SaveChanges();
            }

        }

        public bool CanAdd()
        {
            return (_comboBoxOil != null && _comboBoxOil.Name != "");
        }

        public ICommand Add
        {
            get { return new RelayCommand(AddExecute, CanAdd); }
        }

        public void DeleteExecute()
        {
            try
            {
                
                /*
                EssentialOil oil1 = _context.EssentialOils.Local.Where<EssentialOil>(
                    (value, index) => value.Name.Equals(selectedCombo.EssentialOil1.Name)).First();
                EssentialOil oil2 = _context.EssentialOils.Local.Where<EssentialOil>(
                    (value, index) => value.Name.Equals(selectedCombo.EssentialOil2.Name)).First();
                */
                Combo[] comboes = _context.Comboes.Local.Where<Combo>((
                    value, index) => ((value.EssentialOilId1 == _selectedCombo.EssentialOilId1) && (value.EssentialOilId2 == _selectedCombo.EssentialOilId2))
                    || ((value.EssentialOilId1 == _selectedCombo.EssentialOilId2) && (value.EssentialOilId2 == _selectedCombo.EssentialOilId1))).ToArray<Combo>();

                _context.Comboes.Local.Remove(comboes[0]);
                _context.Comboes.Local.Remove(comboes[1]);
                _context.SaveChanges();
                
            }
            catch (Exception ex)
            {
                //Not selected
            }
        }

        public bool CanDelete()
        {
            return (_selectedCombo != null);
        }

        public ICommand Delete
        {
            get { return new RelayCommand(DeleteExecute, CanDelete); }
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
