using System;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;


namespace AdoSqlCommand
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SqlConnection conn = new SqlConnection(@"Server=.\SECONDINSTANCE;Database=SoftUni;User Id=temporary; Password=Temporary10;Encrypt=yes;TrustServerCertificate=True;");

            conn.Open();

            //DB connections are IDisposable objects
            // Always use the using construct in C#!

            using (conn)
            {
                SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Employees", conn);
                var result = cmd.ExecuteScalar();
                // ExecuteScalar() винаги връща единична стойност, която е (as System.Object) нетипизирана стойнст,каквито се очаква да са в C# 

                // може да се кастне към int предполага се, че ще е int, 
                var employeesCount = (int)cmd.ExecuteScalar();
                Console.WriteLine(result);
                Console.WriteLine("Employees count: {0} ", employeesCount);

            };
        }
    }
}