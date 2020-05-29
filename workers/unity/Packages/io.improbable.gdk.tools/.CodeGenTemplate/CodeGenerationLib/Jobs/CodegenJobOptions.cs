namespace Improbable.Gdk.CodeGeneration.Jobs
{
    public readonly struct CodegenJobOptions
    {
        public readonly bool IsForce;
        public string OutputDir => isEditor ? editorOutputDir : outputDir;
        private readonly bool isEditor;
        private readonly string outputDir;
        private readonly string editorOutputDir;

        public CodegenJobOptions(string outputDir, string editorOutputDir, bool isForce) : this(outputDir, editorOutputDir, isForce, false)
        {
        }

        private CodegenJobOptions(string outputDir, string editorOutputDir, bool isForce, bool isEditor)
        {
            this.outputDir = outputDir;
            this.editorOutputDir = editorOutputDir;
            IsForce = isForce;
            this.isEditor = isEditor;
        }

        public CodegenJobOptions AsEditor()
        {
            return new CodegenJobOptions(outputDir, editorOutputDir, IsForce, true);
        }
    }
}
