namespace FireMarket.Model
{
    public interface ITable
    {
        int ID { get; set; }

        void SetProperties(params string[] arg);
        string[] GetProperties();
    }
}
