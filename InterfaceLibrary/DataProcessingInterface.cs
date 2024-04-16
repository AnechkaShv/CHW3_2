using FileWorkingLibrary;
using System.Text;
using System.Text.Json;

namespace InterfaceLibrary
{
    public class DataProcessingInterface
    {
        /// <summary>
        /// This enum containes movies' fields for changing.
        /// </summary>
        enum Column
        {
            movieId = 1,
            movieTitle,
            genre,
            actorsPercent,
            earnings,
            rating,
            releaseYear,
            actors

        }
        /// <summary>
        /// This enum containes actors' fields for changing.
        /// </summary>
        enum NestColumns
        {
            actorName = 1,
            nationality
        }
        private int _idx;
        private Movie[] _newMovie;
        private AutoSaver _autoSaver;
        public DataProcessingInterface() { }

        /// <summary>
        /// This constructor initializes index of processing.
        /// </summary>
        /// <param name="idx"></param>
        public DataProcessingInterface(int idx)
        {
            _idx = idx;
        }

        /// <summary>
        /// This method implements sorted interface and rturns sorted data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public Movie[] SortInterface(DataProcessing data)
        {
            // Printing menu of choosing sorting value.
            MainInterface.PrintColor("Please choose the number of field for sorting", ConsoleColor.Magenta, ConsoleColor.Cyan);
            Menu menu = new Menu("Use up/down keys to choose menu item.", new string[] { "movieId", "movieTitle", "genre", "actorsPercent", "earnings", "rating", "releaseYear" });

            // Getting user's choice of value.
            int num = menu.ActMenu();

            MainInterface.PrintColor($"You have chosen field {(Column)num} for sorting.", ConsoleColor.Magenta, ConsoleColor.Cyan);

            // Sorting movies.
            _newMovie = data.Sort(_idx, num);
            return _newMovie;
        }

