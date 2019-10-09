using Improbable.Gdk.CodeGeneration.FileHandling;

namespace CodeGeneration.Tests.FileHandling
{
    public class MockFileTree : IFileTree
    {
        public string GetFullPathForRelativeSchema(string canonicalPath)
        {
            return canonicalPath;
        }
    }
}
