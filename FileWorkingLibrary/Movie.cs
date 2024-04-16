using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FileWorkingLibrary
{
    [Serializable]
    public class Movie
    {
        private string _movieTitle, _genre;
        private string _movieId;
        private double _actorsPercent, _rating;
        private double _earnings;
        private int _releaseYear;
        private Actor[] _actors;

        /// <summary>
        /// This property returns or sets movie's id and checks its correctness.
        /// </summary>
        [JsonPropertyName("movieId")]
        public string MovieId
        {
            get { return _movieId; }
            init
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException();

                _movieId = value;
            }
        }

        /// <summary>
        /// This property returns and sets movie's title and checks its correctness.
        /// </summary>
        [JsonPropertyName("movieTitle")]
        public string MovieTitle 
        { 
            get => _movieTitle; 
            set { 
                if(String.IsNullOrEmpty(value))
                    throw new ArgumentNullException();

                // If filed hav been changed event will be fired.
                if(_movieTitle != null)
                    OnChangeField();
                _movieTitle = value;               
            } 
        }

        /// <summary>
        /// This property returns or sets movie's genre and checks correctness of setting value.
        /// </summary>
        [JsonPropertyName("genre")]
        public string Genre 
        { 
            get => _genre;
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException();

                _genre = value;
                OnChangeField();
            }
        }

        /// <summary>
        /// This property returns or sets actors' percent in the movie and checks correctness of setting value.
        /// </summary>
        [JsonPropertyName("actorsPercent")]
        public double ActorsPercent 
        {
            get => _actorsPercent;
            set 
            {
                if (value < 0)
                    throw new ArgumentException();
                _actorsPercent = value;

                // Firing event to change all actors' earnings
                OnChangeEarnings();

                // Firing event for saving changed data.
                OnChangeField();
            }
        }

        /// <summary>
        /// This property returns or sets movie's earnings and checks correctness of setting value.
        /// </summary>
        [JsonPropertyName("earnings")]
        public double Earnings
        {
            get => _earnings;
            set
            {
                if (value < 0)
                    throw new ArgumentException();
                _earnings = value;

                // Firing event to change all actors' earnings
                OnChangeEarnings();

                // Firing event for saving chenged data.
                OnChangeField();
            }
        }

        /// <summary>
        /// This property returns or sets movie's rating and checks correctness of setting value.
        /// </summary>
        [JsonPropertyName("rating")]
        public double Rating 
        { 
            get => _rating; 
            set 
            { 
                if (value < 0)
                    throw new ArgumentException();
                _rating = value;
                OnChangeField();
            }
        }

        /// <summary>
        /// This property returns or sets movie's release year and checks correctness of setting value.
        /// </summary>
        [JsonPropertyName("releaseYear")]
        public int ReleaseYear 
        { 
            get => _releaseYear;  
            set 
            { 
                // Release year can't be less then year when cinema had been invented and more than current year.
                if(value <= 1870 || value > DateTime.Now.Year)
                {
                    throw new ArgumentException();
                }
                _releaseYear =  value;

                // Firing event for saving chenged data.
                OnChangeField();
            }
        }

        /// <summary>
        /// This property returns or sets array of actors in the movie and checks nullability of setting value.
        /// </summary>
        [JsonPropertyName("actors")]
        public Actor[] Actors 
        { 
            get => _actors;           
            set 
            { 
                if(value == null)
                    throw new ArgumentNullException();
                _actors = value;

                // Firing event for saving chenged data.
                OnChangeField();
            }
        }

        public Movie() 
        {
            _movieId = string.Empty;
            _movieTitle = string.Empty;
            _genre = string.Empty;
            _actors = Array.Empty<Actor>();
        }
        /// <summary>
        /// This constructor initializes the object's fields and use composition for filling actors' array.
        /// </summary>
        /// <param name="movieId"></param>
        /// <param name="movieTitle"></param>
        /// <param name="genre"></param>
        /// <param name="actorsPercent"></param>
        /// <param name="earnings"></param>
        /// <param name="rating"></param>
        /// <param name="releaseYear"></param>
        /// <param name="actors"></param>
        public Movie(string movieId, string movieTitle, string genre, double actorsPercent, double earnings, double rating, int releaseYear, Tuple<string, string, string, double>[] actors)
        {
            MovieId = movieId;
            MovieTitle = movieTitle;
            Genre = genre;
            ActorsPercent = actorsPercent;
            Rating = rating;
            ReleaseYear = releaseYear;
            Earnings = earnings;
            Actors = new Actor[actors.Length];
            for (int i = 0; i < actors.Length; i++)
            {
                Actors[i] = new Actor(actors[i].Item1, actors[i].Item2, actors[i].Item3, actors[i].Item4);
            }
        }

        /// <summary>
        /// This method adds new actor in nested array.
        /// </summary>
        /// <param name="actor"></param>
        public void AddActor(Actor actor)
        {
            // Increasing size of actors' array.
            Array.Resize(ref _actors, _actors.Length + 1);
            _actors[^1] = actor;

            // Invoking event of updating data and changing actors' earnings.
            OnChangeEarnings();
            OnChangeField();
        }

        /// <summary>
        /// This event will be invoked when the object has been changed.
        /// </summary>
        public event EventHandler<UpdatedEventArgs> Updated;
        /// <summary>
        /// This event will be invoked when actors' percent or earnings have changed to change actors' earnings.
        /// </summary>
        public event EventHandler<ChangeEarningsEventArgs> ChangingEarnings;

        /// <summary>
        /// This method counts new actors' earnings and invokes the event to change it.
        /// </summary>
        private void OnChangeEarnings()
        {
            double sum = _actorsPercent/100 * _earnings;
            ChangingEarnings?.Invoke(this, new ChangeEarningsEventArgs(sum / _actors.Length));
            
        }
        /// <summary>
        /// This method invokes the event to update fields.
        /// </summary>
        private void OnChangeField()
        {
            Updated?.Invoke(this, new UpdatedEventArgs(DateTime.Now));
        }
        /// <summary>
        /// This method returns json formatted string of a movie object.
        /// </summary>
        /// <returns></returns>
        public string ToJSON()
        {
            string[] names = new string[] { "movieId", "movieTitle", "earnings", "actorsPercent", "releaseYear", "genre", "rating", "actors" };
            object[] objects = new object[] { MovieId, MovieTitle, Earnings, ActorsPercent, ReleaseYear, Genre, Rating, Actors };

            // Returning string.
            StringBuilder sb = new StringBuilder();

            // Setting json options to print data in json format with spaces.
            JsonSerializerOptions serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;

            // Beginning an object string with {. 
            sb.Append("  {\n");

            // Writing formatted wtring with an object's fields.
            for (int i = 0; i < names.Length - 1; i++)
            {
                sb.Append($"    \"{names[i]}\": {JsonSerializer.Serialize(objects[i], serializerOptions)},\n");
            }

            // Writing actors' array.
            sb.Append($"    \"{names[7]}\": [\n");
            for (int i = 0; i < Actors.Length; i++)
            {
                // Printing all actor's objects with coma at the end except last actor.
                if (i != Actors.Length - 1)
                    sb.Append($"{Actors[i].ToJSON()},\n");
                else
                    sb.Append($"{Actors[i].ToJSON()}\n");
            }

            // Ending ab object's string.
            sb.Append("    ]\n");
            sb.Append("  }");
            return sb.ToString();
        }
    }
}