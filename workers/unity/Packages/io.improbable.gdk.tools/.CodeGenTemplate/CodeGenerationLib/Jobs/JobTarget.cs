using System;

namespace Improbable.Gdk.CodeGeneration.Jobs
{
    public class JobTarget
    {
        public readonly string FilePath;

        private CodeWriter.CodeWriter generatedContent;
        private readonly Func<CodeWriter.CodeWriter> generate;

        public JobTarget(string filePath, Func<CodeWriter.CodeWriter> generate)
        {
            FilePath = filePath;
            generatedContent = null;
            this.generate = generate;
        }

        public JobTarget(string filePath, Func<string> generate)
        {
            FilePath = filePath;
            generatedContent = null;
            this.generate = () => CodeWriter.CodeWriter.Raw(generate());
        }

        public void Generate()
        {
            generatedContent = generate();
        }

        public string Format()
        {
            return generatedContent.Format()
                .Replace("\r\n", "\n")
                .Replace("\n", Environment.NewLine);
        }
    }
}