        /// <summary>
        /// This method implements changing interface and returns changed data.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="movies"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public Movie[] ChangeInterface(DataProcessing data, Movie[] movies, ref bool flag)
        {
            // Creating new autosaver object for saving data if less than 15 seconds have passed.
            _autoSaver = new AutoSaver(movies);

            // Subscribing on events.
            for (int i = 0; i < movies.Length; i++)
            {
                for (int j = 0; j < movies[i].Actors.Length; j++)
                {
                    movies[i].ChangingEarnings += movies[i].Actors[j].ChangeEarnings;
                    movies[i].Actors[j].Updated += _autoSaver.Save;
                }
                movies[i].Updated += _autoSaver.Save;
            }

            // Repeating cycle for several changings.
            while (true)
            {
                MainInterface.PrintColor($"Please choose the object for changing. Enter number 1 <= N <= {movies.Length}", ConsoleColor.Magenta, ConsoleColor.DarkCyan);

                // Inex number of an object to change.
                int indexElem;

                // Checking correctness of input number.
                while (!int.TryParse(Console.ReadLine(), out indexElem) || indexElem < 1 || indexElem > movies.Length)
                    MainInterface.PrintColor("Wrong number. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);

                // Informing user about chosen object.
                MainInterface.PrintColor($"You have chosen object number {indexElem} for changing:", ConsoleColor.Magenta, ConsoleColor.Cyan);
                // -1 because user enters value starting with 1, not 0.
                Console.WriteLine(movies[indexElem - 1].ToJSON());

                // Index number of field to change.
                int indexColumn;
                // Index number of nested element to change.
                int indexComposeElem = 0;

                MainInterface.PrintColor("Please choose the field for changing", ConsoleColor.Magenta, ConsoleColor.DarkCyan);

                // Printing menu of choosing a field for changing.
                Menu menu = new Menu("Use up/down keys to choose field.", new string[] { "movieTitle", "genre", "actorsPercent", "earnings", "rating", "releaseYear", "actors" });
                indexColumn = menu.ActMenu();

                int choice = 1;

                // If field is not a nested array.
                if (indexColumn != 7)
                {
                    MainInterface.PrintColor($"You have chosen field {(Column)(indexColumn + 1)} for changing.", ConsoleColor.Magenta, ConsoleColor.Cyan);
                }

                // If field is an actors array.
                else
                {
                    // Printing menu for choosing actions with nested data.
                    MainInterface.PrintColor($"You have chosen the field with nested object. Do you want to change it or add new actor?", ConsoleColor.Yellow, ConsoleColor.DarkYellow);
                    menu = new Menu("Use up/down keys to choose menu item.", new string[] { "Change", "Add new actor", "Return to the menu" });

                    choice = menu.ActMenu();

                    // If user chooses change a nested array's field.
                    if (choice == 1)
                    {
                        MainInterface.PrintColor($"You are trying to change field in the nested array. Please choose the object for sorting. Enter number 1 <= N <= {movies[indexElem - 1].Actors.Length}", ConsoleColor.Yellow, ConsoleColor.DarkYellow);

                        // Checking correctness of input number of actor's object.
                        while (!int.TryParse(Console.ReadLine(), out indexComposeElem) || indexComposeElem < 1 || indexComposeElem > movies[indexElem - 1].Actors.Length)
                            MainInterface.PrintColor("Wrong number. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);

                        // Printing menu for choosing field to change.
                        Menu nestMenu = new Menu("Use up/down keys to choose field.", new string[] { "actorName", "nationality" });
                        indexColumn = nestMenu.ActMenu();

                        // Informing user abour his choice.
                        MainInterface.PrintColor($"You have chosen array Actors and field {(NestColumns)indexColumn} for changing.", ConsoleColor.Magenta, ConsoleColor.Cyan);

                        // Plus 6 because counting has been begun from the first field in movie object.
                        indexColumn += 6;
                    }

                    // If user chooses adding new actor's object.
                    else if (choice == 2)
                    {
                        MainInterface.PrintColor("Enter information about new actor", ConsoleColor.Magenta, ConsoleColor.DarkCyan);

                        // Adding new actor.
                        AddActorInterface(movies[indexElem - 1]);

                        // Changing resulting array.
                        _newMovie = movies;
                    }
                    // Returning to the menu.
                    else
                    {
                        flag = false;
                        return _newMovie;
                    }
                }
                // Getting value from user.
                if (choice == 1)
                {
                    Console.WriteLine("Enter value:");
                    string? input = Console.ReadLine();
                    double value = 0;
                    int intValue = 0;

                    // Checking correctness of the input value.
                    while (((indexColumn == 1 || indexColumn == 2 || indexColumn == 7 || indexColumn == 8) && string.IsNullOrEmpty(input)) || ((indexColumn == 3 || indexColumn == 4 || indexColumn == 5) && !double.TryParse(input, out value)) || (indexColumn == 6 && !int.TryParse(input, out intValue)))
                    {
                        MainInterface.PrintColor("Wrong value.Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                        input = Console.ReadLine();
                    }
                    // Changing result array.
                    _newMovie = indexColumn switch
                    {
                        1 or 2 => data.ChangeItem(indexElem - 1, indexColumn, input),
                        7 or 8 => data.ChangeItem(indexElem - 1, indexColumn, input, indexComposeElem - 1),
                        6 => data.ChangeItem(indexElem - 1, indexColumn, intValue),
                        _ => data.ChangeItem(indexElem - 1, indexColumn, value)

                    };
                    // Printing message if earnings have been changed.
                    if (indexColumn + 1 == (int)Column.earnings || indexColumn + 1 == (int)Column.actorsPercent)
                        MainInterface.PrintColor("Actor's earnings have been recounted", ConsoleColor.Yellow, ConsoleColor.DarkYellow);
                }
                // Printing an object after changing.
                Console.WriteLine("You have changed data. Now the object is:");
                Console.WriteLine(_newMovie[indexElem - 1].ToJSON());

                // Asking user about continuation of changing elements.
                menu = new Menu("Do you want to change another field", new string[] { "Yes", "No" });
                int decision = menu.ActMenu();

                // If user chooses to change another field.
                if (decision == 1)
                    continue;
                break;
            }
            return _newMovie;
        }
        /// <summary>
        /// This method implements interface of adding new Actor object in nested array.
        /// </summary>
        /// <param name="movie"></param>
        public void AddActorInterface(Movie movie)
        {
            string[] actorNames = new string[] { "actorId", "actorName", "nationality" };
            string[] actorValues;

            // Repeating cycle for several changings.
            while (true)
            {
                // Printing menu of choosing the way of enterring new object.
                Menu format = new Menu("How do you want to add new actor?", new string[] { "Print \"actor\" object in json format", "Enter values manually" });
                int num = format.ActMenu();

                // If user chooses to enter every field manually.
                if (num == 2)
                {
                    // Creating new array with actor's fields in string format.
                    actorValues = new string[actorNames.Length + 1];

                    // Getting actor's fields from user.
                    for (int i = 0; i < actorNames.Length; i++)
                    {
                        Console.Write($"Enter {actorNames[i]}:");
                        string? input = Console.ReadLine();

                        // Checking correctness if input values.
                        while ((i == 0 || i == 1 || i == 2) && string.IsNullOrEmpty(input))
                        {
                            MainInterface.PrintColor("Wrong value. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                            Console.Write($"Enter {actorNames[i]}:");
                            input = Console.ReadLine();
                        }
                        actorValues[i] = input;
                    }
                    // Initializing earnings with 0.
                    actorValues[^1] = "0";

                    // Creating a new actor object.
                    Actor actor = new Actor(actorValues[0], actorValues[1], actorValues[2], double.Parse(actorValues[3]));

                    // Subscribing new actor object on events.
                    movie.ChangingEarnings += actor.ChangeEarnings;
                    actor.Updated += _autoSaver.Save;

                    // Adding new actor to a movies' array. Changing actors' earnings.
                    movie.AddActor(actor);

                    // Informing user about new earnings.
                    MainInterface.PrintColor($"Actor's earnings are counted automatically. It is {movie.Actors[0].Earnings} for every actor in this movie.", ConsoleColor.Yellow, ConsoleColor.DarkYellow);
                }
                // If user chooses to enter data in json format.
                else
                {
                    MainInterface.PrintColor("Enter your data. To finish enter \"exit\" at new line", ConsoleColor.Magenta, ConsoleColor.DarkCyan);
                    StringBuilder sb = new StringBuilder();
                    string? line;

                    // Reading data while !exit
                    while ((line = Console.ReadLine()) != null && line != "exit")
                    {
                        sb.Append(line);
                    }
                    try
                    {
                        // Trying to deserealize printed data to actor object.
                        Actor? actor = JsonSerializer.Deserialize<Actor>(sb.ToString());

                        // Subscribing new object on events.
                        movie.ChangingEarnings += actor.ChangeEarnings;
                        actor.Updated += _autoSaver.Save;

                        // Adding new actor to movies' array and changing earnings.
                        movie.AddActor(actor);
                    }
                    catch (JsonException)
                    {
                        MainInterface.PrintColor("Invalid format.Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                        continue;
                    }
                    catch (NotSupportedException)
                    {
                        MainInterface.PrintColor("Invalid json data. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                        continue;
                    }
                    catch (ArgumentNullException)
                    {
                        MainInterface.PrintColor("Wrong data.Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                        continue;
                    }
                    catch (ArgumentException)
                    {
                        MainInterface.PrintColor("Wrong data. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                        continue;
                    }
                    catch (NullReferenceException)
                    {
                        MainInterface.PrintColor("Sorry, there is a problem with data. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                        continue;
                    }

                }
                // Printing menu about enterring another nested object.
                Menu menu = new Menu("Do you want to enter another actor?", new string[] { "Yes", "No" });
                int numb = menu.ActMenu();

                if (numb == 1)
                    continue;
                break;
            }
        }
    }
}
