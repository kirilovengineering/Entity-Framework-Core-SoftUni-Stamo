using System;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;


namespace AdoWithClassForReaderAsync
{
    internal class Program
    {
        // Необходимо е да добавим using System.Threading.Tasks;
        // На практика тези неща трябва да се случват асинхронно. Това гарантира ненатоварването на сървъра и при положение, че има змного заявки. За да стане асинхронно трябва да се използват асинхронни методи там където може. В стандартния
        // static void Main(string[] args)
        // Няма как да се случи и се налагат няколко промени - void се смена с Task пред Task трябва да стане async. This async method lacks(липсва - подчертавка в жълто във Вижуъл Студио) 'await' operators ... може да се прочете в балона ако посочим Main метода без да сме използвали 'await' оператор преди вътрешните методи. Добавяме там където е необходимо await и ползваме async методи.
        static async Task Main(string[] args)
        {
            List<Employee> Employees = new List<Employee>(); //създаваме лист от тип Employee
            SqlConnection conn = new SqlConnection(@"Server=.\SECONDINSTANCE;Database=SoftUni;User Id=temporary; Password=Temporary10;Encrypt=yes;TrustServerCertificate=True;");

            // conn.Open(); след като се добави async метод за конекция подчертавката на Main метода изчезва ... хелп подробност, полезно.
            // Отново ... важно е да се работи с асинхронниметоди когато се работи с външни стуктури, каквито са баите данни наппример.
            await conn.OpenAsync();

            using (conn)
            {
                //SqlCommand cmd = new SqlCommand("SELECT * FROM Employees", conn);
                //SqlCommand cmd = new SqlCommand("SELECT TOP 5 * FROM Employees", conn);
                //SqlCommand cmd = new SqlCommand("SELECT TOP 5 * FROM Employees WHERE Salary > 30000", conn);
                // Подреждане на по дълга заявка ...

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
                //SqlDataReader reader = cmd.ExecuteReader();
                // Тук също имаме асинхронен метод.
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                using (reader)
                {
                    //while (reader.Read())
                    // Тук също иамаме async възможност, въпреки че тук няма обръщане към базата директно, а чрез рийдъра.
                    while (await reader.ReadAsync())
                    {
                        // Добавяме към листа всеки ред от рийдъра, поспочваме(кастваме какъв тип е стойността - типизираме за да може да се присвои правилно стойността).
                        Employees.Add(new Employee()
                        {
                            FirstName = (string)reader["FirstName"],
                            LastName = (string)reader["LastName"],
                            Salary = (decimal)reader["Salary"]
                        });
                    }
                }
            }
            // След като са затворени всички юзинги извън канекшън-а можем да изведем резултата, като работим с типизирания обект.
            foreach (var employee in Employees)
            {
                Console.WriteLine($"{employee.FirstName} {employee.LastName} - {employee.Salary}");
            }
        }
    }
}
// Всичко това освобождава ресурсите на съръра при работа на много приложения към базата.
// ВИНАГИ КОГАТО ИМАМЕ ВЪЗМОЖНОСТ ТРЯБВА ДА РАБОТИМ АСИНХРОННО!
// Използваме async await патърна + async Task за Main метода.
// По конвеция всички асинхронни методи завършват на ... Async.