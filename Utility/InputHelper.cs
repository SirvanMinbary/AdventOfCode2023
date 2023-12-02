using System;
using System.IO;

namespace Utility
{
    public static class InputHelper
    {
        public static string[] ReadInputFile(string fileName)
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), @$"Advent of Code/2023/{fileName}");
            return File.ReadAllLines(path);
        }
    }
}