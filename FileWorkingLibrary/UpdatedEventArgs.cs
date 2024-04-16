namespace FileWorkingLibrary
{
    /// <summary>
    /// This class contains data that must be given to subscribers when the event has been fired.
    /// </summary>
    public class UpdatedEventArgs:EventArgs
    {
        /// <summary>
        /// This property returns and sets the date of last object's changing.
        /// </summary>
        public DateTime Date { get; private set; }
        /// <summary>
        /// This property returns message of successful saving.
        /// </summary>
        public string Message {  get; private set; }
        /// <summary>
        /// This constructor initializes fields with data.
        /// </summary>
        /// <param name="date"></param>
        public UpdatedEventArgs(DateTime date)
        {
            Date = date;
            Message = "You have changed data with an interval, which is less than 15 seconds. Data has been automatically saved to the file 9V_tmp.json in the same folder with this project.";
        }
    }
}
