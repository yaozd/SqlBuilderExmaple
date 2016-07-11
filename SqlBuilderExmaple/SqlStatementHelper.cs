using System.Linq;
using System.Text.RegularExpressions;
using FastMember;

namespace SqlBuilderExmaple
{
    /// <summary>
    /// SQL语句生成--用于Dapper或ibaits.net等ORM框架
    /// 目前仅处理一般的SQL语句转为实际执行的SQL语名
    /// 如果需要对复杂的语句请特殊处理
    /// --项目就要是用于程序调试
    /// </summary>
    public class SqlStatementHelper
    {
        private static readonly Regex SqlParmaRegex = new Regex(@"@[\w_]+", RegexOptions.Compiled);

        public static string  SqlBuilder<T>(string sqlTemplate, T param)
        {
            var sql = sqlTemplate;
            MatchCollection mc = SqlParmaRegex.Matches(sqlTemplate);
            foreach (Match ma in mc)
            {
                var name = ma.Value;
                var value = WithFastMember(param, name);
                if (value == null) continue;
                sql = sql.Replace(name, value);
            }
            return sql;
        }
        private static string WithFastMember(object obj, string name)
        {
            name = name.TrimStart('@');
            var accessor = TypeAccessor.Create(obj.GetType());
            var hasProp = accessor
                .GetMembers()
                .Any(m => m.Name == name);
            if (!hasProp) return null;
            var val = accessor[obj, name];
            var result = ConvertResult(val);
            return result;
        }

        private static string ConvertResult(object result)
        {
            if (result == null)
                return "null";
            var typeVal = result.GetType().Name.ToLower();
            if (typeVal == "sbyte"
                || typeVal == "int16"
                || typeVal == "int32"
                || typeVal == "int64"
                || typeVal == "byte"
                || typeVal == "uint16"
                || typeVal == "uint32"
                || typeVal == "uint64"
                || typeVal == "char"
                || typeVal == "single"
                || typeVal == "double"
                || typeVal == "decimal")
                return result.ToString();
            return "'" + result.ToString().Replace("'", "''") + "'";
        }
    }
}