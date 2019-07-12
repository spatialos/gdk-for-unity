using System;

namespace Improbable.Gdk.CodeGenerator
{
    public static class CommonGeneratorUtils
    {
        private const int SpacesPerIndent = 4;

        public static string GetGeneratedHeader()
        {
            return $"// ==========={System.Environment.NewLine}// DO NOT EDIT - this file is automatically " +
                $"regenerated.{System.Environment.NewLine}// ===========";
        }

        public static string IndentEveryNewline(string input)
        {
            return IndentEveryNewline(input, 1);
        }

        public static string IndentEveryNewline(string input, int numIndents)
        {
            var spaces = new String(' ', numIndents * SpacesPerIndent);
            return input.Replace($"{System.Environment.NewLine}", $"{System.Environment.NewLine}{spaces}");
        }
    }
}
