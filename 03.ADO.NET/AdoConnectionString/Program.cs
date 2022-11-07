using System;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;

// !!!Важно конекшън стринга може да се види от SQL Server Object Explorer -> Върху базата с десен бутон - Properites и се извежда при мен :    Data Source=LAPTOPKIRIL\SECONDINSTANCE;User ID=sa;Password=********;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False
//

namespace AdoConnectionString
{
    internal class Program
    {
        static void Main(string[] args)
        {

            // В стринга две еднакви наименования на едно и също нещо (Data Source и Server) както и (Initial Catalog е едно и също с Database). Парсера на стринга ще хване еднотипни работи.

            // without Windows authentication
            //Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;

            // with Windows authentication
            //Server=(local)\SQLEXPRESS;Initial Catalog=SoftUni;Integrated Security=true;

            // Integrated Security – false if credentials are provided

            //Необходимо е с Microsoft SQL Management Studio са се добави за сигурност друг user и passowrd и трябва да се разрешат няколко настройки от LAPTOPKIRIL\SECONDINSTANCE -> Security -> Logins -> съзззздаваме нов потребител, от Properites задаваме и паролата и задаваме Defaul Database - SoftUni , Server Roles - > чек на public и sysadmin, в User Mapping чек на SoftUni базата данни - МОЖЕ БИ ВЪВ ВРЪЗКА СЪС СИГУРНОСТТ ТРЯБВА ДА СЕ ОГРАНИЧИ ОТ НЯКЪДЕ ДОСТЪП ОТ ВЪН И ДА Е САМО ОТ localhost. Може да се ползва сикрет мениджър от типа https://www.vaultproject.io/

            // User ID / Password – credentials

            //@"Server=.\SECONDINSTANCE;Database=SoftUni;User Id=temporary; Password=Temporary10;Encrypt=yes;TrustServerCertificate=True;

            //SqlClient and ADO.NET Retreiving data in connecting model
            // 1.Oprn a connection (SqlConnection)
            // 2.Execute command (SqlCommand)
            // 3.Process the result set of the query by using the reader (SqlDataReader)
            // 4.Close the reader (за ADO.NET сами управляваме отваряне и затваряне на рийдарите и конекциите)
            // 5.Close the connection

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