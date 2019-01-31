using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireMarket.Model;

namespace FireMarket.ViewModel
{
    public class ProvidersVM : BasePageVM<Provider>
    {
        protected override void AddItem()
        {
            DBActions.Add<Provider>(new Provider
            {
                FullName = "Поставщик новый " + ListCurrentData.Count
            });

            ListCurrentData = DBActions.Get<Provider>();
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

            DBActions.Update<Provider>(TempItem);
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

            DBActions.Delete<Provider>(SelectedDataItem);
            ListCurrentData.Remove(SelectedDataItem);
            OnPropertyChanged("ListCurrentData");
            SelectedDataItem = ListCurrentData[ListCurrentData.Count - 1];
        }

        public ProvidersVM() : base() { }
    }
}
