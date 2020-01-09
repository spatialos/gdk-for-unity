namespace Improbable.Gdk.CodeGeneration.CodeWriter
{
    /// <summary>
    /// A unit of code that can be returned as an indented string.
    /// </summary>
    public interface ICodeBlock
    {
        string Output(int indentLevel = 0);
    }
}
