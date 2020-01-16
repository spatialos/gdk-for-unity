namespace Improbable.Gdk.CodeGeneration.CodeWriter
{
    /// <summary>
    /// A unit of code that can be returned as an indented string.
    /// </summary>
    public interface ICodeBlock
    {
        string Format(int indentLevel = 0);
    }
}
