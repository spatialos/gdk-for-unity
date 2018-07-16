using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace CopySchema.Tests
{
    [TestFixture]
    class ProgramTests
    {
        private string scratch;
        private string packages;
        private string manifest;
        private string schema;
        private string relativePackagePath;

        [SetUp]
        public void CreateTempFiles()
        {
            scratch = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString())).FullName;
            packages = Directory.CreateDirectory(Path.Combine(scratch, "my_packages")).FullName;
            schema = Directory.CreateDirectory(Path.Combine(scratch, "my_schema")).FullName;
            relativePackagePath = Directory.CreateDirectory(Path.Combine(scratch, "relative_package_path")).FullName;
            manifest = Path.Combine(packages, "manifest.json");
            File.WriteAllText(manifest, "");
        }

        [TearDown]
        public void DeleteTempFiles()
        {
            Directory.Delete(scratch, true);
        }

        [Test]
        public void ParseArguments_should_throw_unless_two_arguments_are_supplied()
        {
            try
            {
                Program.ParseArguments(new string[] { }, out var manifestFile, out var packagesRoot,
                    out var schemaRoot);
                Assert.Fail("Did not throw exception.");
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void ParseArguments_should_throw_if_manifest_file_does_not_exist()
        {
            try
            {
                Program.ParseArguments(new string[] { "MissingFile.json", "" }, out var manifestFile,
                    out var packagesRoot,
                    out var schemaRoot);
                Assert.Fail("Did not throw exception.");
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void ParseArguments_should_throw_if_schema_destination_does_not_exist()
        {
            try
            {
                Program.ParseArguments(new string[] { manifest, "MissingDirectory" }, out var manifestFile,
                    out var packagesRoot,
                    out var schemaRoot);
                Assert.Fail("Did not throw exception.");
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void ParseArguments_should_return_manifest_schemaRoot_packageRoot_from_args()
        {
            Program.ParseArguments(new string[] { manifest, schema }, out var manifestFile, out var packagesRoot, out var schemaRoot);

            Assert.AreEqual(manifest, manifestFile, "Manifest does not match");
            Assert.AreEqual(schema, schemaRoot, "schemaRoot does not match");
            Assert.AreEqual(packages, packagesRoot, "packagesRoot does not match");
        }

        [Test]
        public void CleanDestination_should_delete_existing_packages_destination()
        {
            Directory.CreateDirectory(Path.Combine(schema, Program.FromGdkPackagesDir, "nested"));
            File.WriteAllText(Path.Combine(schema, Path.GetRandomFileName()), string.Empty);
            File.WriteAllText(Path.Combine(schema, Program.FromGdkPackagesDir, "nested", Path.GetRandomFileName()), string.Empty);

            Program.CleanDestination(schema);

            Assert.False(Directory.Exists(Path.Combine(schema, Program.FromGdkPackagesDir)));
        }

        [Test]
        public void ParseManifest_throws_if_not_valid_json()
        {
            File.WriteAllText(manifest, "Not a json file");

            try
            {
                Program.ParseManifest(manifest);
                Assert.Fail("Did not throw exception");
            }
            catch (ArgumentException)
            {
            }
        }

        [Test]
        public void ParseManifest_returns_json_object()
        {
            File.WriteAllText(manifest, "{ \"foo\": \"bar\"}");

            dynamic json = Program.ParseManifest(manifest);

            Assert.AreEqual("bar", json.foo.ToString(), "Json not parsed correctly.");
        }

        [Test]
        public void GetPackageInfos_returns_only_file_dependencies()
        {
            dynamic json = JsonConvert.DeserializeObject(@"
                {
                    ""dependencies"": {
                        ""with_version"":""1.0.0"",
                        ""dep.one"": ""file:./path_of/dep_one"",
                        ""dep.two"": ""file:com.dep.two.path"",
                        ""dep.three"": ""file:../relative_package_path""
                    }
                }
            ");
            var depOne = new Program.PackageInfo("dep.one", Path.Combine(packages, "path_of", "dep_one"));
            var depTwo = new Program.PackageInfo("dep.two", Path.Combine(packages, "com.dep.two.path"));
            var depThree = new Program.PackageInfo("dep.three", relativePackagePath);

            List<Program.PackageInfo> results = Program.GetPackageInfos(json, packages);

            Assert.AreEqual(3, results.Count, "Wrong number of dependencies returned.");
            Assert.Contains(depOne, results);
            Assert.Contains(depTwo, results);
            Assert.Contains(depThree, results);
        }

        [Test]
        public void FindSchemaFiles_returns_all_found_schema_files()
        {
            var depOne = new Program.PackageInfo("dep.one", Path.Combine(packages, "path_of", "dep_one"));
            var depTwo = new Program.PackageInfo("dep.two", Path.Combine(packages, "com.dep.two.path"));
            var depThree = new Program.PackageInfo("dep.three", relativePackagePath);

            Directory.CreateDirectory(Path.Combine(packages, "path_of", "dep_one", "Schema"));
            Directory.CreateDirectory(Path.Combine(relativePackagePath, "Schema", "nested"));

            var fileOne = Path.Combine(packages, "path_of", "dep_one", "Schema", "schema_file.schema");
            var fileTwo = Path.Combine(relativePackagePath, "Schema", "other.schema");
            var fileThree = Path.Combine(relativePackagePath, "Schema", "nested", "nested.schema");

            File.WriteAllText(fileOne, string.Empty);
            File.WriteAllText(Path.Combine(packages, "path_of", "dep_one", "Schema", "schema_file.schema.meta"), string.Empty);
            File.WriteAllText(fileTwo, string.Empty);
            File.WriteAllText(fileThree, string.Empty);

            var packageInfos = new List<Program.PackageInfo>()
            {
                depOne,
                depTwo,
                depThree
            };

            var results = Program.FindSchemaFiles(packageInfos);

            Assert.AreEqual(3, results.Count, "Wrong number of files returned.");
            Assert.Contains(new KeyValuePair<Program.PackageInfo, string>(depOne, fileOne), results);
            Assert.Contains(new KeyValuePair<Program.PackageInfo, string>(depThree, fileTwo), results);
            Assert.Contains(new KeyValuePair<Program.PackageInfo, string>(depThree, fileThree), results);
        }

        [Test]
        public void GetFilesToCopy_returns_destinations_nested_in_package_name()
        {
            var depOne = new Program.PackageInfo("dep.one", Path.Combine(packages, "path_of", "dep_one"));
            var depThree = new Program.PackageInfo("dep.three", relativePackagePath);

            var fileOne = Path.Combine(packages, "path_of", "dep_one", "Schema", "schema_file.schema");
            var fileOneDestination = Path.Combine(schema, Program.FromGdkPackagesDir, depOne.Name, "schema_file.schema");
            var fileTwo = Path.Combine(relativePackagePath, "Schema", "other.schema");
            var fileTwoDestination = Path.Combine(schema, Program.FromGdkPackagesDir, depThree.Name, "other.schema");
            var fileThree = Path.Combine(relativePackagePath, "Schema", "nested", "nested.schema");
            var fileThreeDestination = Path.Combine(schema, Program.FromGdkPackagesDir, depThree.Name, "nested", "nested.schema");

            var schemaFiles = new List<KeyValuePair<Program.PackageInfo, string>>()
            {
                new KeyValuePair<Program.PackageInfo, string>(depOne, fileOne),
                new KeyValuePair<Program.PackageInfo, string>(depThree, fileTwo),
                new KeyValuePair<Program.PackageInfo, string>(depThree, fileThree)
            };

            var results = Program.GetFilesToCopy(schemaFiles, schema);

            Assert.AreEqual(3, results.Count, "Wrong number of files returned");
            Assert.Contains(new KeyValuePair<string, string>(fileOne, fileOneDestination), results);
            Assert.Contains(new KeyValuePair<string, string>(fileTwo, fileTwoDestination), results);
            Assert.Contains(new KeyValuePair<string, string>(fileThree, fileThreeDestination), results);
        }

        [Test]
        public void CopyFiles_should_copy_from_key_to_value()
        {
            var fileOne = Path.Combine(packages, "package_root", "file_one.schema");
            var fileOneDest = Path.Combine(schema, "package.name", "file_one.schema");
            Directory.CreateDirectory(Path.GetDirectoryName(fileOne));
            File.WriteAllText(fileOne, "This is file one");
            var fileTwo = Path.Combine(packages, "other_package.root", "nested", "file_one.schema");
            var fileTwoDest = Path.Combine(schema, "other.package.name", "nested", "file_one.schema");
            Directory.CreateDirectory(Path.GetDirectoryName(fileTwo));
            File.WriteAllText(fileTwo, "Second file contents");

            var filesCopied = Program.CopyFiles(new Dictionary<string, string>()
            {
                { fileOne, fileOneDest },
                { fileTwo, fileTwoDest }
            });

            Assert.AreEqual(2, filesCopied);
            Assert.AreEqual("This is file one", File.ReadAllText(fileOneDest));
            Assert.AreEqual("Second file contents", File.ReadAllText(fileTwoDest));
        }
    }
}
