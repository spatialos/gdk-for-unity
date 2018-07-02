using System;
using System.Collections.Generic;
using System.IO;

namespace DocsLinter
{
    /// <summary>
    ///     Helper class for loading the dotenv file.
    /// </summary>
    public class DotenvLoader
    {
        /// <summary>
        ///     Helper method that parses the dotenv file.
        ///     The dotenv file is assumed to be in the cwd.
        /// </summary>
        /// <returns>The dotenv file parsed into environment variables.</returns>
        public static Dictionary<string, string> LoadDotenvFile()
        {
            var dotenvContents = new Dictionary<string, string>();

            var dotenvFileContents = File.ReadAllLines(".env");

            foreach (var dotenvRow in dotenvFileContents)
            {
                var index = dotenvRow.IndexOf("=");

                if (index < 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(
                        $"Warning: Malformed environment variable declaration in .env detected: {dotenvRow}");
                    Console.ResetColor();
                    continue;
                }

                var key = dotenvRow.Substring(0, index).Trim();
                var value = dotenvRow.Substring(index + 1, dotenvRow.Length - (index + 1)).Trim();

                if (key.Length > 0 && value.Length > 0)
                {
                    dotenvContents.Add(key, value);
                }
            }

            return dotenvContents;
        }
    }
}
