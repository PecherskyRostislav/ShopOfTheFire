using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireMarket.Model;

namespace FireMarket.ViewModel
{
    public class ClientsVM : BasePageVM<Client>
    {
        protected override void AddItem()
        {
            DBActions.Add<Client>(new Client
            {
                FullName = "Поставщик клиент " + ListCurrentData.Count
            });

            ListCurrentData = DBActions.Get<Client>();
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

            DBActions.Update<Client>(TempItem);
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

            DBActions.Delete<Client>(SelectedDataItem);
            ListCurrentData.Remove(SelectedDataItem);
            OnPropertyChanged("ListCurrentData");
            SelectedDataItem = ListCurrentData[ListCurrentData.Count - 1];
        }

        public ClientsVM() : base() { }
    }
}
