using System;
using System.Collections.Generic;

namespace Improbable.Gdk.CodeGeneration.Utils
{
    public static class CommonGeneratorUtils
    {
        private const int SpacesPerIndent = 4;

        private static readonly Dictionary<int, string> IndentCache = new Dictionary<int, string>
        {
            { 0, string.Empty }
        };

        public static string GetGeneratedHeader()
        {
            return $"// ==========={System.Environment.NewLine}// DO NOT EDIT - this file is automatically " +
                $"regenerated.{System.Environment.NewLine}// ===========";
        }

        // TODO: remove once all TT files have been ported over
        public static string IndentEveryNewline(string input)
        {
            return IndentEveryNewline(input, 1);
        }

        // TODO: remove once all TT files have been ported over
        public static string IndentEveryNewline(string input, int numIndents)
        {
            var spaces = new String(' ', numIndents * SpacesPerIndent);
            return input.Replace($"{System.Environment.NewLine}", $"{System.Environment.NewLine}{spaces}");
        }

        public static string GetIndentSpaces(int indentLevel)
        {
            if (!IndentCache.TryGetValue(indentLevel, out var indentSpaces))
            {
                indentSpaces = new string(' ', indentLevel * SpacesPerIndent);
            }

            return indentSpaces;
        }
    }
}
