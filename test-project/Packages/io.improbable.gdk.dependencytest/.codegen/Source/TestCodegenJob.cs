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

        public TestCodegenJob(CodegenJobOptions options, IFileSystem fileSystem, DetailsStore detailsStore)
            : base(options, fileSystem, detailsStore)
        {
            AddJobTarget(relativeOutputPath, () => TestContent);
            AddJobTarget(relativeTemplateOutputPath, () => ModularCodegenTestGenerator.Generate());
        }
    }
}
