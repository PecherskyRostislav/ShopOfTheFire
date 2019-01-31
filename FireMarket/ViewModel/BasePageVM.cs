using System.Collections.ObjectModel;
using System.Windows.Input;
using FireMarket.Model;

namespace FireMarket.ViewModel
{
    public abstract class BasePageVM<T> : BaseViewModel where T: ITable, new()
    {
        public ObservableCollection<ITable> ListCurrentData { get; set; }
        private ITable _selectedDataItem;
        public ITable SelectedDataItem
        {
            get => _selectedDataItem;
            set
            {
                _selectedDataItem = value;
                TempItem = new T();
                TempItem.SetProperties(value.GetProperties());
                OnPropertyChanged("TempItem");

                OnPropertyChanged("SelectedDataItem");
            }
        }

        public ITable TempItem { get; set; }

        public ICommand Add
        {
            get => new RelayCommand((obj) => AddItem());
        }
        public ICommand Edit
        {
            get => new RelayCommand((obj) => EditItem());
        }
        public ICommand Remove
        {
            get => new RelayCommand((obj) => RemoveItem());
        }


        bool CanSave
        {
            get
            {
                return true;
            }
        }

        protected abstract void AddItem();
        protected abstract void EditItem();
        protected abstract void RemoveItem();


        public BasePageVM()
        {
            ListCurrentData = new ObservableCollection<ITable>();
            ListCurrentData = DBActions.Get<T>();
            _selectedDataItem = new T();
            TempItem = new T();
            if (ListCurrentData.Count > 0)
            {
                _selectedDataItem = ListCurrentData[0];
                TempItem.SetProperties(_selectedDataItem.GetProperties());
            }
        }
    }
}
