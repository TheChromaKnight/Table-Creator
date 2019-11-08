using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Table_Creator
{
    class Functionalities
    {
        //Every function that is function related is here

        String yourFurniture;
        int numberOfTimes;
        Char selectedOption;
        Database db;

        List<String> tableNamesList;
        List<int> tableRowCountList;
        List<String> emptyTablesList;

        //Dictionary used, to store the free spaces's count in each table
        Dictionary<String, int> freeSpaceDictionary;

        //Path to the directory, where the tableinfo files are
        String fileDirectoryPath = "tables/";
        int counterStart;

        //Variable to check whether the tables are full
        bool tablesAreFull;

        //Empty spaces in the table (500-x)
        int emptySpace;
        //Name of the table
        String tableName;

        //Constructor
        public Functionalities()
        {
            getNumberOfFilesInDirectory();

            //Create them here, these are used at least 2 times
            emptyTablesList = new List<string>();
            tableNamesList = new List<string>();
        }

        //getter
        public void writeCurrentFurniture()
        {
            Console.WriteLine("Your furniture: {0}", yourFurniture);
        }

        //getter
        public void writeCurrentNumber()
        {
            Console.WriteLine("Your number: {0}", numberOfTimes);
        }

        //getter
        public Char getSelectedOption()
        {
            return selectedOption;
        }

        //setter
        public void setValueOfYourFurniture(String furniture)
        {
            yourFurniture = furniture;
        }

        //setter
        public void setValueOfNumberOfTimes(int number)
        {
            numberOfTimes = number;
        }

        //setter
        public void setValueOfSelectedOption(Char option)
        {
            selectedOption = option;
        }

        /*This method writes all table names from our database in the console
        USE IT FOR DEBUGGING
        
        public void writeAllTableNames()
        {
            tableNames = new List<string>();

            db = new Database();

            tableNames = db.getAllTableNames();

            foreach(String table in tableNames)
            {
                Console.WriteLine(table);
            }
        }
        */

        //Gets the count of files in the tables directory, where we create the tableinfo files.
        public void getNumberOfFilesInDirectory()
        {
            counterStart = Directory.GetFiles(fileDirectoryPath, "*.*", SearchOption.TopDirectoryOnly).Length;
        }

        //This method gets all the unique table names from the database
        public void getTableNames()
        {
            db = new Database();

            tableNamesList = db.getAllTableNames();
        }


        //This method will check the files for free spaces in the tables
        public void checkFilesForFreeSpaces()
        {
            String path = ("tables/Furniture");
            freeSpaceDictionary = new Dictionary<string, int>();

            tablesAreFull = false;

            //DEBUG
            //Console.WriteLine(counterStart);

            for (int i = 1; i<= counterStart; i++)
            {
                //Check if file exists!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                String[] lines = File.ReadAllLines(path + i.ToString() + ".txt");


                emptySpace = 500 - Convert.ToInt16(lines[1]);
                tableName = lines[0];

                //Only add it to the Dictionary if it has any empty space
                if(emptySpace > 0 && emptySpace <=500)
                {
                    freeSpaceDictionary.Add(tableName, emptySpace);
                }

            }
            if(freeSpaceDictionary.Count == 0)
            {
                tablesAreFull = true;
            }
            
            /*USE IT FOR DEBUGGING
            foreach (String nums in freeSpaceDictionary.Keys)
            {
                Console.WriteLine(nums +"  " +  freeSpaceDictionary[nums]);
            }
            */
        }

        //The method which decides whether the system requires new tables or a single table is enough
        public void upload()
        {
            //Option 2: Every table is full (Create new tables)
            if (tablesAreFull == true)
            {
                //DEBUG
                //Console.WriteLine("OPTION 21");       

                //Ceil the value as we need a table for the remainder too
                double howmanyTimes = Math.Ceiling(Convert.ToDouble(numberOfTimes) / 500);

                String currentTable;
                db = new Database();

                //Create as many table as needed
                for (int i = 0; i < howmanyTimes; i++)
                {
                    counterStart++;
                    db.createNewTable(counterStart);
                }

                //Reset every file in the directory
                getTableNames();
                createTableInfoFiles();
                getNumberOfFilesInDirectory();
                checkFilesForFreeSpaces();

                //DEBUG
                //Console.WriteLine("OPTION 22");

                foreach (KeyValuePair<String, int> item in freeSpaceDictionary)
                {
                    emptyTablesList.Add(item.Key);
                    Console.WriteLine(item.Key);
                }
                //Console.WriteLine(tablesAreFull);
                //Console.ReadKey();
               
                for (int i = 0; i < howmanyTimes; i++)
                {
                    currentTable = emptyTablesList[i];

                    if(numberOfTimes <= 500)
                    {
                        db.insertIntoDatabase(currentTable, yourFurniture, numberOfTimes);
                    }
                    else if(numberOfTimes > 500)
                    {
                        db.insertIntoDatabase(currentTable, yourFurniture, 500);
                        numberOfTimes -= 500;
                    }

                   
                }

                //DEBUG
                //Console.WriteLine("OPTION 23");
            }
            //OPTION 3: The table does not have enough spaces to upload into a single table
            // (upload into the last table and THEN create new tables)
            else if(tablesAreFull == false)
            {
                //DEBUG
                //Console.WriteLine("OPTION 3");

                String currentTable = freeSpaceDictionary.Keys.Last();
                int currentFreeSpaces = freeSpaceDictionary.Values.Last();
                db = new Database();

                //If numberofTimes is more than the current free spaces in the table,
                //we upload it into the table and subtract the quantity, then create the new tables and upload to the new ones.
                if (numberOfTimes > currentFreeSpaces)
                {
                    db.insertIntoDatabase(currentTable, yourFurniture, currentFreeSpaces);
                    numberOfTimes -= currentFreeSpaces;

                    //DEBUG
                    //Console.WriteLine("option 34");
                }
                //If the numberOfTimes variable is less than the empty spaces in the table,
                //we just upload it.
                else if(numberOfTimes < currentFreeSpaces)
                {
                    db.insertIntoDatabase(currentTable, yourFurniture, numberOfTimes);

                    /*DEBUG
                    Console.WriteLine("option 35");
                    Console.WriteLine("numOfTImes: " + numberOfTimes);
                    */

                    numberOfTimes -= numberOfTimes;
                    
                    //DEBUG
                    //Console.WriteLine("numOfTImes: " + numberOfTimes);
                }
                    

                if(numberOfTimes != 0)
                {
                    //Ceil the value as we need a table for the remainder too
                    double howmanyTimes = Math.Ceiling(Convert.ToDouble(numberOfTimes) / 500);

                    //Create as many table as needed
                    for (int i = 0; i < howmanyTimes; i++)
                    {
                        counterStart++;
                        db.createNewTable(counterStart);
                    }

                    //Reset every file in the directory
                    getTableNames();
                    createTableInfoFiles();
                    getNumberOfFilesInDirectory();
                    checkFilesForFreeSpaces();

                    //DEBUG
                    //Console.WriteLine("OPTION 32");

                    foreach (KeyValuePair<String, int> item in freeSpaceDictionary)
                    {
                        emptyTablesList.Add(item.Key);
                    }

                    /*DEBUG
                    Console.WriteLine(tablesAreFull);
                    Console.ReadKey();
                    */

                    for (int i = 0; i < howmanyTimes; i++)
                    {
                        currentTable = emptyTablesList[i];

                        if (numberOfTimes <= 500)
                        {
                            db.insertIntoDatabase(currentTable, yourFurniture, numberOfTimes);
                        }
                        else if (numberOfTimes > 500)
                        {
                            db.insertIntoDatabase(currentTable, yourFurniture, 500);
                            numberOfTimes -= 500;
                        }


                    }

                    //DEBUG
                    //Console.WriteLine("OPTION 33");
                }

            }

        }



        //this method will create table info files. every file's first row contains the table name.
        //The the second row has the number of rows in the table
        //We create a single file for every table in the database
        public void createTableInfoFiles()
        {
           

            //If there are no tables in the database, we create one file with the default values (FILE NAME: Furniture1,
            //row 1:Furniture1, row2: 0)
            //
            if(tableNamesList.Count == 0)
            {
                String path = "tables/Furniture1.txt";

                File.WriteAllText(path, "Furniture1" + Environment.NewLine + "0");

                db.createNewTable();


            }
            else
            {
                String currentTableName;

                tableRowCountList = db.getAllTablesRowCount(tableNamesList);

                //Creates files, then writes the 2 rows into it
                for (int i = 0; i < tableNamesList.Count; i++)
                {
                    currentTableName = tableNamesList[i];

                    String path = "tables/" + currentTableName + ".txt";

                    File.WriteAllText(path, currentTableName + Environment.NewLine + tableRowCountList[i]);


                }
            }

           

        }


        //this method will get a furniture from the user
        public void getFurnitureFromUser()
        {
            //Clear console, because we can reuse the method
            Console.Clear();

            Console.WriteLine("What's the furniture that you'd like to upload?");
            Console.Write("Remember, the furniture cannot contain any number, and it must contain at least 2 characters, ");
            Console.WriteLine("also, it must be a single word. ");


            bool furnitureContainsNumber = true;

            //The system will repeat the get furniture message from the user, until all characters are letters, there are no numbers in it,
            //and it is a single word.
            do
            {
                Console.Write(Messages.writeHereMessage);
                yourFurniture = Console.ReadLine();

                if (yourFurniture.Length < 2 || yourFurniture.Any(char.IsDigit) == true)
                {
                    Console.WriteLine("The furniture must be at least 2 characters long and it should not contain any numbers.");
                    continue;
                }
                else if (yourFurniture.Length > 2 && yourFurniture.Trim().All(char.IsLetter) == true)
                {
                    furnitureContainsNumber = false;
                    Console.WriteLine(yourFurniture + " is correct");

                    Console.WriteLine(Messages.proceedMessage);
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("the furniture can not contain white spaces,symbols. ONLY letters are allowed");
                }

            }
            while (furnitureContainsNumber == true);
        }

        //this method will get a number from the user
        public void getNumberFromUser()
        {
            //Clear console, because we can reuse the method
            Console.Clear();

            //this is the variable, used until the user's number is really a number, and a correct one.
            String convertableTextToYourNumber;
            bool yourNumberIsNumber = false;

            Console.WriteLine("How many times would you like to upload the furniture?");

            do
            {
                Console.Write(Messages.writeHereMessage);
                convertableTextToYourNumber = Console.ReadLine();

                if (convertableTextToYourNumber.Any(char.IsLetter) == true || convertableTextToYourNumber.Length >= 5 || convertableTextToYourNumber == "0" || convertableTextToYourNumber.Contains('.') || convertableTextToYourNumber.Contains(',') || convertableTextToYourNumber == "")
                {
                    Console.WriteLine("The number cannot be negative nor 0, or be more than 10000, and it must be an integer");
                    continue;
                }
                else if (convertableTextToYourNumber.Trim().All(char.IsNumber) == true)
                {
                    numberOfTimes = Convert.ToInt16(convertableTextToYourNumber);
                    yourNumberIsNumber = true;

                    Console.WriteLine(numberOfTimes + " is correct");

                    Console.WriteLine(Messages.proceedMessage);
                    Console.ReadKey();
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("Exception");
                }
            }
            while (yourNumberIsNumber == false);
        }

        //Choosing the next option (does the user made a mistake and would like to correct it, or the user wants to upload)
        public void chooseNextOption()
        {

            Console.WriteLine("Would you like to upload with the current data?");

            Console.WriteLine("1.) Yes");
            Console.WriteLine("2.) No");



            while (selectedOption != '1' || selectedOption != '2')
            {
                selectedOption = Console.ReadKey().KeyChar;

                if (selectedOption == '1' || selectedOption == '2')
                {
                    break;
                }
                else
                    continue;
            }
        }
        
    }
}
