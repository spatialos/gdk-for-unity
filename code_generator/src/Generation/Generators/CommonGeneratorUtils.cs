namespace Improbable.Gdk.CodeGenerator
{
    public static class CommonGeneratorUtils
    {
        public static string GetGeneratedHeader()
        {
            return "// ===========\r\n// DO NOT EDIT - this file is automatically regenerated.\r\n// ===========";
        }

        public static string IndentEveryNewline(string input)
        {
            return input.Replace("\r\n", "\r\n    ");
        }
    }
}
