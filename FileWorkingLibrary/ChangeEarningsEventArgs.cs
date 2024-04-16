namespace FileWorkingLibrary
{
    /// <summary>
    /// This class contains data that muct be given to subscribers when event has been fired.
    /// </summary>
    public class ChangeEarningsEventArgs
    {
        /// <summary>
        /// This property returns or sets actor's earnings.
        /// </summary>
        public double Earnings {  get; private set; }
        /// <summary>
        /// This constructor initialize earnings with checking it.
        /// </summary>
        /// <param name="earnings"></param>
        /// <exception cref="ArgumentException"></exception>
        public ChangeEarningsEventArgs(double earnings) 
        {  
            if(earnings < 0)
                throw new ArgumentException();
            Earnings = earnings;
        }
    }
}
