using System;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

namespace AdoSqlDataReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlConnection conn = new SqlConnection(@"Server=.\SECONDINSTANCE;Database=SoftUni;User Id=temporary; Password=Temporary10;Encrypt=yes;TrustServerCertificate=True;");

            conn.Open();

            using (conn)
            {
                //SqlCommand cmd = new SqlCommand("SELECT * FROM Employees", conn);
                //SqlCommand cmd = new SqlCommand("SELECT TOP 5 * FROM Employees", conn);
                //SqlCommand cmd = new SqlCommand("SELECT TOP 5 * FROM Employees WHERE Salary > 30000", conn);
                // подреждане на по дълга заявка ...

                SqlCommand cmd = new SqlCommand(
                     @"SELECT TOP (1000) [EmployeeID]
                      ,[FirstName]
                      ,[LastName]
                      ,[MiddleName]
                      ,[JobTitle]
                      ,[DepartmentID]
                      ,[ManagerID]
                      ,[HireDate]
                      ,[Salary]
                      ,[AddressID]
                  FROM [SoftUni].[dbo].[Employees]", conn);
                SqlDataReader reader = cmd.ExecuteReader();

                using (reader)
                {
                    while (reader.Read())
                    {
                        // Рийдъра има индексатор, той е като дикшънърито, като ключа е името на колоната да речем от таблицат в базата данни, а стойността е текущия ред в кото се чете от базата. Задължително обаче трябва да знаем името на колоната, няма кой да ни спре да сбъркаме в таи ситуация, нама интелисенс ...
                        // Може да се използва илии индексатора :
                        //Console.WriteLine(reader.GetString(0));
                        //или имената на колоните
                        Console.WriteLine($"{reader["FirstName"]} {reader["LastName"]} - {reader["Salary"]}");


                    }
                    //или може и така
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            string firstName = (string)reader["FirstName"];
                            string lastName = (string)reader["LastName"];
                            decimal salary = (decimal)reader["Salary"];
                            Console.WriteLine("{0} {1} - {2}", firstName, lastName, salary);
                        }


                    }
                }
            }
        }
    }
}