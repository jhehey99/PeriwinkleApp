using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PeriwinkleApp.Core.Sources.CommonInterfaces;
using DiagDebug = System.Diagnostics.Debug;
namespace PeriwinkleApp.Core.Sources.Utils
{
    public static class Logger
    {
        public static void Log(string message, string tag ="",
                               [CallerFilePath] string filePath = "",
                               [CallerMemberName] string method = "")
		{
			message = message ?? "null";
            DiagDebug.WriteLine($"\n<--\n\t<FilePath:\t{filePath} />" +
                                $"\n\t<Tag:\t{tag} />" +
                                $"\n\t<Method:\t{method} />\n" +
                                $"<Message:\n\t\t{message}" +
                                "\n/>\n--/>\n");
        }

        public static void LogList (IEnumerable <string> messages,
                                    string tag = "",
                                    [CallerFilePath] string filePath = "",
                                    [CallerMemberName] string method = "")
        {
            DiagDebug.WriteLine ($"\n<--\n\t<FilePath:\t{filePath} />" +
                                 $"\n\t<Tag:\t{tag} />" +
                                 $"\n\t<Method:\t{method} />\n" +
                                 $"<Message:");

            foreach (string message in messages)
            {
                DiagDebug.WriteLine ("\t\t" + message);
            }

            DiagDebug.WriteLine ("\n/>\n--/>\n");
        }

        public static void Debug (IDebugString iDebugString, 
                                  bool isJson = false, string tag="",
                                  [CallerFilePath] string filePath = "",
                                  [CallerMemberName] string method = "")
        {
            DiagDebug.WriteLine($"\n<--\n\t<FilePath:\t{filePath} />" +
                                $"\n\t<Tag:\t{tag} />" +
                                $"\n\t<Method:\t{method} />\n" +
                                $"<Message:");
            if(!isJson)
                DiagDebug.WriteLine ("\t" + iDebugString.ToDebug ());
            else
                DiagDebug.WriteLine (iDebugString.ToDebug ());
                    
            DiagDebug.WriteLine ("\n/>\n--/>\n");
        }
        
        public static void DebugList (IEnumerable <IDebugString> iDebugStrings,
                                      bool isJson = false, string tag = "",
                                      [CallerFilePath] string filePath = "",
                                      [CallerMemberName] string method = "")
        {
            DiagDebug.WriteLine ($"\n<--\n\t<FilePath:\t{filePath} />" +
                                 $"\n\t<Tag:\t{tag} />" +
                                 $"\n\t<Method:\t{method} />\n" +
                                 $"<Message:");

            foreach (IDebugString iDebugString in iDebugStrings)
            {
                if(!isJson)
                    DiagDebug.WriteLine ("\t\t" + iDebugString.ToDebug ());
                else
                    DiagDebug.WriteLine (iDebugString.ToDebug ());
            }

            DiagDebug.WriteLine ("\n/>\n--/>\n");
        }
        
        
        
    }
}
