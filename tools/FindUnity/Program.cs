using System;
using System.Linq;
using UnityPaths;

namespace FindUnity
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Contains("--help") || args.Contains("/?") || args.Contains("/help"))
            {
                Paths.PrintHelp();
                Environment.Exit(0);
            }

            try
            {
                Console.Out.WriteLine(Paths.TryGetUnityPath());
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                Environment.Exit(1);
            }
        }
    }
}
