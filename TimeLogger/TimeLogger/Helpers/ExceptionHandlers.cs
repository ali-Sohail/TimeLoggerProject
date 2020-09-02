using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace TimeLogger.Helpers
{
    public class ExceptionHandlers : Exception
    {
        public ExceptionHandlers(string message) : base(message)
        {
        }

        public ExceptionHandlers(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ExceptionHandlers(Exception exception,
            [CallerFilePath] string filePath = "",
            [CallerLineNumber] int lineNumber = 0,
            [CallerMemberName] string callerMemberName = "") : base(exception.Message, exception)
        {
            var fileName = System.IO.Path.GetFileName(filePath);
            Debug.WriteLine($"\nException Occur : {exception.GetType()} \nError: {exception.Message} \nLine Number: {lineNumber} \nCaller Name: {callerMemberName} \nFile Name: {fileName}\n");
            //Microsoft.AppCenter.Crashes.Crashes.TrackError(exception, new System.Collections.Generic.Dictionary<string, string> { { "Class Name : ", GetType().Name }, { "Method Name : ", System.Reflection.MethodBase.GetCurrentMethod().Name } });
        }
    }

    public static class ExceptionLogger
    {
        public static void LogException(Exception exception,
           [CallerFilePath] string filePath = "",
           [CallerLineNumber] int lineNumber = 0,
           [CallerMemberName] string callerMemberName = "")
        {
            var fileName = System.IO.Path.GetFileName(filePath);
            Debug.WriteLine($"\nException Occur : {exception.GetType()} \nError: {exception.Message} \nLine Number: {lineNumber} \nCaller Name: {callerMemberName} \nFile Name: {fileName}\n");
            //Microsoft.AppCenter.Crashes.Crashes.TrackError(exception, new System.Collections.Generic.Dictionary<string, string> { { "FilePath : ", fileName }, { "Method Name : ", System.Reflection.MethodBase.GetCurrentMethod().Name } });
        }
    }
}