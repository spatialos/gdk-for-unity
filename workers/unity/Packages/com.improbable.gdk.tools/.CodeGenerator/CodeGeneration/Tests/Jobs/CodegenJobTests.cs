using System;
using System.Collections.Generic;
using System.Linq;
using CodeGeneration.Tests.FileHandling;
using Improbable.Gdk.CodeGeneration.FileHandling;
using Improbable.Gdk.CodeGeneration.Jobs;
using NUnit.Framework;

namespace CodeGeneration.Tests.Jobs
{
    [TestFixture]
    public class CodegenJobTests
    {
        [Test]
        public void MarkAsDirty_triggers_override()
        {
            var job = CodegenStub.GetCleanInstance();
            job.MarkAsDirty();

            Assert.IsTrue(job.IsDirty());
        }

        [Test]
        public void IsDirty_returns_true_for_0_input_files()
        {
            var job = CodegenStub.GetCleanInstance();
            job.AddOutputFile("my/filepath", DateTime.Now, true);

            Assert.IsTrue(job.IsDirty());
        }

        [Test]
        public void IsDirty_returns_true_for_0_output_files()
        {
            var job = CodegenStub.GetCleanInstance();
            job.AddInputFile("my/filepath", DateTime.Now);

            Assert.IsTrue(job.IsDirty());
        }

        [Test]
        public void IsDirty_returns_true_if_missing_output_files()
        {
            var job = CodegenStub.GetCleanInstance();
            job.AddInputFile("input/file.schema", DateTime.Now);
            job.AddOutputFile("output/test.cs", DateTime.Now, true);
            job.AddOutputFile("output/test2.cs", DateTime.Now, false);

            Assert.IsTrue(job.IsDirty());
        }

        [Test]
        public void IsDirty_returns_false_if_input_least_recently_changed()
        {
            var job = CodegenStub.GetCleanInstance();
            job.AddInputFile("input/file.schema", DateTime.Now);
            job.AddOutputFile("output/test.cs", DateTime.Now, true);
            job.AddOutputFile("output/test2.cs", DateTime.Now, true);

            Assert.IsFalse(job.IsDirty());
        }

        [Test]
        public void IsDirty_returns_true_if_input_most_recently_changed()
        {
            var job = CodegenStub.GetCleanInstance();
            job.AddOutputFile("output/test.cs", DateTime.Now, true);
            job.AddOutputFile("output/test2.cs", DateTime.Now, true);
            job.AddInputFile("input/file.schema", DateTime.Now);

            Assert.IsTrue(job.IsDirty());
        }

        [Test]
        public void Clean_deletes_all_output_files()
        {
            var job = CodegenStub.GetCleanInstance();
            job.AddOutputFile("output/test.cs", DateTime.Now, true);
            job.AddOutputFile("output/test2.cs", DateTime.Now, true);

            job.Clean();

            var files = job.myFileSystem.GetFilesInDirectory("output").Cast<MockFile>();

            foreach (var file in files)
            {
                Assert.IsTrue(file.WasDeleted);
            }
        }

        public class CodegenStub : CodegenJob
        {
            public static CodegenStub GetCleanInstance()
            {
                return new CodegenStub("", new MockFileSystem());
            }

            internal MockFileSystem myFileSystem;

            public CodegenStub(string outputDirectory, IFileSystem fileSystem) : base(outputDirectory, fileSystem)
            {
                InputFiles = new List<string>();
                OutputFiles = new List<string>();
                myFileSystem = (MockFileSystem) fileSystem;
            }

            protected override void RunImpl()
            {
                throw new NotImplementedException();
            }

            public void AddInputFile(string path, DateTime timestamp)
            {
                InputFiles.Add(path);
                myFileSystem.AddFile(path, timestamp);
            }

            public void AddOutputFile(string path, DateTime timestamp, bool shouldExist)
            {
                OutputFiles.Add(path);
                if (shouldExist)
                {
                    myFileSystem.AddFile(path, timestamp);
                }
            }
        }
    }
}
