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
        public string DescriptorDirectory { get; private set; }
        public string NativeOutputDirectory { get; private set; }
        public bool ShouldShowHelp { get; private set; }
        public bool Quiet { get; private set; }
        public bool EnableLoggingToStdout { get; private set; }
        public string AbsoluteLogPath { get; private set; }
        public string HelpText { get; private set; }
        public List<string> SchemaInputDirs { get; } = new List<string>();
        public string SchemaCompilerPath { get; private set; }
        public List<string> SerializationOverrides { get; } = new List<string>();

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
                    "descriptor-dir=", "REQUIRED: the directory that will contain the Schema Descriptor",
                    j => options.DescriptorDirectory = j
                },
                {
                    "native-output-dir=", "REQUIRED: the directory to output generated components and structs to",
                    u => options.NativeOutputDirectory = u
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
                    "logger-output-dir=", "REQUIRED: absolute path to logger output file",
                    p => options.AbsoluteLogPath = p
                },
                {
                    "h|help", "show help",
                    h => options.ShouldShowHelp = h != null
                },
                {
                    "q|quiet", "non-verbose logging",
                    q => options.Quiet = q != null
                },
                {
                    "o|enable-stdout", "enable logging to stdout",
                    o => options.EnableLoggingToStdout = o != null
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
            if (string.IsNullOrEmpty(NativeOutputDirectory))
            {
                yield return "Native output directory not specified";
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
