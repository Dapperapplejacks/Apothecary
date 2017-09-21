using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Apothecary
{
    public class ComboEditVM : TabVMBase 
    {
        public delegate void ComboChangeHandler();
        public static event ComboChangeHandler ComboChangeEvent;


        private EssentialOilVM _comboBoxOil;
        private EssentialOilVM _selectedOil;
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

        public EssentialOil SelectedOil
        {
            get 
            {
                if (_selectedOil == null)
                {
                    return null;
                }
                return _selectedOil.Oil; 
            }
            set { _selectedOil = new EssentialOilVM(value); }
        }

        public ObservableCollection<EssentialOil> ComboBoxOils
        {
            get { return _comboBoxOils; }
            set { _comboBoxOils = value; }
        }

        public ComboEditVM(CollectionViewSource viewSource) : base(viewSource)
        {
            _comboBoxOils = new ObservableCollection<EssentialOil>();
            ReloadComboBox();

            TabVMBase.ModelChangeEvent += ReloadComboBox;
        }

        private void ReloadComboBox()
        {
            _comboBoxOils.Clear();

            foreach (EssentialOil oil in _context.EssentialOils)
            {
                _comboBoxOils.Add(oil);
            }
        }

        public bool CanSave()
        {
            return true;
        }

        public void SaveExecute()
        {
            SaveContextChanges();
        }

        public ICommand Save
        {
            get { return new RelayCommand(SaveExecute, CanSave); }
        }

        private void AddExecute()
        {
            Combo combo1 = new Combo();
            Combo combo2 = new Combo();
            try
            {
                combo1.EssentialOil1 = _selectedOil.Oil;
                combo1.EssentialOilId1 = _selectedOil.Id;
                combo1.EssentialOil2 = _comboBoxOil.Oil;
                combo1.EssentialOilId2 = _comboBoxOil.Id;

                combo2.EssentialOil1 = _comboBoxOil.Oil;
                combo2.EssentialOilId1 = _comboBoxOil.Id;
                combo2.EssentialOil2 = _selectedOil.Oil;
                combo2.EssentialOilId2 = _selectedOil.Id;
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
            catch (Exception ex)
            {
                //Already exists, remove comboes from local
                _context.Comboes.Local.Remove(combo1);
                _context.Comboes.Local.Remove(combo2);
                _context.SaveChanges();
            }
 
                
            if(ComboEditVM.ComboChangeEvent != null){
                ComboEditVM.ComboChangeEvent();
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
    }
}
