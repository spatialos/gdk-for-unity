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
            if (this.InputFiles.Length < 1)
            {
                this.Log.LogMessage(MessageImportance.Normal,
                    "No template files were specified in the InputFiles parameter.", new object[0]);
                return true;
            }

            string str = Path.Combine(this.ProjectDirectory.ItemSpec, "Generated");
            this.Log.LogMessage(MessageImportance.Normal, "Generated files will be output to directory: {0}",
                (object) str);
            if (Directory.Exists(str))
                Directory.Delete(str, true);
            Directory.CreateDirectory(str);
            List<string> source = new List<string>();
            foreach (ITaskItem inputFile in this.InputFiles)
            {
                string itemSpec = inputFile.ItemSpec;
                string withoutExtension = Path.GetFileNameWithoutExtension(itemSpec);
                string path2 = withoutExtension + ".cs";
                string outputFile = Path.Combine(str, path2);
                TemplateGenerator templateGenerator = new TemplateGenerator();
                Encoding utF8 = Encoding.UTF8;
                templateGenerator.Imports.AddRange((IEnumerable<string>) this.Imports);
                string language;
                string[] references;
                templateGenerator.PreprocessTemplate(itemSpec, withoutExtension, this.ClassNameSpace, outputFile, utF8,
                    out language, out references);
                source.Add(outputFile);
                this.Log.LogMessage(MessageImportance.Normal, "Transformed template {0} into generator: {1}",
                    (object) Path.GetFileName(itemSpec), (object) path2);
            }

            this.OutputFiles = (ITaskItem[]) source
                .Select<string, TaskItem>((Func<string, TaskItem>) (name => new TaskItem(name))).ToArray<TaskItem>();
            this.Log.LogMessage(MessageImportance.Normal, "Finished transforming template files.", new object[0]);
            return true;
        }
    }
}
