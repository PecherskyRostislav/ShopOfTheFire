using System;

namespace FireMarket.Model
{
    public class Client : ITable
    {
        public int ID { get; set; }
        public string FullName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public int TotalPrice { get; set; }
        public int TotalAmount { get; set; }

        public string[] GetProperties() => new string[] {
            ID.ToString(),
            FullName,
            Phone,
            Address,
            Note,
            TotalPrice.ToString(),
            TotalAmount.ToString()
        };

        public void SetProperties(params string[] arg)
        {
            ID = Convert.ToInt32(arg[0]);
            FullName = arg[1];
            Phone = arg[2];
            Address = arg[3];
            Note = arg[4];
            TotalPrice = Convert.ToInt32(arg[5]);
            TotalAmount = Convert.ToInt32(arg[6]);
        }

        public Client()
        {
            ID = default(int);
            TotalPrice = default(int);
            TotalAmount = default(int);
            FullName = default(string);
            Phone = default(string);
            Address = default(string);
            Note = default(string);
        }
    }
}
