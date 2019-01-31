using System;

namespace FireMarket.Model
{
    public class Price : ITable
    {
        public int ID { get; set; }
        public int IDProduct { get; set; }
        public int Cost { get; set; }
        public DateTime Data { get; set; }

        public string[] GetProperties() => new string[] {
            ID.ToString(),
            IDProduct.ToString(),
            Cost.ToString(),
            Data.ToShortDateString()
        };

        public void SetProperties(params string[] arg)
        {
            ID = Convert.ToInt32(arg[0]);
            IDProduct = Convert.ToInt32(arg[1]);
            Cost = Convert.ToInt32(arg[2]);
            Data = Convert.ToDateTime(arg[3]);
        }

        public Price()
        {
            ID = default(int);
            IDProduct = default(int);
            Cost = default(int);
            Data = default(DateTime);
        }
    }
}
