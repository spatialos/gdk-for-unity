using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Improbable.CodeGeneration.FileHandling;
using System.Runtime.InteropServices;

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

            GenerateNativeTypesAndAst();

            var schemaFilesRaw = SchemaFiles.GetSchemaFilesRaw(options.JsonDirectory, fileSystem).ToList();
            var schemaProcessor = new UnitySchemaProcessor(schemaFilesRaw);
            var globalEnumSet = ExtractEnums(schemaProcessor.ProcessedSchemaFiles);

            foreach (var processedSchema in schemaProcessor.ProcessedSchemaFiles)
            {
                var job = new SingleGenerationJob(options.NativeOutputDirectory, processedSchema, fileSystem,
                    globalEnumSet);
                job.Run();
            }

            return 0;
        }

        private void GenerateNativeTypesAndAst()
        {
            var files = options.SchemaInputDirs.SelectMany(dir =>
                Directory.GetFiles(dir, "*.schema", SearchOption.AllDirectories));
            var inputPaths = options.SchemaInputDirs.Select(dir => $"--schema_path={dir}");

            SystemTools.EnsureDirectoryEmpty(options.JsonDirectory);
            SystemTools.EnsureDirectoryEmpty(options.NetworkTypesOutputDirectory);
            SystemTools.EnsureDirectoryEmpty(options.NativeOutputDirectory);

            // Add all of the files we found to the command line.
            var arguments = new[]
            {
                $@"--csharp_out={options.NetworkTypesOutputDirectory}",
                $@"--ast_json_out={options.JsonDirectory}"
            }.Union(inputPaths).Union(files).ToList();

            SystemTools.RunRedirected(options.SchemaCompiler, arguments);
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

                foreach (var typeDefinition in schema.TypeDefinitions)
                {
                    ExtractEnums(typeDefinition, enumSet);
                }
            }

            return enumSet;
        }

        private void ExtractEnums(UnityTypeDefinition typeDefinition, HashSet<string> enumSet)
        {
            foreach (var enumDefinition in typeDefinition.EnumDefinitions)
            {
                enumSet.Add(enumDefinition.qualifiedName);
            }

            foreach (var nestedTypeDefinition in typeDefinition.TypeDefinitions)
            {
                ExtractEnums(nestedTypeDefinition, enumSet);
            }
        }

        private void ShowHelpMessage()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine(options.HelpText);
        }

        private bool ValidateOptions()
        {
            if (options.NativeOutputDirectory == null)
            {
                Console.WriteLine("Native output directory not specified");
                return false;
            }

            if (options.NetworkTypesOutputDirectory == null)
            {
                Console.WriteLine("Network types output directory not specified");
                return false;
            }

            if (options.SchemaInputDirs == null || options.SchemaInputDirs.Count == 0)
            {
                Console.WriteLine("Schema input directories not specified");
                return false;
            }

            return true;
        }
    }
}
