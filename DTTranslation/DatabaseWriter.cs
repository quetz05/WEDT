using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEDT.DBTranslation
{
    class DatabaseWriter
    {
        private SQLiteConnection m_dbConnection;

        public DatabaseWriter(String filePath)
        {
            SQLiteConnection.CreateFile(filePath);
            String connectionString = String.Format("Data Source={0};Version=3;", filePath);
            m_dbConnection = new SQLiteConnection(connectionString);
            m_dbConnection.Open();
            string sql = "CREATE TABLE links (start TEXT, end TEXT)";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }

        public void saveLink(String from, String to)
        {
            m_dbConnection.Open();
            string sql = String.Format("insert into links (start, end) values ('{0}', '{1}')", from, to);
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();
            m_dbConnection.Close();
        }
    }
}
