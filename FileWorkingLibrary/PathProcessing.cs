namespace FileWorkingLibrary
{
    public class PathProcessing
    {
        char[] invalidPathChars = Path.GetInvalidPathChars();
        private string? _fPath, _nPath;
        /// <summary>
        /// This property initializes and checks path of the input file.
        /// </summary>
        public string FPath
        {
            get
            {
                // Checking path and returning its value.
                if (IsFPathCorrect(_fPath))
                    return _fPath;
                else
                    throw new ArgumentException();
            }
            set
            {
                // Checking path and setting value
                if (IsFPathCorrect(value))
                    _fPath = value;
                else
                    throw new ArgumentException();
            }
        }
        /// <summary>
        /// This property initializes and checks path of the output file.
        /// </summary>
        public string NPath
        {
            get
            {
                // Checking path and returning its value.
                if (IsNPathCorrect(_nPath))
                    return _nPath;
                else
                    throw new ArgumentException();
            }
            set
            {
                // Checking path and setting value
                if (IsNPathCorrect(value))
                    _nPath = value;
                else
                    throw new ArgumentException();
            }
        }
        public PathProcessing() { }
        /// <summary>
        /// This constructor initialize a path.
        /// </summary>
        /// <param name="fPath"></param>
        public PathProcessing(string fPath)
        {
            _fPath = fPath;
            _nPath = fPath;
        }
        /// <summary>
        /// This method checks the initial path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool IsFPathCorrect(string? path)
        {
            // Allowing files with only json extension.
            if (path is null || path.Length <= 0 || !File.Exists(path) || path.IndexOfAny(invalidPathChars) != -1 || path[^5..] != ".json")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        ///<summary>
        /// This method checks a new path.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private bool IsNPathCorrect(string? path)
        {
            // Allowing files with only json extension.
            if (path == null || path.IndexOfAny(invalidPathChars) != -1 || path.Length <= 0 || path == " " || path[^5..] != ".json")
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
