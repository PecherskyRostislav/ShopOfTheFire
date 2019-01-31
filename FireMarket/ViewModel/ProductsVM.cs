using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FireMarket.Model;

namespace FireMarket.ViewModel
{
    public class ProductsVM : BasePageVM<Product>
    {
        protected override void AddItem()
        {
            DBActions.Add<Product>(new Product
            {
                Name = "Новый элемент" + ListCurrentData.Count                
            });

            ListCurrentData = DBActions.Get<Product>();
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

            DBActions.Update<Product>(TempItem);
            DBActions.Add<Price>(new Price
            {
                Cost = ((Product)TempItem).Price,
                IDProduct = ((Product)TempItem).ID,
                Data = DateTime.Now
            });
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

            DBActions.Delete<Product>(SelectedDataItem);
            ListCurrentData.Remove(SelectedDataItem);
            OnPropertyChanged("ListCurrentData");
            SelectedDataItem = ListCurrentData[ListCurrentData.Count - 1];
        }

        public ProductsVM() : base() {}
    }
}
