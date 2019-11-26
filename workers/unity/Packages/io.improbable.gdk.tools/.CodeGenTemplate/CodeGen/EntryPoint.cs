using System;
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
                logger.Error(e, "Code generation failed due to exception");

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
            var codeGeneratorOptions =  CodeGeneratorOptions.Instance;

            var config = new NLog.Config.LoggingConfiguration();

            var jsonLayout = new JsonLayout
            {
                Attributes =
                {
                    new JsonAttribute("time", "${longdate}"),
                    new JsonAttribute("level", "${level:uppercase=true}"),
                    new JsonAttribute("logger", "${logger}"),
                    new JsonAttribute("message", "${message}"),
                    new JsonAttribute("exception", "${exception:format=ToString}")
                }
            };

            var minimumLogLevel = codeGeneratorOptions.DisableVerboseLogging ? LogLevel.Info : LogLevel.Trace;

            if (codeGeneratorOptions.EnableLoggingToConsole)
            {
                var consoleTarget = new ConsoleTarget("consoleTarget")
                {
                    Layout = jsonLayout
                };
                config.AddTarget(consoleTarget);
                config.AddRule(minimumLogLevel, LogLevel.Fatal, consoleTarget);
            }

            var fileTarget = new FileTarget("fileTarget")
            {
                FileName = codeGeneratorOptions.AbsoluteLogPath,
                Layout = jsonLayout,
                DeleteOldFileOnStartup = true
            };
            config.AddTarget(fileTarget);
            config.AddRule(minimumLogLevel, LogLevel.Fatal, fileTarget);

            LogManager.Configuration = config;
        }

        public int Run()
        {
            logger.Info("Starting code generation");
            if (options.ShouldShowHelp)
            {
                ShowHelpMessage();
                return 0;
            }

            logger.Info("Validating options");
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

            logger.Info("Gathering schema information");
            var bundlePath = GenerateBundle();

            logger.Info("Loading schema bundle from json");
            var schemaBundle = SchemaBundle.LoadBundle(File.ReadAllText(bundlePath));

            logger.Info("Setting up schema file tree");
            var fileTree = new FileTree(options.SchemaInputDirs);

            logger.Info("Initialising DetailsStore");
            var store = new DetailsStore(schemaBundle, options.SerializationOverrides, fileTree);

            logger.Info("Setting up code generation jobs");
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
                        logger.Error($"Failed to load assembly {assembly.FullName} with error {e}");
                        return Enumerable.Empty<Type>();
                    }
                })
                .Where(type => typeof(CodegenJob).IsAssignableFrom(type))
                .Where(type => !type.IsAbstract)
                .Where(type => !type.GetCustomAttributes(typeof(IgnoreCodegenJobAttribute)).Any())
                .Select(type => (CodegenJob) Activator.CreateInstance(type, options.NativeOutputDirectory, fileSystem, store))
                .ToArray();

            logger.Info("Calling JobRunner");
            new JobRunner(fileSystem).Run(jobs);

            logger.Info("Finished code generation");
            return 0;
        }

        private string GenerateBundle()
        {
            var inputPaths = options.SchemaInputDirs.Select(dir => $"--schema_path=\"{dir}\"");

            logger.Info("Preparing bundle output path");
            SystemTools.EnsureDirectoryEmpty(options.JsonDirectory);

            var bundlePath = Path.Join(options.JsonDirectory, "bundle.json");

            var descriptorPath = Path.Join(options.DescriptorDirectory, "schema.descriptor");

            var arguments = new[]
            {
                "--load_all_schema_on_schema_path",
                $"--bundle_json_out=\"{bundlePath}\"",
                $"--descriptor_set_out=\"{descriptorPath}\""
            }.Union(inputPaths).ToList();

            logger.Info("Generating schema bundle and descriptor");
            SystemTools.RunRedirected(options.SchemaCompilerPath, arguments);

            return bundlePath;
        }

        private void ShowHelpMessage()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine(options.HelpText);
        }
    }
}
