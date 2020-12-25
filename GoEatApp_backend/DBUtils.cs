using MySql.Data.MySqlClient;

namespace GoEatapp_backend
{
    public class DBUtils
    {
        public static MySqlConnection GetDBConnection()
        {
            return new MySqlConnection("server=localhost;database=goeatapp;port=3306;user id=root;password=root;CHARSET=utf8;");
        }

        public static string SafeGetString(MySqlDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }
    }
}
