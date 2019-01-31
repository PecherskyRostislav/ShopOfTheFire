using System;

namespace FireMarket.Model
{
    public class Quantity : ITable
    {
        public int ID { get; set; }
        public int IDStorage { get; set; }
        public int IDProduct { get; set; }
        public int Count { get; set; }

        public string[] GetProperties() => new string[] {
            ID.ToString(),
            IDStorage.ToString(),
            IDProduct.ToString(),
            Count.ToString()
        };

        public void SetProperties(params string[] arg)
        {
            ID = Convert.ToInt32(arg[0]);
            IDStorage = Convert.ToInt32(arg[1]);
            IDProduct = Convert.ToInt32(arg[2]);
            Count = Convert.ToInt32(arg[3]);
        }

        public Quantity()
        {
            ID = default(int);
            IDStorage = default(int);
            IDProduct = default(int);
            Count = default(int);
        }
    }
}
