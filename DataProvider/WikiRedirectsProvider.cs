using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEDT.DataProvider
{
    public class WikiRedirectsProvider
    {
        private SQLiteConnection m_dbConnection;

        public WikiRedirectsProvider()
        {
            String filePath = "dbRedirects.sqlite";
            //SQLiteConnection.CreateFile(filePath);
            String connectionString = String.Format("Data Source={0};Version=3;", filePath);
            m_dbConnection = new SQLiteConnection(connectionString);
        }

        public String redirect(String from)
        {
            String result = null;
            m_dbConnection.Open();
            using (SQLiteCommand fmd = m_dbConnection.CreateCommand())
            {
                fmd.CommandText = 
                    String.Format("SELECT end FROM links where start LIKE '{0}';", from);
                SQLiteDataReader r = fmd.ExecuteReader();
                if (r.Read())
                {
                    result = Convert.ToString(r["end"]);
                }
            }
            m_dbConnection.Close();
            return result;
        }
    }
}
