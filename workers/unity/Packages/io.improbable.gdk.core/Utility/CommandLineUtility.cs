using System;
using System.Collections.Generic;

namespace Improbable.Gdk.Core
{
    public class CommandLineArgs
    {
        private Dictionary<string, string> arguments;

        // Hide default constructor
        private CommandLineArgs()
        {
        }

        public static CommandLineArgs FromCommandLine()
        {
            return From(Environment.GetCommandLineArgs());
        }

        public static CommandLineArgs From(Dictionary<string, string> args)
        {
            return new CommandLineArgs
            {
                arguments = args
            };
        }

        public static CommandLineArgs From(IList<string> args)
        {
            return new CommandLineArgs
            {
                arguments = ParseCommandLineArgs(args)
            };
        }

        public bool Contains(string key)
        {
            return arguments.ContainsKey(key);
        }

        public T GetCommandLineValue<T>(string key, T defaultValue)
        {
            T value = defaultValue;
            TryGetCommandLineValue(key, ref value);
            return value;
        }

        public bool TryGetCommandLineValue<T>(string key, ref T value)
        {
            var desiredType = typeof(T);
            if (arguments.TryGetValue(key, out var strValue))
            {
                if (desiredType.IsEnum)
                {
                    try
                    {
                        value = (T) Enum.Parse(desiredType, strValue);
                        return true;
                    }
                    catch (Exception e)
                    {
                        throw new FormatException($"Unable to parse argument {strValue} as enum {desiredType.Name}.",
                            e);
                    }
                }

                value = (T) Convert.ChangeType(strValue, typeof(T));
                return true;
            }

            return false;
        }

        private static Dictionary<string, string> ParseCommandLineArgs(IList<string> args)
        {
            var config = new Dictionary<string, string>();
            for (var i = 0; i < args.Count; i++)
            {
                var flag = args[i];
                if (flag.StartsWith("+") || flag.StartsWith("-"))
                {
                    if (i + 1 >= args.Count)
                    {
                        throw new ArgumentException(
                            $"Flag \"{flag}\" requires an argument\nArguments: \"{string.Join(", ", args)}\"");
                    }

                    var flagArg = args[i + 1];
                    var strippedOfPlus = flag.Substring(1, flag.Length - 1);
                    config[strippedOfPlus] = flagArg;
                    // We've already processed the next argument, so skip it.
                    i++;
                }
            }

            return config;
        }
    }
}
