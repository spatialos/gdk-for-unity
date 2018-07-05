using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CopySchema
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine("Expected 2 arguments, path to a manifest.json file, path to schema directory");
                Environment.Exit(1);
            }

            string manifestFile = args[0];
            string schemaDirectory = args[1];

            if (!File.Exists(manifestFile))
            {
                Console.Error.WriteLine($"manifest ${manifestFile} does not exist!");
                Environment.Exit(1);
            }

            string packagesDirectory = Path.GetDirectoryName(manifestFile);

            if (!Directory.Exists(schemaDirectory))
            {
                Console.Error.WriteLine($"Schema directory: {schemaDirectory} does not exist!");
                Environment.Exit(1);
            }

            CleanDestination(schemaDirectory);

            using (StreamReader file = File.OpenText(manifestFile))
            {
                dynamic manifest = JsonConvert.DeserializeObject(file.ReadToEnd());

                JObject dependencies = manifest.dependencies;

                foreach (var dep in dependencies.Properties())
                {
                    Console.WriteLine($"Found {dep.Name} -> {dep.Value}");

                    if (dep.Value.ToString().StartsWith("file:"))
                    {
                        var packagepath = Path.Combine(packagesDirectory, dep.Value.ToString().Replace("file:", ""));
                        CopySchema(dep.Name, packagepath, schemaDirectory);
                    }
                }
            }
        }

        static void CleanDestination(string schemaDirectory)
        {
            var destination = Path.Combine(schemaDirectory, "from_gdk_packages");
            if (Directory.Exists(destination))
            {
                Directory.Delete(destination, true);
            }
        }

        static void CopySchema(string packageName, string packageLocation, string schemaDirectory)
        {
            var source = Path.Combine(packageLocation, "Schema");
            var destination = Path.Combine(schemaDirectory, "from_gdk_packages", packageName);

            if (!Directory.Exists(source))
            {
                Console.WriteLine($"{source} not found, skipping");
                return;
            }
            
            if (!Directory.Exists(destination))
            {
                Console.WriteLine($"Creating {destination}");
                Directory.CreateDirectory(destination);
            }

            foreach (var file in System.IO.Directory.GetFiles(source, "*.schema", SearchOption.AllDirectories))
            {
                var fileDestination = destination + file.Replace(source, "");
                var fileDestinationDirectory = System.IO.Path.GetDirectoryName(fileDestination);
                if (!Directory.Exists(fileDestinationDirectory))
                {
                    Directory.CreateDirectory(fileDestinationDirectory);
                }

                File.Copy(file, fileDestination);
                Console.WriteLine($"copy: {file} -> {fileDestination}");
            }
        }
    }
}
