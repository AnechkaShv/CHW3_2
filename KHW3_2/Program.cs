using InterfaceLibrary;

namespace KHW3_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            do
            {
                try
                {
                    MainInterface program = new MainInterface();
                    int num;
                    program.ShowMenu(out num);
                    if (num != 4)
                    {
                        program.SaveData();
                    }
                }
                catch (ArgumentNullException)
                {
                    MainInterface.PrintColor("Wrong file. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (PathTooLongException)
                {
                    MainInterface.PrintColor("Your file name is too long. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (DirectoryNotFoundException)
                {
                    MainInterface.PrintColor("Wrong file path. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (UnauthorizedAccessException)
                {
                    MainInterface.PrintColor("This file can be only read. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (IOException)
                {
                    MainInterface.PrintColor("Error while opening file. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (ArgumentOutOfRangeException)
                {
                    MainInterface.PrintColor("Error while working with arrays. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (ArgumentException)
                {
                    MainInterface.PrintColor("Error while working with file. Please try again.", ConsoleColor.Red, ConsoleColor.DarkRed);
                    continue;
                }
                catch (NullReferenceException)
                {
                    MainInterface.PrintColor("Something went wrong while working with file. Please try again", ConsoleColor.Red, ConsoleColor.DarkRed);
                }
                Console.WriteLine("Enter key to restart. To exit enter Escape.");
                Console.WriteLine();
            } while (Console.ReadKey().Key != ConsoleKey.Escape);
        }
    }
}