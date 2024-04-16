using System.Text.Json;

namespace FileWorkingLibrary
{
    public class AutoSaver
    {
        private DateTime firstDate;

        private Movie[]? movies;
        public AutoSaver() { }

        /// <summary>
        /// This constructor initializes array with movies.
        /// </summary>
        /// <param name="movies"></param>
        public AutoSaver(Movie[] movies)
        {
            this.movies = movies;
        }
        /// <summary>
        /// This method checks changing's time and saves movie array.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Save(object sender, UpdatedEventArgs e)
        {
            // If it is first changing.
            if(firstDate.ToBinary() == 0)
                firstDate = e.Date;

            // If less than 15 seconds have passed after last changing data will be saved.
            // Counting seconds until the beginning of the year.
            else if((e.Date - firstDate.Date).TotalSeconds <= 15000)
            {
                // Current date has become last changing date.
                firstDate = e.Date;

                // For formatted printing a json object.
                JsonSerializerOptions js = new JsonSerializerOptions();
                js.WriteIndented = true;

                // Writing mevies array in the file.
                using (StreamWriter fileStream = new StreamWriter("9V_tmp.json"))
                {
                    //FileStream fileStream = new FileStream("9V_tmp.json", FileMode.OpenOrCreate);
                    fileStream.WriteLine(JsonSerializer.Serialize(movies, js));
                }
                //fileStream.Flush();
                //fileStream.Close();

                //  Informing user about changing.
                Console.WriteLine(e.Message);
            }
            // If more than 15 seconds have passed after last changing.
            else
            {
                firstDate = e.Date;
            }

            if (sender is Movie)
                ChangeData((Movie)sender);
            else
                ChangeData((Actor)sender);
        }
        /// <summary>
        /// This method changes movie's field.
        /// </summary>
        /// <param name="movie"></param>
        private void ChangeData(Movie movie)
        {
            // Searching the element with the same id.
            for (int i = 0; i < movies.Length; i++)
            {
                if (movies[i].MovieId == movie.MovieId)
                    movies[i] = movie;
            }
        }
        /// <summary>
        /// This method changes actor's field
        /// </summary>
        /// <param name="actor"></param>
        private void ChangeData(Actor actor)
        {
            // Searching the element with corresponded actor id.
            for (int i = 0; i < movies.Length; i++)
            {
                for (int j = 0; j < movies[i].Actors.Length; j++)
                {
                    if (movies[i].Actors[j].ActorId == actor.ActorId)
                        movies[i].Actors[j] = actor;
                }
            }
        }
    }
}
