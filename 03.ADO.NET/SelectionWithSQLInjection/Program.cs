using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectionWithSQLInjection
{
    using System.Data;
    using System.Data.SqlClient;
    internal class Program
    {
        static void Main(string[] args)
        {
            string connectionString = @"Server=.\SECONDINSTANCE;Database=SoftUni;User Id=temporary; Password=Temporary10;Encrypt=yes;TrustServerCertificate=True;";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (connection)
            {
                Selecting("Guy", connection);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("First query passed");
                Console.ForegroundColor = ConsoleColor.White;
                // Демонстрация на инджекшън, дъмпва цялата таблица с посочения стринг
                Selecting("' OR 1=1 --", connection);
                // 1.Първия апостроф затваря вътешно зададения в заявката --> FirstName = '{name}' 
                //    заявката се получва:
                //  $"SELECT * FROM Employees WHERE FirstName = '' OR 1=1 --" ....
                // т.е. празен стринг или уловието 1=1 което винаги е изпълнено ...
                // зявката дав всички записи от таблицата
                // 2.Допълва заявката с условието OR 1=1 , което винаги е изпълнено и
                // 3.Вкарва -- за коментар на всичкоостаноло след тиретата за да не гръмне заявката
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Second query passed");
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static void Selecting(string name, SqlConnection connection)
        {
            string selectionCommandString = $"SELECT * FROM Employees WHERE FirstName = '{name}'";
            SqlCommand command = new SqlCommand(selectionCommandString, connection);
            SqlDataReader reader = command.ExecuteReader();
            using (reader)
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write($"{reader[i]} ");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}