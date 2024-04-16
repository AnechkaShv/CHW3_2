using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FileWorkingLibrary
{
    [Serializable]
    public class Actor
    {
        private string _actorId, _actorName, _nationality;
        private double _earnings;

        /// <summary>
        /// This property returns or sets actor's id and checks nullability of setting value. 
        /// </summary>
        [JsonPropertyName("actorId")]
        public string ActorId 
        {
            get { return _actorId; } 
            init
            {
                if(string.IsNullOrEmpty(value))
                    throw new ArgumentNullException();
                _actorId = value;
            }
        }
        /// <summary>
        /// This property returns or sets actor's name and checks nullability of setting value.
        /// </summary>
        [JsonPropertyName("actorName")]
        public string ActorName 
        {
            get { return _actorName; } 
            set
            {
                if( string.IsNullOrEmpty(value) )
                    throw new ArgumentNullException();
                _actorName = value;
                // Firing event when field is changed.
                OnChangeField();
            }
        }
        /// <summary>
        /// This property returns or sets actor's nationality and checks nullability of setting value.
        /// </summary>
        [JsonPropertyName("nationality")]
        public string Nationality 
        {
            get { return _nationality; }
            set
            {
                if(string.IsNullOrEmpty( value) )
                    throw new ArgumentNullException();
                _nationality = value;
                // Firing event when field is changed.
                OnChangeField();
            }
        }
        /// <summary>
        /// This property returns or sets actor's earning and checks correctness of setting value.
        /// </summary>
        [JsonPropertyName("earnings")]
        public double Earnings 
        {
            get { return _earnings; } 
            set
            {
                if(value<0)
                    throw new ArgumentException();
                _earnings = value;
            }
        }
        /// <summary>
        /// This constructor initializes fields with empty objects.
        /// </summary>
        public Actor()
        {
            _actorId = string.Empty;
            _actorName = string.Empty;
            _nationality = string.Empty;
            _earnings = 0;
        }
        /// <summary>
        /// This constructor initializes fields with input values.
        /// </summary>
        /// <param name="actorId"></param>
        /// <param name="actorName"></param>
        /// <param name="nationality"></param>
        /// <param name="earnings"></param>
        public Actor(string actorId, string actorName, string nationality, double earnings)
        {
            ActorId = actorId;
            ActorName = actorName;
            Nationality = nationality;
            Earnings = earnings;
        }
        /// <summary>
        /// This event informs subscribers about changing fields in the object.
        /// </summary>
        public event EventHandler<UpdatedEventArgs> Updated;

        /// <summary>
        /// This method fires event to save data.
        /// </summary>
        private void OnChangeField()
        {
            Updated?.Invoke(this, new UpdatedEventArgs(DateTime.Now));
        }
        /// <summary>
        /// This method reacts on earnings' changing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ChangeEarnings(object sender, ChangeEarningsEventArgs e)
        {
            _earnings = Math.Round(e.Earnings, 2);
        }
        /// <summary>
        /// This method converts object to json format.
        /// </summary>
        /// <returns></returns>
        public string ToJSON()
        {

            string[] names = new string[] { "actorId", "actorName", "nationality", "earnings" };
            object[] objects = new object[] { ActorId, ActorName, Nationality, Earnings };

            StringBuilder sb = new StringBuilder();

            // For formatted printing json data.
            JsonSerializerOptions serializerOptions = new JsonSerializerOptions();
            serializerOptions.WriteIndented = true;

            // Creating correct looking json string.
            sb.Append("      {\n");
            for (int i = 0; i < names.Length; i++)
            {
                // Writing all objects with coma in the end except last object
                if (i != names.Length - 1)
                    sb.Append($"        \"{names[i]}\": {JsonSerializer.Serialize(objects[i], serializerOptions)},\n");
                else
                    sb.Append($"        \"{names[i]}\": {JsonSerializer.Serialize(objects[i], serializerOptions)}\n");
            }
            sb.Append("      }");
            return sb.ToString();
        }
    }
}
