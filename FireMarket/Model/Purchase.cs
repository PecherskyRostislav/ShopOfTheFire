using System;

namespace FireMarket.Model
{
    public class Purchase : ITable
    {
        public int ID { get; set; }
        public int IDProvider { get; set; }
        public int IDProduct { get; set; }
        public int Count { get; set; }
        public int Cost { get; set; }
        public DateTime DataPurchase { get; set; }

        public string[] GetProperties() => new string[] {
            ID.ToString(),
            IDProvider.ToString(),
            IDProduct.ToString(),
            Count.ToString(),
            DataPurchase.ToShortDateString(),
            Cost.ToString()
        };

        public void SetProperties(params string[] arg)
        {
            ID = Convert.ToInt32(arg[0]);
            IDProvider = Convert.ToInt32(arg[1]);
            IDProduct = Convert.ToInt32(arg[2]);
            Count = Convert.ToInt32(arg[3]);
            DataPurchase = Convert.ToDateTime(arg[4]);
            Cost = Convert.ToInt32(arg[5]);
        }

        public Purchase()
        {
            ID = default(int);
            IDProvider = default(int);
            IDProduct = default(int);
            Cost = default(int);
            Count = default(int);
            DataPurchase = default(DateTime);
        }
    }
}
