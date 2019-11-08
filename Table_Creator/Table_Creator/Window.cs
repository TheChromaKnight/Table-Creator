using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Table_Creator
{
    class Window
    {

        static void Main(string[] args)
        {
            //Main controller of the application
            Database db = new Database();
            Functionalities fc = new Functionalities();

            fc.getTableNames();
            fc.createTableInfoFiles();
            fc.checkFilesForFreeSpaces();

            Console.Write("This application allows you to play with tables, which can have up to 500 elements, ");
            Console.Write("you'll have to give me an Integer X number and a furniture, and I'll insert the furniture X times into the table,");
            Console.Write("if it's not in there yet, ");
            Console.Write("if the table is full, the system will create a new table and insert the remaining date into the new table. ");
            Console.WriteLine(Messages.proceedMessage);

            Console.ReadKey();

            Char selectedOption;

            do
            {
                //getting the values for the furniture and the number of times, the system's going to upload
                fc.getFurnitureFromUser();
                fc.getNumberFromUser();

                //Writing the current value of the furniture and the number
                fc.writeCurrentFurniture();
                fc.writeCurrentNumber();

                //Choosing the next option (1 or 2) and retrieving the value
                fc.chooseNextOption();
                selectedOption = fc.getSelectedOption();

            }

            while (selectedOption != '1');

            if (selectedOption == '1')
            {
                fc.upload();
                Console.WriteLine("Done");
                Console.WriteLine("Thanks for checking out my app");
            }
           


            Console.ReadKey();
        }
    }
}
