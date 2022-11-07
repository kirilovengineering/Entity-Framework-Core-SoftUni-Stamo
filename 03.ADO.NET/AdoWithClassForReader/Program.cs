using System;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;


namespace AdoWithClassForReader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Employee> Employees = new List<Employee>(); //създаваме лист от тип Employee
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
                        // добавяме към листа всеки ред оот рийдъра, поспочваме(кастваме какъв тип е стойността - типизираме за да може да се присвои правилно стойността)
                        Employees.Add(new Employee()
                        {
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            Salary = (decimal)reader["Salary"]
                        });
                    }
                }
            }
            //След като са затворени всички юзинги извън канекшън-а можем да изведем резултата, като работим с типизирания обект
            foreach (var employee in Employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.Salary}");
            }
        }
    }
}