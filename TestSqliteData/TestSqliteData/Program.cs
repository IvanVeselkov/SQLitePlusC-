using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
namespace TestSqliteData
{
    class Program
    {
        private SQLiteCommand m_sqlCmd;

        public void LoadData(string ConnectionName)
        {
            m_dbConn = new SQLiteConnection();
            m_sqlCmd = new SQLiteCommand();
        }

        public void CreateData()
        {
            

            if (!File.Exists(@"D:\TestDB.db")) // если базы данных нету, то...
            {
                SQLiteConnection.CreateFile(@"D:\TestDB.db"); // создать базу данных, по указанному пути содаётся пустой файл базы данных
            }

            try
            {
                using (SQLiteConnection m_dbConn = new SQLiteConnection(@"Data Source=D:\TestDB.db; Version=3;")) // в строке указывается к какой базе подключаемся
                {
                    m_dbConn.Open();
                    m_sqlCmd = new SQLiteCommand();
                    m_sqlCmd.Connection = m_dbConn;

                    m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Authorization (id INTEGER PRIMARY KEY AUTOINCREMENT, login TEXT, password TEXT)";
                    m_sqlCmd.ExecuteNonQuery();
                    m_dbConn.Close();
                    Console.WriteLine("Connected");
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Disconnected");

                Console.WriteLine("Error: " + ex.Message);
            }
        }
        public void ReadToData()
        {
            DataTable dTable = new DataTable();
            String sqlQuery;
            Dictionary<string, string> data = new Dictionary<string, string>();


            try
            {
                using (SQLiteConnection m_dbConn = new SQLiteConnection(@"Data Source=D:\TestDB.db; Version=3;")) // в строке указывается к какой базе подключаемся
                {
                    m_dbConn.Open();
                    sqlQuery = "SELECT * FROM Authorization";
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
                    adapter.Fill(dTable);

                    if (dTable.Rows.Count > 0)
                    {
                        data.Clear();

                        for (int i = 0; i < dTable.Rows.Count; i++)
                        {
                            Console.WriteLine(dTable.Rows[i].ItemArray[0].ToString() + "  "+dTable.Rows[i].ItemArray[1].ToString() + "  " + dTable.Rows[i].ItemArray[2].ToString());
                            //data.Add(dTable.Rows[i].ItemArray[1].ToString(), dTable.Rows[i].ItemArray[2].ToString());
                        }
                    }
                    else
                        Console.WriteLine("Database is empty");
                    m_dbConn.Close();
                    Console.WriteLine("Done");
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            foreach (var i in data)
            {
                Console.WriteLine(i.Key + "  " + i.Value);
            }
        }

        public void AddToData(string lg = null, string pw = null)
        {
            string login;
            string password;
            if (lg == null)
                login = "admin";
            else
                login = lg;

            if (pw == null)
                password = "ksusa so pretty";
            else
                password = pw;


            try
            {
                using (SQLiteConnection m_dbConn = new SQLiteConnection(@"Data Source=D:\TestDB.db; Version=3;")) // в строке указывается к какой базе подключаемся
                {
                    m_dbConn.Open();
                    Console.WriteLine("Connected");
                    m_sqlCmd = new SQLiteCommand();
                    m_sqlCmd.Connection = m_dbConn;

                    m_sqlCmd.CommandText = "INSERT INTO Authorization ('login', 'password') values ('" +
                     login + "' , '" +
                    password + "')";
                    m_sqlCmd.ExecuteNonQuery();
                    m_dbConn.Close();
                }
            }
            catch (SQLiteException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }


        static void Main(string[] args)
        {
            #region first try
            Program test = new Program();

            int check;
            do
            {
                Console.WriteLine("Введи значение переключателя");
                check = int.Parse(Console.ReadLine());

                switch (check)
                {
                    case 1:
                        Console.WriteLine("Создание базы данных");
                        test.CreateData();
                        break;
                    case 2:
                        Console.WriteLine("Подключение к базе данных");
                        test.ConnectToData();
                        break;
                    case 3:
                        Console.WriteLine("Чтение из базы данных");
                        test.ReadToData();
                        break;
                    case 4:
                        Console.WriteLine("Добавление записи к базе данных");
                        test.AddToData();
                        break;

                    default:
                        break;
                }
            } while (check != 0);
        }

        private void ConnectToData()
        {
            throw new NotImplementedException();
        }
        #endregion


    }
}
