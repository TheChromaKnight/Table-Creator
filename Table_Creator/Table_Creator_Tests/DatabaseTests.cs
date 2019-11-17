using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Table_Creator;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace Table_Creator_Tests
{
    [TestClass]
    public class DatabaseTests
    {
        List<String> resultList;
        List<int> numberResultList;

        Database db;
        String connectionString = "Data Source=" + Environment.MachineName + ";Initial Catalog=Table_Creator_Test;Integrated Security=True";
        SqlConnection sqlconn;
        SqlCommand command;
        SqlDataReader reader;
        DataTable dTable;
        


        [TestInitialize]
        public void init()
        {
            db = new Database(connectionString);

            dTable = new DataTable();
            resultList = new List<string>();
            numberResultList = new List<int>();
        }



        [TestMethod]
        public void createNewTable_thereAreNoTablesInDb_createsTableFurniture1()
        {
            //Arrange
            bool tableHasBeenCreated = false;
            

            //Act
            db.createNewTable();

            //Checks whether the table has been created or not
            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                dTable = sqlconn.GetSchema("Tables");

                foreach(DataRow row in dTable.Rows)
                {
                    if(row[2].ToString() == "Furniture1")
                    {
                        tableHasBeenCreated = true;
                    }
                }
            }

            //Assert
            Assert.AreEqual(true, tableHasBeenCreated);
                
        }

        [TestMethod]
        public void createNewTable_counterStartIs5_createsTableFurniture5()
        {
            //Arrange
            bool tableHasBeenCreated = false;
            int counterStart = 5;

            //Act
            db.createNewTable(counterStart);

            //Checks whether the table has been created or not
            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                dTable = sqlconn.GetSchema("Tables");

                foreach (DataRow row in dTable.Rows)
                {
                    if (row[2].ToString() == "Furniture5")
                    {
                        tableHasBeenCreated = true;
                    }
                }
            }

            //Assert
            Assert.AreEqual(true, tableHasBeenCreated);

        }
        
        [TestMethod]
        public void insertIntoDatabase_tableNameIsFurniture1_furnitureNameIsDesk_insertsDeskIntoFurniture1()
        {
            //Arrange
            bool insertIsSuccessful = false;
            String tableName = "Furniture1";
            String furnitureName = "Desk";


            //Act

            //Creating new table as the database is empty
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

            //Inserting data
            db.insertIntoDatabase(tableName, furnitureName);

            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                using (command = new SqlCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText = "SELECT Furniture_Name AS name FROM Furniture1 WHERE Furniture_Name = '" + furnitureName + "' ";

                    using (reader = command.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            resultList.Add(Convert.ToString(reader["name"]));
                        }
                    }
                }
            }

            if(resultList.Count == 1 && resultList[0] == furnitureName)
            {
                insertIsSuccessful = true;
            }

            //Assert
            Assert.AreEqual(true, insertIsSuccessful);

        }

        [TestMethod]
        public void insertIntoDatabase_tableNameIsFurniture1_furnitureNameIsDesk_howManyTimesIs50_insertsDesk50TimesIntoFurniture1()
        {
            //Arrange
            bool insertIsSuccessful = false;
            String tableName = "Furniture1";
            String furnitureName = "Desk";
            int howManyTimes = 50;


            //Act

            //Creating new table as the database is empty
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

            //Inserting data
            db.insertIntoDatabase(tableName, furnitureName, howManyTimes);

            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                using (command = new SqlCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText = "SELECT Furniture_Name AS name FROM Furniture1 WHERE Furniture_Name = '" + furnitureName + "' ";

                    using (reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultList.Add(Convert.ToString(reader["name"]));
                        }
                    }
                }
            }

            bool listOnlyContainsDesk = true;

            foreach(String element in resultList)
            {
                if(element != furnitureName)
                {
                    listOnlyContainsDesk = false;
                }
            }

            if (resultList.Count == howManyTimes && listOnlyContainsDesk == true)
            {
                insertIsSuccessful = true;
            }

            //Assert
            Assert.AreEqual(true, insertIsSuccessful);

        }

        [TestMethod]
        public void getAllTableNames_thereIsOnlyOneUniqueTableInTheDatabase_returnsListWith1Element()
        {
            //Arrange
            bool listContainsProperElements = false;
            String tableName = "Furniture1";

            //Act
            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                using (command = new SqlCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText = "CREATE TABLE "+tableName+" (Furniture_Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,Furniture_Name varchar(30) NOT NULL) ";

                    command.ExecuteNonQuery();
                }
            }

            resultList = db.getAllTableNames();

            if(resultList.Count == 1 && resultList[0] == tableName)
            {
                listContainsProperElements = true;
            }

            //Assert
            Assert.AreEqual(true, listContainsProperElements);

        }

        [TestMethod]
        public void getAllTableNames_thereAre20UniqueTablesInTheDatabase_returnsListWith20Elements()
        {
            //Arrange
            String tableName = "Furniture";
            int counter = 1;
            int correctElementsOfList = 0;
            int uniqueTables = 20;

            //Act

            //Creating as many tables as the uniqueTables variable
            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                using (command = new SqlCommand())
                {
                    command.Connection = sqlconn;
                    for(int i = 1; i <= uniqueTables; i++)
                    {
                        //appending the number to the table name
                        tableName += i.ToString();

                        command.CommandText = "CREATE TABLE "+tableName+" (Furniture_Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,Furniture_Name varchar(30) NOT NULL) ";

                        command.ExecuteNonQuery();

                        //removing the numbers from the string
                        //the tables's names always contain FURNITURE,
                        //which means that the remove property can start from the 9th index every time
                        //as after this, there are only numbers
                        tableName = tableName.Remove(9);
                        
                    }
                   
                }
            }

            resultList = db.getAllTableNames();

            
            foreach(String table in resultList)
            {
                if(table == "Furniture" + counter)
                {
                    correctElementsOfList++;

                }
                counter++;

            }

            //Assert
            Assert.AreEqual(correctElementsOfList, uniqueTables);

        }

        [TestMethod]
        public void getAllTablesRowCount_thereisOnlyOneUniqueTableInTheDatabaseWith1Element_returnsListWith1Element()
        {
            //Arrange
            List<String> tablesList = new List<string>();
            
            String tableName = "Furniture1";
            String furnitureName = "Desk";

            //Act

            //Creating new table
            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                using (command = new SqlCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText = "CREATE TABLE " + tableName + " (Furniture_Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,Furniture_Name varchar(30) NOT NULL) ";

                    command.ExecuteNonQuery();
                }
            }

            //Inserting data
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

            //getting the table's name we've just created
            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                dTable = sqlconn.GetSchema("Tables");

                foreach (DataRow row in dTable.Rows)
                {
                    tableName = row[2].ToString();
                    tablesList.Add(tableName);
                }
            }

            numberResultList = db.getAllTablesRowCount(tablesList);


            //Assert
            Assert.AreEqual(1, numberResultList[0]);
        }

        [TestMethod]
        public void getAllTablesRowCount_thereAre20UniqueTablesInTheDatabaseWith30Elements_returnsListWith20Elements()
        {
            //Arrange
            List<String> tablesList = new List<string>();

            String tableName = "Furniture";
            String furnitureName = "Desk";
            int uniqueTables = 20;
            int howManyTimes = 30;

            int correctElements = 0;
            bool listContainsCorrectElements = false;
            

            //Act
            for (int i = 1; i <= uniqueTables; i++)
            {
                //appending the number to the table name
                tableName += i.ToString();

                //Creating new table with the tableName variable
                using (sqlconn = new SqlConnection(connectionString))
                {
                    sqlconn.Open();

                    using (command = new SqlCommand())
                    {
                        command.Connection = sqlconn;

                        command.CommandText = "CREATE TABLE " + tableName + " (Furniture_Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,Furniture_Name varchar(30) NOT NULL) ";

                        command.ExecuteNonQuery();

                    }
                }


                //Inserting data
                using (sqlconn = new SqlConnection(connectionString))
                {
                    sqlconn.Open();
                    using (command = new SqlCommand())
                    {
                        command.Connection = sqlconn;
                        command.CommandText = "INSERT INTO " + tableName + " (Furniture_Name) VALUES(@furnitureName)";
                        command.Parameters.AddWithValue("@furnitureName", furnitureName);

                        for(int j = 0; j < howManyTimes; j++)
                        {
                            command.ExecuteNonQuery();
                        }
                       
                    }
                }

                //removing the numbers from the string
                //the tables's names always contain FURNITURE,
                //which means that the remove property can start from the 9th index every time
                //as after this, there are only numbers
                tableName = tableName.Remove(9);
            }

            //getting the table's name we've just created
            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                dTable = sqlconn.GetSchema("Tables");

                foreach (DataRow row in dTable.Rows)
                {
                    tableName = row[2].ToString();
                    tablesList.Add(tableName);
                }
            }

            numberResultList = db.getAllTablesRowCount(tablesList);

            foreach(int element in numberResultList)
            {
                if(element == 30)
                {
                    correctElements++;
                }
            }

            if (correctElements == uniqueTables)
            {
                listContainsCorrectElements = true;
            }

                //Assert
                Assert.AreEqual(true, listContainsCorrectElements);


        }

        [TestCleanup]
        public void cleanup()
        {
            resultList.Clear();

            //Getting all the table names
            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                using (command = new SqlCommand())
                {
                    command.Connection = sqlconn;
                    command.CommandText = "Select TABLE_NAME AS 'Table' FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE '";

                    using (reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            resultList.Add(reader["Table"].ToString());
                        }
                    }

                } 
               
            }

            //Dropping all the tables in the database
            using (sqlconn = new SqlConnection(connectionString))
            {
                sqlconn.Open();

                using (command = new SqlCommand())
                {
                    command.Connection = sqlconn;

                    for(int i = 0; i<resultList.Count; i++)
                    {
                        command.CommandText = "DROP TABLE " + resultList[i] + ";";

                        command.ExecuteNonQuery();
                    }

                }


            }

            //Disposing of objects
            resultList.Clear();
            numberResultList.Clear();
        }

        [ClassCleanup]
        public static void classCleanup()
        {
            DatabaseTests dbt = new DatabaseTests();

            dbt.db = null;
        }
    }
}
