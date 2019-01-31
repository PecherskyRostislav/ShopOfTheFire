using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data.OleDb;

namespace FireMarket.Model
{
    public static class DBActions
    {
        private static OleDbConnection connection;
        private static OleDbCommand command;
        static DBActions()
        {
            string DBname = "Database.accdb";
            if (!System.IO.File.Exists(DBname))
            {
                throw new Exception($"File {DBname} not found.");
            }
            connection = new OleDbConnection
            {
                ConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={DBname}"
            };
            command = new OleDbCommand
            {
                Connection = connection
            };
        }

        public static ObservableCollection<ITable> Get<T>() where T : ITable, new()
        {
            string tableName = GetTableName<T>();

            ObservableCollection<ITable> result = new ObservableCollection<ITable>();
            connection.Open();
            command.CommandText = GetQuery<T>();

            OleDbDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                ITable table = new T();
                string[] mass = new string[dataReader.FieldCount];
                for (int i = 0; i < dataReader.FieldCount; i++)
                    mass[i] = dataReader[i].ToString();

                table.SetProperties(mass);
                result.Add(table);
            }
            dataReader.Close();
            connection.Close();
            return result;
        }

        public static void Delete<T>(ITable Item) where T : ITable, new()
        {
            connection.Open();
            command.CommandText = $"DELETE FROM [{GetTableName<T>()}] WHERE [ID] = {Item.ID}";
            command.ExecuteScalar();
            connection.Close();
        }

        public static void Update<T>(ITable Item) where T : ITable, new()
        {
            connection.Open();
            command.CommandText = GetUpdateQuery<T>(Item.GetProperties());
            command.ExecuteScalar();
            connection.Close();
        }

        public static void Add<T>(ITable Item) where T : ITable, new()
        {
            connection.Open();
            command.CommandText = GetAddQuery<T>(Item.GetProperties());
            command.ExecuteScalar();
            
            if (typeof(T).IsAssignableFrom(typeof(Product)))
            {                
                command.CommandText = "SELECT @@IDENTITY";               
                int lastId = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                DBActions.Add<Price>(new Price
                {
                    Cost = 0,
                    Data = DateTime.Now,
                    IDProduct = lastId
                });
                DBActions.Add<Quantity>(new Quantity
                {
                    Count = 0,
                    IDStorage = 1,
                    IDProduct = lastId
                });                
            }
            if (typeof(T).IsAssignableFrom(typeof(Provider)))
            {
                command.CommandText = "SELECT @@IDENTITY";
                int lastId = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                connection.Open();
                int idProduct = 0;
                command.CommandText = "SELECT First(Product.id) FROM Product";
                OleDbDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    idProduct = Convert.ToInt32(dataReader[0]);
                }
                dataReader.Close();
                connection.Close();

                DBActions.Add<Purchase>(new Purchase
                {
                    IDProvider = lastId,
                    Count = 0,
                    Cost = 0,
                    DataPurchase = DateTime.Now,
                    IDProduct = idProduct
                });                
            }
            if (typeof(T).IsAssignableFrom(typeof(Client)))
            {
                command.CommandText = "SELECT @@IDENTITY";
                int lastId = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
                connection.Open();
                int idProduct = 0;
                command.CommandText = "SELECT First(Product.id) FROM Product";
                OleDbDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    idProduct = Convert.ToInt32(dataReader[0]);
                }
                dataReader.Close();
                connection.Close();

