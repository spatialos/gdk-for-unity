using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.CodeGeneration.FileHandling;

namespace Improbable.Gdk.CodeGenerator
{
    public class CodeGenerator
    {
        private readonly CodeGeneratorOptions options;
        private readonly IFileSystem fileSystem;

        public static int Main(string[] args)
        {
            try
            {
                var options = CodeGeneratorOptions.ParseArguments(args);
                var generator = new CodeGenerator(options, new FileSystem());

                return generator.Run();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Code generation failed with exception: {0}", e.Message);
                if (e.InnerException != null)
                {
                    Console.Error.WriteLine(e.InnerException);
                }

                Console.Error.WriteLine(e.StackTrace);
            }

            return 1;
        }

        public CodeGenerator(CodeGeneratorOptions options, IFileSystem fileSystem)
        {
            this.options = options;
            this.fileSystem = fileSystem;
        }

        public int Run()
        {
            if (options.ShouldShowHelp)
            {
                ShowHelpMessage();
                return 0;
            }

            if (!ValidateOptions())
            {
                ShowHelpMessage();
                return 1;
            }

            CleanTargetDirectory();

            var schemaFilesRaw = SchemaFiles.GetSchemaFilesRaw(options.JsonDirectory, fileSystem).ToList();
            var schemaProcessor = new UnitySchemaProcessor(schemaFilesRaw);
            var globalEnumSet = ExtractEnums(schemaProcessor.ProcessedSchemaFiles);

            foreach (var processedSchema in schemaProcessor.ProcessedSchemaFiles)
            {
                var job = new SingleGenerationJob(options.OutputDirectory, processedSchema, fileSystem, globalEnumSet);
                job.Run();
            }

            return 0;
        }


        private HashSet<string> ExtractEnums(ICollection<UnitySchemaFile> schemas)
        {
            var enumSet = new HashSet<string>();
            foreach (var schema in schemas)
            {
                foreach (var unityEnum in schema.EnumDefinitions)
                {
                    enumSet.Add(unityEnum.qualifiedName);
                }
            }

            return enumSet;
        }

        private void CleanTargetDirectory()
        {
            if (!fileSystem.DirectoryExists(options.OutputDirectory))
            {
                return;
            }

            Directory.GetDirectories(options.OutputDirectory).ToList().ForEach(dir => fileSystem.DeleteDirectory(dir));
        }

        private void ShowHelpMessage()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine(options.HelpText);
        }

        private bool ValidateOptions()
        {
            if (options.JsonDirectory == null)
            {
                Console.WriteLine("Input directory not specified.");
                return false;
            }

            if (!fileSystem.DirectoryExists(options.JsonDirectory))
            {
                Console.WriteLine("The provided input directory does not exist: {0}", options.JsonDirectory);
                return false;
            }

            if (options.OutputDirectory == null)
            {
                Console.WriteLine("Output directory not specified");
                return false;
            }

            return true;
        }
    }
}
