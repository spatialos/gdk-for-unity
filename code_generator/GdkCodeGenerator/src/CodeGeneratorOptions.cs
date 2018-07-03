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
        public string JsonDirectory { get; private set; }
        public string OutputDirectory { get; private set; }
        public bool ShouldShowHelp { get; private set; }
        public string HelpText { get; private set; }

        public static CodeGeneratorOptions ParseArguments(ICollection<string> args)
        {
            var options = new CodeGeneratorOptions();
            var optionSet = new OptionSet
            {
                {
                    "json-dir=", "REQUIRED: the directory containing the json representation of your schema",
                    j => options.JsonDirectory = j
                },
                {
                    "output-dir=", "REQUIRED: the directory to output generated components and structs to",
                    u => options.OutputDirectory = u
                },
                {
                    "h|help", "show help",
                    h => options.ShouldShowHelp = h != null
                }
            };

            optionSet.Parse(args);

            using (var sw = new StringWriter())
            {
                optionSet.WriteOptionDescriptions(sw);
                options.HelpText = sw.ToString();
            }


            return options;
        }
    }
}
