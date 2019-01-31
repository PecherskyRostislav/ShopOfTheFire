using FireMarket.Model;

namespace FireMarket.ViewModel
{
    public class StoragesVM : BasePageVM<Storage>
    {
        protected override void AddItem()
        {
            DBActions.Add<Storage>(new Storage
            {
                Name = "Новый элемент" + ListCurrentData.Count
            });
            ListCurrentData = DBActions.Get<Storage>();
            OnPropertyChanged("ListCurrentData");
            SelectedDataItem = ListCurrentData[ListCurrentData.Count - 1];
        }

        protected override void EditItem()
        {            
            System.Windows.MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(
                "Вы уверены, что хотите внести изменения?", 
                "Предупреждение", 
                System.Windows.MessageBoxButton.YesNo, 
                System.Windows.MessageBoxImage.Warning
            );
            if (messageBoxResult == System.Windows.MessageBoxResult.No)
                return;

            DBActions.Update<Storage>(TempItem);
            ListCurrentData[ListCurrentData.IndexOf(SelectedDataItem)] = TempItem;
        }

        protected override void RemoveItem()
        {
            System.Windows.MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show(
                "Вы уверены, что хотите удалить выбранный элемент?",
                "Предупреждение",
                System.Windows.MessageBoxButton.YesNo,
                System.Windows.MessageBoxImage.Warning
            );
            if (messageBoxResult == System.Windows.MessageBoxResult.No)
                return;

            DBActions.Delete<Storage>(SelectedDataItem);
            ListCurrentData.Remove(SelectedDataItem);
            OnPropertyChanged("ListCurrentData");
            SelectedDataItem = ListCurrentData[ListCurrentData.Count - 1];
        }

        public StoragesVM() : base() { }
    }
}
