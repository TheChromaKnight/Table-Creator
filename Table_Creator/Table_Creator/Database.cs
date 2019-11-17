using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.SqlServer.Management.SmoMetadataProvider;
using System.Data;


namespace Table_Creator
{
    public class Database
    {
        //Every database operation is here
        SqlConnection sqlconn;
        SqlCommand command;
        SqlDataReader reader;
        DataTable dTable;

        String connectionString;

        //Property
        public String ConnectionString
        {
            get
            {
                return connectionString;
            }
            set
            {
                connectionString = value;
            }
        }

        //Constructor
        public Database(String connString)
        {
            connectionString = connString;
        }


        //Creating a new table, starting the table's name with the counterStart variable.
        //The counterStart variable holds the value of the number of files in the directory
        //We create a new table from the (Files.count + 1)
        public void createNewTable(int counterStart)
        {
            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                using (command = new SqlCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText = "CREATE TABLE Furniture"+counterStart+ " (Furniture_Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,Furniture_Name varchar(30) NOT NULL) ";

                   
                    command.ExecuteNonQuery();
                }
            }
        }

        //Create default table if there aren't any (Default table name: Furniture1, column1: Furniture_Id, column2: Furniture_Name) 
        public void createNewTable()
        {
            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                using (command = new SqlCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText = "CREATE TABLE Furniture1 (Furniture_Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,Furniture_Name varchar(30) NOT NULL) ";

                    command.ExecuteNonQuery();
                }
            }
        }

        //Inserts the furniture into a table with a for loop (howManyTimes)
        public void insertIntoDatabase(String tableName, String furnitureName, int howManyTimes)
        {
            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();
                using (command = new SqlCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText = "INSERT INTO "+tableName+" (Furniture_Name) VALUES(@furnitureName)";
                    command.Parameters.AddWithValue("@furnitureName", furnitureName);

                    for (int i = 0; i < howManyTimes; i++)
                    {
                        command.ExecuteNonQuery();
                    }

                }
            }
        }

        //Insert once and without for loop
        public void insertIntoDatabase(String tableName, String furnitureName)
        {
            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();
                using (command = new SqlCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText = "INSERT INTO " + tableName + " (Furniture_Name) VALUES(@furnitureName)";
                    command.Parameters.AddWithValue("@furnitureName", furnitureName);

                    command.ExecuteNonQuery();

                }
            }
        }

        //Gets all the unique table names from the database (Unique table names are: Furniture1,2,3,4,5,6 and so on).
        public List<String> getAllTableNames()
        {
            List<String> tableList = new List<string>();
            String tableName;

            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                dTable = sqlconn.GetSchema("Tables");

                foreach (DataRow row in dTable.Rows)
                {
                    tableName = row[2].ToString();
                    tableList.Add(tableName);
                }
            }

            return tableList;

        }


        //This function will return a list from all the tables in the database with the count of rows
        //(might change list for a dictionary)
        public List<int> getAllTablesRowCount(List<String> tableList)
        {
            List<int> rowCountList = new List<int>();

            using (sqlconn = new SqlConnection(connectionString))
            {
                
                sqlconn.Open();

                using (command = new SqlCommand())
                {
                    for (int i = 0; i < tableList.Count; i++)
                    {
                        command.Connection = sqlconn;
                        command.CommandText = "SELECT COUNT(Furniture_Id) AS id FROM " + tableList[i] + " ";

                        using (reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                rowCountList.Add(Convert.ToInt16(reader["id"]));
                            }
                        }
                    }
                }
            }

            return rowCountList;
        }

    }
}
