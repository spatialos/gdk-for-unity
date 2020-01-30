using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using Improbable.Gdk.CodeGeneration.Model;
using Improbable.Gdk.CodeGeneration.Model.Details;
using Improbable.Gdk.CodeGeneration.Utils;
using NLog;
using NLog.Layouts;
using NLog.Targets;

namespace Improbable.Gdk.CodeGenerator
{
    public class CodeGenerator
    {
        private readonly CodeGeneratorOptions options;
        private readonly IFileSystem fileSystem;

        private static Logger logger;

        public static int Main(string[] args)
        {
            try
            {
                var options = CodeGeneratorOptions.ParseArguments(args);
                var generator = new CodeGenerator(options, new MetaDataCompatibleFileSystem());

                return generator.Run();
            }
            catch (Exception e)
            {
                logger?.Error(e, "Code generation failed due to exception.");

                Console.Error.WriteLine("Code generation failed with exception: {0}", e);
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

            ConfigureLogger();
            logger = LogManager.GetCurrentClassLogger();
        }

        private static void ConfigureLogger()
        {
            var codeGeneratorOptions = CodeGeneratorOptions.Instance;

            var config = new NLog.Config.LoggingConfiguration();

            var minimumLogLevel = codeGeneratorOptions.Verbose ? LogLevel.Trace : LogLevel.Info;

            if (codeGeneratorOptions.EnableLoggingToStdout)
            {
                var consoleTarget = new ConsoleTarget("consoleTarget")
                {
                    Layout = new JsonLayout
                    {
                        Attributes =
                        {
                            new JsonAttribute("time", "${longdate}"),
                            new JsonAttribute("level", "${level:uppercase=true}"),
                            new JsonAttribute("logger", "${logger}"),
                            new JsonAttribute("message", "${message}"),
                            new JsonAttribute("exception", "${exception:format=ToString}")
                        }
                    }
                };
                config.AddTarget(consoleTarget);
                config.AddRule(minimumLogLevel, LogLevel.Fatal, consoleTarget);
            }

            var fileTarget = new FileTarget("fileTarget")
            {
                FileName = codeGeneratorOptions.AbsoluteLogPath,
                Layout = "${level:uppercase=true:padding=-5} | ${longdate} | ${logger} | ${message} ${exception:format=ToString}",
                DeleteOldFileOnStartup = true
            };
            config.AddTarget(fileTarget);
            config.AddRule(minimumLogLevel, LogLevel.Fatal, fileTarget);

            LogManager.Configuration = config;
        }

        public int Run()
        {
            if (options.ShouldShowHelp)
            {
                ShowHelpMessage();
                return 0;
            }

            var optionErrors = options.GetValidationErrors().ToList();
            foreach (var optionError in optionErrors)
            {
                Console.WriteLine(optionError);
            }

            if (optionErrors.Any())
            {
                ShowHelpMessage();
                return 1;
            }

            logger.Info("Starting code generation.");

            logger.Info("Gathering schema information.");
            var bundlePath = GenerateBundle();

            logger.Info("Loading schema bundle from json.");
            var schemaBundle = SchemaBundle.LoadBundle(File.ReadAllText(bundlePath));

            logger.Info("Setting up schema file tree.");
            var fileTree = new FileTree(options.SchemaInputDirs);

            logger.Info("Initialising DetailsStore.");
            var store = new DetailsStore(schemaBundle, options.SerializationOverrides, fileTree);

            logger.Info("Setting up code generation jobs.");
            var jobs = AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(assembly =>
                {
                    try
                    {
                        return assembly.GetTypes();
                    }
                    catch (ReflectionTypeLoadException e)
                    {
                        logger.Error(e, $"Failed to load assembly {assembly.FullName}.");
                        return Enumerable.Empty<Type>();
                    }
                })
                .Where(type => typeof(CodegenJob).IsAssignableFrom(type))
                .Where(type => !type.IsAbstract)
                .Where(type => !type.GetCustomAttributes(typeof(IgnoreCodegenJobAttribute)).Any())
                .Select(type =>
                {
                    logger.Info($"Creating instance of {type}.");
                    return (CodegenJob) Activator.CreateInstance(type, options.NativeOutputDirectory, fileSystem, store);
                })
                .ToArray();

            logger.Info("Calling JobRunner.");
            new JobRunner(fileSystem).Run(jobs);

            logger.Info("Finished code generation.");
            return 0;
        }

        private string GenerateBundle()
        {
            var inputPaths = new List<string>();
            foreach (var schemaDir in options.SchemaInputDirs)
            {
                if (!Directory.Exists(schemaDir))
                {
                    logger.Error($"Schema source directory not found at path \"{schemaDir}\".");
                    continue;
                }

                inputPaths.Add($"--schema_path=\"{schemaDir}\"");
            }

            logger.Info($"Preparing bundle output path: {options.JsonDirectory}.");
            SystemTools.EnsureDirectoryEmpty(options.JsonDirectory);

            var bundlePath = Path.Join(options.JsonDirectory, "bundle.json");

            var descriptorPath = Path.Join(options.DescriptorDirectory, "schema.descriptor");

            var arguments = new[]
            {
                "--load_all_schema_on_schema_path",
                $"--bundle_json_out=\"{bundlePath}\"",
                $"--descriptor_set_out=\"{descriptorPath}\""
            }.Union(inputPaths).ToList();

            logger.Info("Generating schema bundle and descriptor.");
            logger.Trace($"Calling '{options.SchemaCompilerPath} {string.Join(" ", arguments)}'.");

            SystemTools.RunRedirected(options.SchemaCompilerPath, arguments);

            logger.Info($"Generated bundle at {bundlePath}.");
            return bundlePath;
        }

        private void ShowHelpMessage()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine(options.HelpText);
        }
    }
}
