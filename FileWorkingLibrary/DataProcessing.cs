using System.Text;
using System.Text.Json;

namespace FileWorkingLibrary
{
    public class DataProcessing
    {
        public Movie[] Movies { get; private set; }
        public DataProcessing() { }
        /// <summary>
        /// This constructor initializing movies array with deserializing data from the user's file.
        /// </summary>
        /// <param name="fPath"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DataProcessing(string fPath)
        {
            Movie[]? movies = JsonSerializer.Deserialize<Movie[]>(ReadJson(fPath));

            // Checking correctness of deserealization.
            if (movies is null || movies.Length <= 0)
                throw new ArgumentNullException();
            Movies = movies;
        }
        /// <summary>
        /// This constructor initializes movies array with deserealizing data from the console.
        /// </summary>
        /// <param name="sb"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DataProcessing(StringBuilder sb) 
        {
            Movie[]? movies = JsonSerializer.Deserialize<Movie[]>(sb.ToString());

            // Checking correctness of deserealization.
            if (movies is null || movies.Length <= 0)
                throw new ArgumentNullException();
            Movies = movies;
        }
        /// <summary>
        /// This constructor initializes movie array with movie array.
        /// </summary>
        /// <param name="movies"></param>
        public DataProcessing(Movie[] movies)
        {
            Movies = movies;
        }
        /// <summary>
        /// This method reads from the file and returns string with data.
        /// </summary>
        /// <param name="fPath"></param>
        /// <returns></returns>
        private string ReadJson(string fPath)
        {
            // Setting english culture to read json double array correctly.
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            StringBuilder sb = new StringBuilder();

            // Stream redirection.
            using (StreamReader sr = new StreamReader(fPath))
            {
                Console.SetIn(sr);
                string? line;
                while ((line = Console.ReadLine()) != null)
                {
                    sb.Append(line);
                }
                // Returning the the ordinary input stream.
                Console.SetIn(new StreamReader(Console.OpenStandardInput()));
            }
            return sb.ToString();
        }
        /// <summary>
        /// This method return sorted data
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="indexColumn"></param>
        /// <returns></returns>
        public Movie[] Sort(int idx, int indexColumn)
        {
            Movie[]? sortedData = Movies;

            // Ascending sorting.
            if(idx == 1)
            {
                sortedData = indexColumn switch
                {
                    1 => Movies.OrderBy(e => e.MovieTitle).ToArray(),
                    2 => Movies.OrderBy(e => e.MovieId).ToArray(),
                    3 => Movies.OrderBy(e => e.Genre).ToArray(),
                    4 => Movies.OrderBy(e => e.ActorsPercent).ToArray(),
                    5 => Movies.OrderBy(e => e.Earnings).ToArray(),
                    6 => Movies.OrderBy(e => e.Rating).ToArray(),
                    7 => Movies.OrderBy(e => e.ReleaseYear).ToArray(),
                    8 => Movies.OrderBy(e => e.MovieTitle).ToArray(),
                };
            }
            // Descending sorting.
            else
            {
                sortedData = indexColumn switch
                {
                    1 => Movies.OrderByDescending(e => e.MovieTitle).ToArray(),
                    2 => Movies.OrderByDescending(e => e.MovieId).ToArray(),
                    3 => Movies.OrderByDescending(e => e.Genre).ToArray(),
                    4 => Movies.OrderByDescending(e => e.ActorsPercent).ToArray(),
                    5 => Movies.OrderByDescending(e => e.Earnings).ToArray(),
                    6 => Movies.OrderByDescending(e => e.Rating).ToArray(),
                    7 => Movies.OrderByDescending(e => e.ReleaseYear).ToArray()
                };
            }
            return sortedData;
        }

        /// <summary>
        /// This method return movies array after changing string fields by user.
        /// </summary>
        /// <param name="indexElem"></param>
        /// <param name="indexColumn"></param>
        /// <param name="value"></param>
        /// <param name="indexComposeElem"></param>
        /// <returns></returns>
        public Movie[] ChangeItem(int indexElem, int indexColumn, string value, int indexComposeElem = 0)
        {
            // Changing the desired field.
            switch(indexColumn)
            {
                case 1:
                    Movies[indexElem].MovieTitle = value;
                    break;
                case 2:
                    Movies[indexElem].Genre = value;
                    break;
                case 7:
                    Movies[indexElem].Actors[indexComposeElem].ActorName = value;
                    break;
                case 8:
                    Movies[indexElem].Actors[indexComposeElem].Nationality = value;
                    break;
            }
            return Movies;
        }

        /// <summary>
        /// This method return movies array after changing double fields by user.
        /// </summary>
        /// <param name="indexElem"></param>
        /// <param name="indexColumn"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public Movie[] ChangeItem(int indexElem, int indexColumn, double value)
        {
            switch (indexColumn)
            {
                case 3:
                    Movies[indexElem].ActorsPercent = value;
                    break;
                case 4:
                    Movies[indexElem].Earnings = value;
                    break;
                case 5:
                    Movies[indexElem].Rating = value;
                    break;
                case 6:
                    Movies[indexElem].ReleaseYear = (int)value;
                    break;
            }
            return Movies;
        }

        /// <summary>
        /// This method writes processed data.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="print"></param>
        public void WriteJson(string fileName, Action print)
        {
            // Redirecting output stream.
            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                Console.SetOut(sw);
                print();
            }
            // Returning to the ordinary stream.
            StreamWriter stream = new StreamWriter(Console.OpenStandardOutput());
            stream.AutoFlush = true;
            Console.SetOut(stream);
        }
    }
}
