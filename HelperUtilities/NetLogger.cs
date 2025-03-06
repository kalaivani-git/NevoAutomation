namespace Nevo.HelperUtilities
{
    internal class NetLogger
    {
        /// <summary>
        /// Indicates the Logging Severity
        /// </summary>
        public enum Severity
        {
            /// <summary>
            /// Only Information
            /// </summary>
            Information,
            /// <summary>
            /// Error, Usually one cannot continue after an error.
            /// </summary>
            Error,
            /// <summary>
            /// Only Exception, One can catch and continue on exceptions.
            /// </summary>
            Exception,
            /// <summary>
            /// Trace
            /// </summary>
            Trace
        }
        /// <summary>
        /// Name of the logfile.
        /// </summary>
        private string _logFile = Directory.GetParent(@"../../../").FullName +
            Path.DirectorySeparatorChar + @"\Logs\Log_" +
            DateTime.Now.Year + "_" +
            DateTime.Now.Month + "_" +
            DateTime.Now.Day;
        // The Pointer to the file to which Console class will write to.
        private static StreamWriter _fileWriter = null;
      
        public static void Log(Severity severeValue, string message)
        {
            NetLogger fl = new NetLogger();
            fl.LogMe(severeValue, message);
        }
        private void LogMe(Severity severeValue, string message)
        {
            try
            {
                lock (this)
                {
                    SetOut();
                    Console.WriteLine(
                    DateTime.Now.ToString() + " - " + severeValue + " : " + message);
                    Reset();
                }
            }
            catch
            {
            }
        }
        /// <summary>
        /// Sets the Console.Out
        /// </summary>
        private void SetOut()
        {
            try
            {
                string fileName = _logFile + ".txt";
                if (_fileWriter == null)
                {
                    if (!File.Exists(fileName))
                    {
                        if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                        {
                            Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                        }
                        FileStream fs = File.Create(fileName);
                        fs.Close();
                    }
                }
                _fileWriter = new StreamWriter(fileName, true);
                Console.SetOut(_fileWriter);
            }
            catch
            {
            }
        }
        /// <summary>
        /// Resets the Console.Out
        /// </summary>
        private void Reset()
        {
            try
            {
                if (_fileWriter != null)
                {
                    _fileWriter.Close();
                }
                StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
                standardOutput.AutoFlush = true;
                Console.SetOut(standardOutput);
            }
            catch
            {
            }
        }
    }
}
