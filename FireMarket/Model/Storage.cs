using System;

namespace FireMarket.Model
{
    public class Storage : ITable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Manager { get; set; }
        public int Fit { get; set; }

        public string[] GetProperties() => new string[] {
            ID.ToString(),
            Name,
            Phone,
            Address,
            Manager,
            Fit.ToString()
        };

        public void SetProperties(params string[] arg)
        {
            ID = Convert.ToInt32(arg[0]);
            Name = arg[1];
            Phone = arg[2];
            Address = arg[3];
            Manager = arg[4];
            Fit = Convert.ToInt32(arg[5]);
        }

        public Storage()
        {
            ID = default(int);
            Fit = default(int);
            Name = default(string);
            Phone = default(string);
            Address = default(string);
            Manager = default(string);
        }
    }
}
