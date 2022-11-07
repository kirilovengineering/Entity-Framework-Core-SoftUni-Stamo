using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace AdoWithClassForReaderAsyncWithParametrizedQuery
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            List<Employee> Employees = new List<Employee>();
            SqlConnection conn = new SqlConnection(@"Server=.\SECONDINSTANCE;Database=SoftUni;User Id=temporary; Password=Temporary10;Encrypt=yes;TrustServerCertificate=True;");

            await conn.OpenAsync();

            using (conn)
            {
                // с валиден вход извежда коректен запис със хората които са с такова първо име
                //string name = "Guy";
                //SqlCommand cmd = new SqlCommand(
                //     @"SELECT TOP (1000) [EmployeeID]
                //      ,[FirstName]
                //      ,[LastName]
                //      ,[MiddleName]
                //      ,[JobTitle]
                //      ,[DepartmentID]
                //      ,[ManagerID]
                //      ,[HireDate]
                //      ,[Salary]
                //      ,[AddressID]
                //  FROM [SoftUni].[dbo].[Employees] WHERE FirstName = '" + name + "'", conn);

                // ако на name подадем  ' or 1=1 -- , дъмпваме цялата база
                //string name = "' or 1=1 --";
                //SqlCommand cmd = new SqlCommand(
                //     @"SELECT TOP (1000) [EmployeeID]
                //      ,[FirstName]
                //      ,[LastName]
                //      ,[MiddleName]
                //      ,[JobTitle]
                //      ,[DepartmentID]
                //      ,[ManagerID]
                //      ,[HireDate]
                //      ,[Salary]
                //      ,[AddressID]
                //  FROM [SoftUni].[dbo].[Employees] WHERE FirstName = '" + name + "'", conn);

                // малко отклонение и пример за "хаване", тези проблеми се решават с параметризирана заявка - parameterized queries

                // със следния код в частност стринга ( ' INSERT INTO Users VALUES('hacker','') -- ), който ще се въведе ще бъде въведен нов поребител hacker с парола празен стринг, -- ще коментира вътрешно остатъка от заявката

                //SELECT COUNT(*) FROM Users WHERE UserName = '' INSERT INTO Users VALUES('hacker','') --' AND PasswordHash = 'XOwXWxZePV5iyeE86Ejvb + rIG / 8 = '

                // тези проблеми се решават с параметризирана заявка - parameterized queries
                string name = "Guy";
                // ако пробваме по долния код с string name = "' or 1=1 --"; този път няма да сработи "хака", т.е. инжекцията не сработва, простата конкатенация не сработва
                // string name = "' or 1=1 --";
                SqlCommand cmd = new SqlCommand(
                     @"SELECT * FROM Employees WHERE FirstName = @firstname_parametar", conn);

                cmd.Parameters.AddWithValue("firstname_parametar", name);
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                using (cmd)
                {
                    using (reader)
                    {

                        while (await reader.ReadAsync())
                        {

                            Employees.Add(new Employee()
                            {
                                FirstName = (string)reader["FirstName"],
                                LastName = (string)reader["LastName"],
                                Salary = (decimal)reader["Salary"]
                            });
                        }
                    }
                }
            }

            foreach (var employee in Employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.Salary}");
            }
        }
    }
}
// ПОЛЗАВАЙТЕ ПАРАМЕТРИЗИРАНИ ЗАЯВКИ
// Конкретно този проблем в Entity Framework е решен, тъй като завките се генерират автоматично и там са параметризирани ..въпреки всичко има други видове ORM инжекции ..входовете винаги трябва да се изчиствавт предварително