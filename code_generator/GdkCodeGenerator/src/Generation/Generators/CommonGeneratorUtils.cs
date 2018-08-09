using System; 

namespace Improbable.Gdk.CodeGenerator
{
    public static class CommonGeneratorUtils
    {
        private const int SpacesPerIndent = 4;

        public static string GetGeneratedHeader()
        {
            return "// ===========\r\n// DO NOT EDIT - this file is automatically regenerated.\r\n// ===========";
        }

        public static string IndentEveryNewline(string input)
        {
            return IndentEveryNewline(input, 1);
        }

        public static string IndentEveryNewline(string input, int numIndents)
        {
            var spaces = new String(' ', numIndents * SpacesPerIndent);
            return input.Replace("\r\n", $"\r\n{spaces}");
        }
    }
}
