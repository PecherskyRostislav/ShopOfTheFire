using System;

namespace FireMarket.Model
{
    public class Realize : ITable
    {
        public int ID { get; set; }
        public int IDClient { get; set; }
        public int IDProduct { get; set; }
        public int Count { get; set; }
        public int Cost { get; set; }
        public DateTime DataSale { get; set; }

        public string[] GetProperties() => new string[] {
            ID.ToString(),
            IDClient.ToString(),
            IDProduct.ToString(),
            Count.ToString(),
            DataSale.ToShortDateString(),
            Cost.ToString()
        };

        public void SetProperties(params string[] arg)
        {
            ID = Convert.ToInt32(arg[0]);
            IDClient = Convert.ToInt32(arg[1]);
            IDProduct = Convert.ToInt32(arg[2]);
            Count = Convert.ToInt32(arg[3]);
            DataSale = Convert.ToDateTime(arg[4]);
            Cost = Convert.ToInt32(arg[5]);
        }

        public Realize()
        {
            ID = default(int);
            IDClient = default(int);
            IDProduct = default(int);
            Cost = default(int);
            Count = default(int);
            DataSale = default(DateTime);
        }
    }
}
