using System;
using System.Collections.Generic;
using System.Linq;

namespace PeriwinkleApp.Core.Test.Utils
{
    public enum TestFiles
    {
        ValidPasswords,
        ValidUsernames,
        ValidFirstNames,
        ValidLastNames,
        ValidContacts,
        ValidEmails
    }
    
    public class FileUtils
    {
        private static readonly Dictionary <TestFiles, string> testFileMap;
        private static readonly string filesDir;
        static FileUtils ()
        {
            filesDir = @"D:\DevDox\RiderProjects\PeriwinkleApp\PeriwinkleApp.Core.Test\Files\Validation\";
            
            // Add ka dito ng bagong test file at file location nya
            testFileMap = new Dictionary <TestFiles, string> ()
            {
                {TestFiles.ValidPasswords, "ValidPasswords.txt"},
                {TestFiles.ValidUsernames, "ValidUsernames.txt"},
                {TestFiles.ValidFirstNames, "ValidFirstNames.txt"},
                {TestFiles.ValidLastNames, "ValidLastNames.txt"},
                {TestFiles.ValidContacts, "ValidContacts.txt"},
                {TestFiles.ValidEmails, "ValidEmails.txt"}
            };
        }
        
        public static List <string> FileToStringList(TestFiles testFile)
        {
            return System.IO.File.ReadAllLines(filesDir + testFileMap[testFile]).ToList ();
        }
        
        public static List <string> FileToStringList(string filename = default(string))
        {
            return filename == default (string) ? null : System.IO.File.ReadAllLines(filename).ToList ();
        }
    }
}
