using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlBuilderExmaple
{
    class Program
    {
        static void Main(string[] args)
        {
            var person = new Person
            {
                address = "xx,x-\"xxx",
                registerDate = DateTime.Now,
                id = 5
            };
            var strSelect = Select(person);
            Display(strSelect);
            var strInsert = Insert(person);
            Display(strInsert);
            var strUpdate = Update(person);
            Display(strUpdate);
            Pause();
        }

        static string Select(Person model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 id,username,password,age,registerDate,address from Person ");
            strSql.Append(" where id=@id");
            return SqlStatementHelper.SqlBuilder(strSql.ToString(), model);

        }
        static string Insert(Person model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Person(");
            strSql.Append("username,password,age,registerDate,address)");
            strSql.Append(" values (");
            strSql.Append("@username,@password,@age,@registerDate,@address)");
            return SqlStatementHelper.SqlBuilder(strSql.ToString(), model);
        }
        static string Update(Person model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Person set ");
            strSql.Append("username=@username,");
            strSql.Append("password=@password,");
            strSql.Append("age=@age,");
            strSql.Append("registerDate=@registerDate,");
            strSql.Append("address=@address");
            strSql.Append(" where id=@id");
            return SqlStatementHelper.SqlBuilder(strSql.ToString(), model);
        }

        static void Display(string obj)
        {
            Console.WriteLine(obj);
        }
        private static void Pause()
        {
            Console.ReadLine();
        }
    }
}
