using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Mono.TextTemplating;

namespace Improbable.TextTemplating
{
    public class TransformAllTask : Task
    {
        [Required]
        public string[] Imports { get; set; }

        [Required]
        public ITaskItem[] InputFiles { get; set; }

        [Required]
        public ITaskItem ProjectDirectory { get; set; }

        [Required]
        public string ClassNameSpace { get; set; }

        [Output]
        public ITaskItem[] OutputFiles { get; set; }

        public override bool Execute()
        {
            if (InputFiles.Length < 1)
            {
                Log.LogError("No template files were specified in the InputFiles parameter.");
                return false;
            }

            string generatedPath = Path.Combine(ProjectDirectory.ItemSpec, "Generated");
            Log.LogMessage(MessageImportance.Normal, "Generated files will be output to directory: {0}", generatedPath);

            if (Directory.Exists(generatedPath))
            {
                Directory.Delete(generatedPath, true);
            }

            Directory.CreateDirectory(generatedPath);

            var source = new List<string>();
            var failed = false;
            foreach (ITaskItem inputFile in InputFiles)
            {
                var itemSpec = inputFile.ItemSpec;
                var withoutExtension = Path.GetFileNameWithoutExtension(itemSpec);
                var path2 = $"{withoutExtension}.cs";
                var outputFile = Path.Combine(generatedPath, path2);
                var templateGenerator = new TemplateGenerator();
                templateGenerator.Imports.AddRange(Imports);

                templateGenerator.PreprocessTemplate(itemSpec, withoutExtension, ClassNameSpace, outputFile, Encoding.UTF8,
                    out var language, out var references);

                failed |= templateGenerator.Errors.HasErrors;

                source.Add(outputFile);
                Log.LogMessage(MessageImportance.Normal, "Transformed template {0} into generator: {1}",
                    Path.GetFileName(itemSpec), path2);
            }

            OutputFiles = source
                .Select<string, TaskItem>(name => new TaskItem(name))
                .ToArray<TaskItem>();

            Log.LogMessage(MessageImportance.Normal, "Finished transforming template files.");
            return !failed;
        }
    }
}
