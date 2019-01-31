using System;

namespace FireMarket.Model
{
    public class Product : ITable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string View { get; set; }
        public string Pressure { get; set; }
        public string Temperature { get; set; }
        public string ExtingushClass { get; set; }
        public string AreaCovered { get; set; }
        public DateTime Suitability { get; set; }

        public int Price { get; set; }
        public int Count { get; set; }

        public string[] GetProperties() => new string[] {
            ID.ToString(),
            Name,
            Suitability.ToShortDateString(),
            View,
            Pressure,
            ExtingushClass,
            Temperature,
            AreaCovered,
            Price.ToString(),
            Count.ToString()
        };

        public void SetProperties(params string[] arg)
        {
            ID = Convert.ToInt32(arg[0]);
            Name = arg[1];
            Suitability = Convert.ToDateTime(arg[2]);
            View = arg[3];
            Pressure = arg[4];
            ExtingushClass = arg[5];
            Temperature = arg[6];
            AreaCovered = arg[7];
            Price = Convert.ToInt32(arg[8]);
            Count = Convert.ToInt32(arg[9]);
        }

        public Product()
        {
            ID = default(int);
            Count = default(int);
            Price = default(int);
            Name = default(string);
            Suitability = default(DateTime);
            View = default(string);
            Pressure = default(string);
            ExtingushClass = default(string);
            Temperature = default(string);
            AreaCovered = default(string);
        }
    }
}
