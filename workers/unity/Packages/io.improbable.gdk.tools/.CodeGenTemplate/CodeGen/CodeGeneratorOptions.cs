using System.Collections.Generic;
using System.IO;
using Mono.Options;

namespace Improbable.Gdk.CodeGenerator
{
    /// <summary>
    ///     Runtime options for the CodeGenerator.
    /// </summary>
    public class CodeGeneratorOptions
    {
        public static CodeGeneratorOptions Instance { get; private set; }
        public string WorkerJsonDirectory { get; private set; }
        public string JsonDirectory { get; private set; }
        public string BundleDirectory { get; private set; }
        public string OutputDirectory { get; private set; }

        public string EditorOutputDirectory { get; private set; }
        public bool ShouldShowHelp { get; private set; }
        public bool Verbose { get; private set; } = false;
        public bool EnableLoggingToStdout { get; private set; } = true;
        public string AbsoluteLogPath { get; private set; }
        public string HelpText { get; private set; }
        public List<string> SchemaInputDirs { get; } = new List<string>();
        public string SchemaCompilerPath { get; private set; }
        public List<string> SerializationOverrides { get; } = new List<string>();
        public bool Force { get; private set; }

        public static CodeGeneratorOptions ParseArguments(ICollection<string> args)
        {
            var options = new CodeGeneratorOptions();
            var optionSet = new OptionSet
            {
                {
                    "worker-json-dir=", "REQUIRED: the directory that will contain the JSON representation of your workers",
                    j => options.WorkerJsonDirectory = j
                },
                {
                    "json-dir=", "REQUIRED: the directory that will contain the JSON representation of your schema",
                    j => options.JsonDirectory = j
                },
                {
                    "bundle-out-dir=", "REQUIRED: the directory that will contain the schema bundle",
                    j => options.BundleDirectory = j
                },
                {
                    "output-dir=", "REQUIRED: the directory to output generated components and structs to",
                    u => options.OutputDirectory = u
                },
                {
                    "editor-output-dir=", "REQUIRED: the directory to output generated editor code to",
                    u => options.EditorOutputDirectory = u
                },
                {
                    "schema-path=", "REQUIRED: a comma-separated list of directories that contain schema files",
                    u => options.SchemaInputDirs.Add(u)
                },
                {
                    "schema-compiler-path=", "REQUIRED: the path to the CoreSdk schema compiler",
                    u => options.SchemaCompilerPath = u
                },
                {
                    "serialization-override=", "OPTIONAL: defines an override for serialization of a single type",
                    u => options.SerializationOverrides.Add(u)
                },
                {
                    "log-file=", "REQUIRED: absolute path to logger output file",
                    p => options.AbsoluteLogPath = p
                },
                {
                    "h|help", "Show help",
                    h => options.ShouldShowHelp = h != null
                },
                {
                    "v|verbose", "Enable verbose logging",
                    q => options.Verbose = q != null
                },
                {
                    "disable-stdout", "Disable logging to stdout",
                    d => options.EnableLoggingToStdout = d == null
                },
                {
                    "f|force", "Force generating code.",
                    d => options.Force = true
                },
            };

            optionSet.Parse(args);

            using (var sw = new StringWriter())
            {
                optionSet.WriteOptionDescriptions(sw);
                options.HelpText = sw.ToString();
            }

            Instance = options;

            return options;
        }

        public IEnumerable<string> GetValidationErrors()
        {
            if (string.IsNullOrEmpty(OutputDirectory))
            {
                yield return "Output directory not specified";
            }

            if (string.IsNullOrEmpty(EditorOutputDirectory))
            {
                yield return "Editor output directory not specified";
            }

            if (SchemaInputDirs == null || SchemaInputDirs.Count == 0)
            {
                yield return "Schema input directories not specified";
            }

            if (string.IsNullOrEmpty(SchemaCompilerPath))
            {
                yield return "Schema compiler location not specified";
            }

            if (!File.Exists(SchemaCompilerPath))
            {
                yield return $"Schema compiler does not exist at '{SchemaCompilerPath}'";
            }
        }
    }
}
