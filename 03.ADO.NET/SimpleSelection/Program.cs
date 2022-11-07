﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSelection
{
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
                string selectionCommandString = "SELECT * FROM Employees";
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
}