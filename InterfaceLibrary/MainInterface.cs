using System.ComponentModel.Design;
using System.Text;
using System.Text.Json;
using System.Xml;
using FileWorkingLibrary;

namespace InterfaceLibrary
{
    public class MainInterface
    {
        private PathProcessing _fPath;
        private PathProcessing? _nPath;
        private DataProcessing _data;
        private Movie[]? _movies;
        private int _num;

        /// <summary>
        /// This constructor reads data and checks its correctness.
        /// </summary>
        public MainInterface()
        {
            while (true)
            {
                SayHello();
                Console.WriteLine("Please choose one option to read data.");
                Console.WriteLine();

                // Printing menu to choose a way of saving file.
                Menu menu = new Menu("Use up/down keys to choose menu item.", new string[] { "1. Read data from a *.json file.", "2. Read data from the console.", "3. Enter every object manually."});

                // Variable of user's choice.
                _num = menu.ActMenu();
                try
                {
                    // If user chooses reading data from a file.
                    if (_num == 1)
                    {
                        PrintColor("Enter your file's absolute name.", ConsoleColor.Magenta, ConsoleColor.DarkCyan);

                        // Checking path.
                        _fPath = new PathProcessing(Console.ReadLine());
                        _data = new DataProcessing(_fPath.FPath);
                        PrintColor("You enter correct file name and data in it is also right.Congratulations!", ConsoleColor.Green, ConsoleColor.DarkGreen);
                    }
                    // If user chooses reading data from console.
                    else if (_num == 2)
                    {
                        PrintColor("Enter your data. To finish write \"exit\" at new line", ConsoleColor.Magenta, ConsoleColor.DarkCyan);
                        StringBuilder sb = new StringBuilder();
                        string? line;

                        // Reading data while !exit
                        while ((line = Console.ReadLine()) != null && line != "exit")
                        {
                            sb.Append(line);
                        }
                        _data = new DataProcessing(sb);
                        Console.WriteLine("You enter correct data.Congratulations!");
                    }
                    // If users chooses enterring every element manually.
                    else
                    {
                        _data = new DataProcessing(EnterData());
                    }
                    
                }
                catch (JsonException)
                {
                    PrintColor("Invalid json file.Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (NotSupportedException)
                {
                    PrintColor("Invalid objects in json. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (PathTooLongException)
                {
                    PrintColor("Your file name is too long. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    PrintColor("Wrong file path. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (UnauthorizedAccessException)
                {
                    PrintColor("This file can be only read. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (IOException)
                {
                    PrintColor("Error while writing data in the file. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (ArgumentNullException)
                {
                    PrintColor("Wrong file.Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (ArgumentException)
                {
                    PrintColor("Wrong file. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (NullReferenceException)
                {
                    PrintColor("Sorry, there is a problem with data. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                _movies = _data.Movies;
                break;
            }
        }

        /// <summary>
        /// This method implements interface of enterring every element of movie object.
        /// </summary>
        /// <returns></returns>
        private Movie[] EnterData()
        {
            List<Movie> movie = new List<Movie>();

            string[] names = new string[] { "movieId", "movieTitle", "genre", "actorsPercent", "earnings", "rating", "releaseYear"};
            string[] values = new string[names.Length];

            // Repeating cycle for adding several movies.
            while (true)
            {
                // Getting fields except nested array of actors.
                for (int i = 0; i < names.Length; i++)
                {
                    Console.Write($"Enter {names[i]}:");
                    string? input = Console.ReadLine();

                    // Checking correctness of input values according to its keys.
                    while (((i == 0 || i == 1 || i == 2) && string.IsNullOrEmpty(input)) || ((i == 3 || i == 4 || i == 5) && (!double.TryParse(input, out _) || double.Parse(input)<0)) || (i == 6 && (!int.TryParse(input, out _) || int.Parse(input)<=1870 || int.Parse(input) > DateTime.Now.Year)))
                    {
                        PrintColor("Wrong value. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                        Console.Write($"Enter {names[i]}:");
                        input = Console.ReadLine();
                    }
                    // Assigning input value.
                    values[i] = input;
                }
                Console.WriteLine("You are going to enter values for \"actors\" array");

                // Counting movies' objects to inform user about number of new object.
                int counter = 0;

                string[]? actorNames = new string[] { "actorId", "actorName", "nationality" };
                string[]? actorValues;

                // List of elements for actors array.
                List<Tuple<string, string, string, double>> tuple = new List<Tuple<string, string, string, double>>();
                
                // Repeating cycle for adding several actors.
                while (true)
                {
                    counter+= 1;
                    Console.WriteLine($"{counter} actor:");
                    
                    // Array of actor's fields.
                    actorValues = new string[actorNames.Length + 1];

                    // Getting actor's values from user.
                    for (int i = 0; i < actorNames.Length; i++)
                    {
                        Console.Write($"Enter {actorNames[i]}:");
                        string? input = Console.ReadLine();

                        // Checking correctness of input values.
                        while ((i == 0 || i == 1 || i == 2) && string.IsNullOrEmpty(input))
                        {
                            PrintColor("Wrong value. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                            Console.Write($"Enter {actorNames[i]}:");
                            input = Console.ReadLine();
                        }
                        actorValues[i] = input;
                    }
                    // Counting actors' earnings and writing it to array of actor's fields.
                    actorValues[^1] = ((double.Parse(values[3]) / 100 * double.Parse(values[4]))/counter).ToString();

                    PrintColor($"Actor's earnings are counted automatically. It is {actorValues[^1]} for every actor in this movie.", ConsoleColor.Yellow, ConsoleColor.DarkYellow);

                    // Adding all data from the array to tuple of actor's fields.
                    tuple.Add(new Tuple<string, string, string, double>(actorValues[0], actorValues[1], actorValues[2], double.Parse(actorValues[3])));

                    Menu menu = new Menu("Do you want to enter another actor?", new string[] { "Yes", "No" });
                    int numb = menu.ActMenu();

                    if (numb == 1)
                        continue;
                    break;
                }
                // Creating new movie object with data from user.
                Movie m = new Movie(values[0], values[1], values[2], double.Parse(values[3]), double.Parse(values[4]), double.Parse(values[5]), int.Parse(values[6]), tuple.ToArray<Tuple<string, string, string, double>>());
                movie.Add(m);

                Menu choice = new Menu("Do you want to enter another object?", new string[] { "Yes", "No" });
                int num = choice.ActMenu();
                if (num == 1)
                    continue;
                break;
            }
            return movie.ToArray<Movie>();
        }

        /// <summary>
        /// This methods implements menu interface.
        /// </summary>
        /// <param name="num"></param>
        public void ShowMenu(out int num)
        {
            // Repeating cycle to show menu again if the corresponding option has been chosen.
            while (true)
            {
                // Show interactive menu.
                Menu menu = new Menu("Please choose a way of processing file", new string[] { "1. Sort data field ascending", "2. Sort data field descending", "3. Change an object", "4. Save data", "5. Exit" });

            
                // Index of user's choice.
                num = menu.ActMenu();

                // Variable to indicate when we should exit data processing.
                bool flag = true;
                DataProcessingInterface process;

                // If user chooses sorting ascending.
                if (num == 1)
                {
                    process = new DataProcessingInterface(1);
                    _movies = process.SortInterface(_data);
                    break;
                }

                // If user chooses sorting descending.
                if (num == 2)
                {
                    process = new DataProcessingInterface(2);
                    _movies = process.SortInterface(_data);
                    break;
                }

                // If user chooses changing field.
                if (num == 3)
                {
                    process = new DataProcessingInterface(2);
                    _movies = process.ChangeInterface(_data, _data.Movies, ref flag);

                    // If user chooses returns to the menu.
                    if (!flag)
                        continue;
                    continue;
                }

                // If user chooses saving data.
                if(num == 4)
                {
                    // If it is the first menu revealing.
                    if(_movies is null && _num != 3)
                    {
                        Menu menuChange = new Menu("Data hasn't been changed yet. Are you sure you want to save it?", new string[] { "Yes", "No" });

                        // If user chooses to save unchanged data.
                        if (menuChange.ActMenu() == 1)
                            _movies = _data.Movies;
                        else
                            continue;
                    }
                    SaveData(false);
                    return;
                }
                // If user chooses exit.
                if (num == 5)
                    return;
            }
        }

        /// <summary>
        /// This method implements saving data to the file interface.
        /// </summary>
        public void SaveData(bool print = true)
        {
            // Asking about printing data if user doesn't choose saving in the menu.
            if (print)
            {
                // Print or not print.
                Menu printMenu = new Menu("Do you want to print your data?", new string[] { "Yes", "No, I just want to save it." });

                // User's choice.
                int choice = printMenu.ActMenu();

                // If user chooses printing data.
                if (choice == 1)
                {
                    PrintData();
                }
            }

            // Save or not save.
            Menu? saveMenu = null;

            // Asking about saving.
            if (print)
            {
                saveMenu = new Menu("Do you want to save your data?", new string[] { "Yes", "No" });
            }

            // If user chooses saving data and asking about saving is unnecessary.
            if (!print || (saveMenu != null && saveMenu.ActMenu() == 1))
            {
                // Initializing num with 2 when user entered data in console and there are no oportunity to save it in the initial file.
                int num = 2;

                //  If user printed data in console.
                if (_num == 2 || _num == 3)
                {
                    PrintColor("Your data is going to be saved in a new file because wou entered your data without an initial file.", ConsoleColor.Yellow, ConsoleColor.DarkYellow);
                }
                // If data had been read from the file.
                else
                {
                    saveMenu = new Menu("Your data is going to be saved. Please choose a way of saving", new string[] { "Save to the initial file instead of the original data", "Save in a new file" });
                    num = saveMenu.ActMenu();
                }
                // The cycle doesn't allow to continue until data will be saved correctly.
                while (true)
                {
                    try
                    {
                        // Giving 3 to the constructor because it is needed to save the whole data array.
                        PrintInterface printing = new PrintInterface(_movies, 3);

                        // If user chooses saving in the initial file.
                        if (num == 1)
                        {
                            PrintColor($"Your data will be saved in the initial file {_fPath.FPath}", ConsoleColor.Magenta, ConsoleColor.DarkCyan);
                            _data.WriteJson(_fPath.FPath, printing.PrintAsJson);
                        }
                        // If user chooses saving in a new file.
                        if (num == 2)
                        {
                            PrintColor("Enter file name with .json extention in the end to create the file and save data there.", ConsoleColor.Magenta, ConsoleColor.DarkCyan);
                            _nPath = new PathProcessing(Console.ReadLine());
                            _data.WriteJson(_nPath.NPath, printing.PrintAsJson);

                        }
                        Console.WriteLine();
                        PrintColor("Data has been successfully saved.", ConsoleColor.Green, ConsoleColor.DarkGreen);
                        break;
                    }
                    catch (PathTooLongException)
                    {
                        PrintColor("Your file name is too long. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                        continue;
                    }
                    catch (DirectoryNotFoundException)
                    {
                        PrintColor("Wrong file path. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                        continue;
                    }
                    catch (UnauthorizedAccessException)
                    {
                        PrintColor("This file can be only read. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                        continue;
                    }
                    catch (IOException)
                    {
                        PrintColor("Error while writing data in the file. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                        continue;
                    }
                    catch (ArgumentException)
                    {
                        PrintColor("Wrong file name.Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                        continue;
                    }
                    catch (NullReferenceException)
                    {
                        PrintColor("Wrong file name. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                    }
                }
            }
        }

        /// <summary>
        /// This method implements printing data in console.
        /// </summary>
        public void PrintData()
        {

            // Printing menu of choosing.
            Menu topBottom = new Menu("Do you want to see only first/last elemnts", new string[] { "First N elements", "Last N elements", "All elements" });
            int choice = topBottom.ActMenu();

            PrintInterface printData = new PrintInterface(_movies, choice);

            printData.PrintAsJson();
        }

        /// <summary>
        /// This method greets user according to date time.
        /// </summary>
        private void SayHello()
        {
            if (DateTime.Now.Hour < 12 && DateTime.Now.Hour >= 5)
                PrintColor("Good morning!", ConsoleColor.Yellow, ConsoleColor.Yellow);

            else if (DateTime.Now.Hour < 17 && DateTime.Now.Hour >= 12)
                PrintColor("Good afternoon!", ConsoleColor.Magenta, ConsoleColor.Magenta);

            else if (DateTime.Now.Hour < 22 && DateTime.Now.Hour >= 17)
                PrintColor("Good evening!", ConsoleColor.Blue, ConsoleColor.Blue);

            else if (DateTime.Now.Hour < 5 && DateTime.Now.Hour >= 1)
                PrintColor("Good Night! I am sorry you haven't slept yet.", ConsoleColor.Cyan, ConsoleColor.Cyan);

            else
                PrintColor("Good night!", ConsoleColor.DarkBlue, ConsoleColor.DarkBlue);
        }

        /// <summary>
        /// This method prints colourful text.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="color"></param>
        public static void PrintColor(string str, ConsoleColor colorDay, ConsoleColor colorNight)
        {
            // Day time colors.
            if (DateTime.Now.Hour <= 20 && DateTime.Now.Hour >= 7)
            {
                Console.ForegroundColor = colorDay;
                Console.WriteLine(str);
                Console.ResetColor();
            }
            // Night time colors.
            else
            {
                Console.ForegroundColor = colorNight;
                Console.WriteLine(str);
                Console.ResetColor();
            }
        }
    }
}