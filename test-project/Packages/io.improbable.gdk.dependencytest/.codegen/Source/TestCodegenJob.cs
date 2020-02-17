using System.IO;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model.Details;

namespace Improbable.Gdk.CodeGenerator
{
    public class TestCodegenJob : CodegenJob
    {
        private readonly string relativeOutputPath = Path.Combine("improbable", "modular-codegen-tests", "Test.cs");
        private readonly string relativeTemplateOutputPath = Path.Combine("improbable", "modular-codegen-tests", "TemplateTest.cs");

        private const string TestContent = @"
namespace Improbable.Gdk.ModularCodegenTests
{
    public class Test
    {

    }
}
";

        public TestCodegenJob(string baseOutputDir, IFileSystem fileSystem, DetailsStore detailsStore, bool force)
            : base(baseOutputDir, fileSystem, detailsStore, force)
        {
            AddOutputFile(relativeOutputPath);
            AddOutputFile(relativeTemplateOutputPath);
        }

        protected override void RunImpl()
        {
            AddContent(relativeOutputPath, TestContent);
            AddContent(relativeTemplateOutputPath, ModularCodegenTestGenerator.Generate().Format());
        }
    }
}