                DBActions.Add<Realize>(new Realize
                {
                    IDClient = lastId,
                    Count = 0,
                    Cost = 0,
                    DataSale = DateTime.Now,
                    IDProduct = idProduct
                });
            }
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();
        }        

        private static string GetQuery<T>() where T : ITable, new()
        {
            string query = "";
            if (typeof(T).IsAssignableFrom(typeof(Client)))
            {
                query = $"SELECT Client.*, " +
                    "(SELECT Sum(Realize.Cost) FROM Realize WHERE Realize.idClient = Client.id GROUP BY Realize.idClient), " +
                    "(SELECT Sum(Realize.Count) FROM Realize WHERE Realize.idClient = Client.id GROUP BY Realize.idClient) " +
                    "FROM Client";
            }
            if (typeof(T).IsAssignableFrom(typeof(Price)))
            {
                query = $"SELECT * FROM [Price]";
            }
            if (typeof(T).IsAssignableFrom(typeof(Product)))
            {
                query = $"SELECT Product.*, " +
                    $"(SELECT Cost FROM Price WHERE idProduct = Product.ID AND data = (SELECT Last(data) FROM Price WHERE idProduct = Product.ID)) AS Price, " +
                    $"(SELECT Sum(Quantity.Count) FROM Quantity WHERE idProduct = Product.ID GROUP BY Quantity.idProduct) AS [Count] " +
                    $"FROM Product";
            }
            if (typeof(T).IsAssignableFrom(typeof(Provider)))
            {
                query = $"SELECT Provider.*, " +
                    "(SELECT Sum(Purchase.Cost) FROM Purchase WHERE Purchase.idProvider = Provider.id GROUP BY Purchase.idProvider), " +
                    "(SELECT Sum(Purchase.Count) FROM Purchase WHERE Purchase.idProvider = Provider.id GROUP BY Purchase.idProvider) " +
                    "FROM Provider;";
            }
            if (typeof(T).IsAssignableFrom(typeof(Quantity)))
            {
                query = $"SELECT * FROM [Quantity]";
            }
            if (typeof(T).IsAssignableFrom(typeof(Realize)))
            {
                query = $"SELECT * FROM [Realize]";
            }
            if (typeof(T).IsAssignableFrom(typeof(Storage)))
            {
                query = $"SELECT * FROM [Storage]";
            }

            return query;
        }

        private static string GetAddQuery<T>(params string[] arg) where T : ITable, new()
        {
            string query = "";
            if (typeof(T).IsAssignableFrom(typeof(Client)))
            {
                query = String.Format(
                                "INSERT INTO [Client] ([fullName], [phone], [address], [note]) values('{0}', '{1}', '{2}', '{3}')",
                                arg[1],
                                arg[2],
                                arg[3],
                                arg[4]
                               );
            }
            if (typeof(T).IsAssignableFrom(typeof(Price)))
            {
                query = String.Format(
                                "INSERT INTO [Price] ([idProduct], [Cost], [data]) values({0}, {1}, '{2}')",
                                arg[1],
                                arg[2],
                                arg[3]
                               );
            }
            if (typeof(T).IsAssignableFrom(typeof(Product)))
            {
                query = String.Format(
                                 "INSERT INTO [Product] ([Name], [suitability], [view], [Pressure], [Temperature], [extingushClass], [areaCovered]) " +
                                 "values('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')",
                                 arg[1],
                                 arg[2],
                                 arg[3],
                                 arg[4],
                                 arg[5],
                                 arg[6],
                                 arg[7]
                                );
            }
            if (typeof(T).IsAssignableFrom(typeof(Provider)))
            {
                query = String.Format(
                                 "INSERT INTO [Provider] ([fullName], [address], [phone], [note]) " +
                                 "values('{0}', '{1}', '{2}', '{3}')",
                                 arg[1],
                                 arg[2],
                                 arg[3],
                                 arg[4],
                                 arg[5]
                                );
            }
            if (typeof(T).IsAssignableFrom(typeof(Quantity)))
            {
                query = String.Format(
                                 "INSERT INTO [Quantity] ([idStorage], [idProduct], [Count]) values({0}, {1}, {2})",
                                 arg[1],
                                 arg[2],
                                 arg[3]
                                );
            }
            if (typeof(T).IsAssignableFrom(typeof(Realize)))
            {
                query = String.Format(
                                 "INSERT INTO [Realize] ([idClient], [idProduct], [Count], [DataSale], [Cost]) values({0}, {1}, {2}, '{3}', {4})",
                                 arg[1],
                                 arg[2],
                                 arg[3],
                                 arg[4],
                                 arg[5]
                                );
            }
            if (typeof(T).IsAssignableFrom(typeof(Purchase)))
            {
                query = String.Format(
                                 "INSERT INTO [Purchase] ([idProvider], [idProduct], [Count], [Data], [Cost]) values({0}, {1}, {2}, '{3}', {4})",
                                 arg[1],
                                 arg[2],
                                 arg[3],
                                 arg[4],
                                 arg[5]
                                );
            }
            if (typeof(T).IsAssignableFrom(typeof(Storage)))
            {
                query = String.Format(
                                 "INSERT INTO [Storage] ([nameStorage], [phone], [address], [nameManager], [fit]) " +
                                 "values('{0}', '{1}', '{2}', '{3}', {4})",
                                 arg[1],
                                 arg[2],
                                 arg[3],
                                 arg[4],
                                 arg[5]
                                );
            }

            return query;
        }

        private static string GetUpdateQuery<T>(params string[] arg) where T : ITable, new()
        {
            string query = "";
            if (typeof(T).IsAssignableFrom(typeof(Client)))
            {
                query = String.Format(
                                "UPDATE [Client] SET [fullName] = '{1}', [phone] = '{2}', [address] = '{3}', [note] = '{4}' WHERE [ID] = {0}",
                                arg[0],
                                arg[1],
                                arg[2],
                                arg[3],
                                arg[4]
                               );
            }
            if (typeof(T).IsAssignableFrom(typeof(Price)))
            {
                query = String.Format(
                                "UPDATE [Price] SET [idProduct] = {1}, [Cost] = {2}, [data] = '{3}' WHERE [ID] = {0}",
                                arg[0],
                                arg[1],
                                arg[2],
                                arg[3]
                               );
            }
            if (typeof(T).IsAssignableFrom(typeof(Product)))
            {
                query = String.Format(
                                 "UPDATE [Product] SET [Name] = '{1}', [suitability] = '{2}', [view] = '{3}', [Pressure] = '{4}', [Temperature] = '{5}', " +
                                 "[extingushClass] = '{6}', [areaCovered] = '{7}' WHERE [ID] = {0}",
                                 arg[0],
                                 arg[1],
                                 arg[2],
                                 arg[3],
                                 arg[4],
                                 arg[5],
                                 arg[6],
                                 arg[7]
                                );
            }
            if (typeof(T).IsAssignableFrom(typeof(Provider)))
            {
                query = String.Format(
                                 "UPDATE [Provider] SET [fullName] = '{1}', [address] = '{2}', " +
                                 "[phone] = '{3}', [note] = '{4}' WHERE [ID] = {0}",
                                 arg[0],
                                 arg[1],
                                 arg[2],
                                 arg[3],
                                 arg[4],
                                 arg[5]
                                );
            }
            if (typeof(T).IsAssignableFrom(typeof(Quantity)))
            {
                query = String.Format(
                                 "UPDATE [Quantity] SET [idStorage] = {1}, [idProduct] = {2}, [Count] = {3} WHERE [ID] = {0}",
                                 arg[0],
                                 arg[1],
                                 arg[2],
                                 arg[3]
                                );
            }
            if (typeof(T).IsAssignableFrom(typeof(Realize)))
            {
                query = String.Format(
                                 "UPDATE [Realize] SET [idClient] = {1}, [idProduct] = {2}, [Count] = {3}," +
                                 "[DataSale] = '{4}', [Cost] = {5} WHERE [ID] = {0}",
                                 arg[0],
                                 arg[1],
                                 arg[2],
                                 arg[3],
                                 arg[4],
                                 arg[5]
                                );
            }
            if (typeof(T).IsAssignableFrom(typeof(Purchase)))
            {
                query = String.Format(
                                 "UPDATE [Purchase] SET [idProvider] = {1}, [idProduct] = {2}, [Count] = {3}," +
                                 "[Data] = '{4}', [Cost] = {5} WHERE [ID] = {0}",
                                 arg[0],
                                 arg[1],
                                 arg[2],
                                 arg[3],
                                 arg[4],
                                 arg[5]
                                );
            }

            if (typeof(T).IsAssignableFrom(typeof(Storage)))
            {
                query = String.Format(
                                 "UPDATE [Storage] SET [nameStorage] = '{1}', [phone] = '{2}', [address] = '{3}', " +
                                 "[nameManager] = '{4}', [fit] = {5} WHERE [ID] = {0}",
                                 arg[0],
                                 arg[1],
                                 arg[2],
                                 arg[3],
                                 arg[4],
                                 arg[5]
                                );
            }
            return query;
        }

        private static string GetTableName<T>() where T : ITable, new()
        {
            string tableName = "";
            if (typeof(T).IsAssignableFrom(typeof(Client)))
            {
                tableName = "Client";
            }
            if (typeof(T).IsAssignableFrom(typeof(Price)))
            {
                tableName = "Price";
            }
            if (typeof(T).IsAssignableFrom(typeof(Product)))
            {
                tableName = "Product";
            }
            if (typeof(T).IsAssignableFrom(typeof(Provider)))
            {
                tableName = "Provider";
            }
            if (typeof(T).IsAssignableFrom(typeof(Quantity)))
            {
                tableName = "Quantity";
            }
            if (typeof(T).IsAssignableFrom(typeof(Realize)))
            {
                tableName = "Realize";
            }
            if (typeof(T).IsAssignableFrom(typeof(Storage)))
            {
                tableName = "Storage";
            }
            return tableName;
        }
    }
}
