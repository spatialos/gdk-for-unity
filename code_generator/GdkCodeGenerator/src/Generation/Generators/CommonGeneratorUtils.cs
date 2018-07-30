namespace Improbable.Gdk.CodeGenerator
{
    public static class CommonGeneratorUtils
    {
        public static string GetGeneratedHeader()
        {
            return $"// ==========={System.Environment.NewLine}// DO NOT EDIT - this file is automatically " +
            $"regenerated.{System.Environment.NewLine}// ===========";
        }

        public static string IndentEveryNewline(string input)
        {
            return input.Replace($"{System.Environment.NewLine}", $"{System.Environment.NewLine}    ");
        }
    }
}
