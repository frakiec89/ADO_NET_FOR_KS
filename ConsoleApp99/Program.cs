using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace ConsoleApp99
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = "Server=192.168.59.36;" +
                "Database=master; user id=stud; password=stud;" ;

            // Создание подключения
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {

                // Открываем подключение
                connection.Open();
                Info(connection);

                while (true)
                {
                    Console.WriteLine($"Список команд:");
                    Console.WriteLine($"Создать  базу данных: \"create\"");
                    Console.WriteLine($"Удалить  базу данных: \"dell\"");
                    Console.WriteLine($"Получить список баз данных: \"all\"");
                    Console.WriteLine($"Выход: \"exit\"");
                    Console.WriteLine($"Введите команду:");
                    switch (Console.ReadLine())
                    {
                        case "create":
                            Console.WriteLine("Создание новой базы данных:");
                            Console.WriteLine("Введите имя базы данных:");
                            CreatDataBase(connection, Console.ReadLine());
                            break;

                        case "dell":
                            Console.WriteLine("Удаление баз данных:");
                            Console.WriteLine("Введите имя базы данных которую надо удалить:");
                            DeleteDB(connection, Console.ReadLine());
                            break;


                        case "all":
                            Console.WriteLine("Список баз данных на сервере:");
                            var dbs = GetDatabasesHere(connection);
                            if(dbs.Count == 0) 
                            {
                                Console.WriteLine("На сервере нет баз данных:");
                            }

                            for (int i = 0; i < dbs.Count; i++)
                            {
                                Console.WriteLine ( $"{i+1}. {dbs[i]}");
                            }
                            break;
                        case "exit": return;
                        default: Console.WriteLine("команда не опознана"); break;
                    }
                }
            }


            catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }


            finally
            {
                // если подключение открыто
                if (connection.State == ConnectionState.Open)
                {
                    // закрываем подключение
                    connection.Close();
                    Console.WriteLine("Подключение закрыто...");
                }
                Console.WriteLine("Программа завершила работу.");
                Console.Read();
            }
        }

        private static void DeleteDB(SqlConnection connection, string name)
        {
            connection.Close();
            connection.Open();
            SqlCommand command = new SqlCommand();
            command.CommandText = $"DROP DATABASE {name}";
            command.Connection = connection;
            command.ExecuteNonQuery();
            Console.WriteLine("База данных Удалена");
          
        }

        private static void Info(SqlConnection connection)
        {
            Console.WriteLine("Подключение открыто");
            Console.WriteLine($"\tСтрока подключения: {connection.ConnectionString}");
            Console.WriteLine($"\tБаза данных: {connection.Database}");
            Console.WriteLine($"\tСервер: {connection.DataSource}");
            Console.WriteLine($"\tВерсия сервера: {connection.ServerVersion}");
            Console.WriteLine($"\tСостояние: {connection.State}");
            Console.WriteLine($"\tWorkstationld: {connection.WorkstationId}");
        }

        private static void CreatDataBase(SqlConnection connection , string name)
        {
            SqlCommand command = new SqlCommand();
            command.CommandText = $"CREATE DATABASE {name}";
            command.Connection = connection;
            command.ExecuteNonQuery();
            Console.WriteLine("База данных создана");
        }


        private static List<string> GetDatabasesHere (SqlConnection connection)
        {

            List<string> databases = new List<string>(); 
            SqlCommand command = new SqlCommand();
            command.CommandText = $"SELECT name FROM sys.databases Order by [name]";
            command.Connection = connection;
            var res =  command.ExecuteReader();
          
            if (res.HasRows) // если есть данные
            {
                // выводим названия столбцa
                while ( res.Read()) // построчно считываем данные
                {
                    string name = res.GetString(0);
                    databases.Add(name);    
                }
            }
            databases.Sort();


            return databases;   

        }
    }

    
}
