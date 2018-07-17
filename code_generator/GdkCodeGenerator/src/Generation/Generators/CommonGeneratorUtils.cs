namespace Improbable.Gdk.CodeGenerator
{
    public static class CommonGeneratorUtils
    {
        public static string GetGeneratedHeader()
        {
            return "// ===========\n// DO NOT EDIT - this file is automatically regenerated.\n// ===========";
        }

        public static string IndentEveryNewline(string input)
        {
            return input.Replace("\n", "\n    ");
        }
    }
}
