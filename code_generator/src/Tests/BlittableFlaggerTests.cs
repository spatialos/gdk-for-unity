using System.IO;
using System.Linq;
using Improbable.CodeGeneration.Model;
using NUnit.Framework;

namespace Improbable.Gdk.CodeGenerator.Tests
{
    [TestFixture]
    public class BlittableFlaggerTests
    {
        private string fakeFileName;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // This file needs to be created because UnitySchemaProcessor asserts that the SchemaFileRaw has a reference to a file that exists.
            fakeFileName = Path.GetTempFileName();
            File.WriteAllText(fakeFileName, "this is a fake file for testing");
        }

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            try
            {
                File.Delete(fakeFileName);
            }
            catch
            {
                // ignored
            }
        }

        [Test]
        public void MarksComponentsAsBlittableAccordingToReferencedTypes()
        {
            var processedSchemaFiles = new UnitySchemaProcessor(new[]
            {
                new SchemaFileRaw
                {
                    completePath = fakeFileName,
                    enumDefinitions = new EnumDefinitionRaw[0],
                    typeDefinitions = new[]
                    {
                        new TypeDefinitionRaw
                        {
                            qualifiedName = "usertypewithint32",
                            enumDefinitions = new EnumDefinitionRaw[0],
                            fieldDefinitions = new[]
                            {
                                new FieldDefinitionRaw
                                {
                                    singularType = new TypeReferenceRaw
                                    {
                                        builtInType = "int32"
                                    }
                                }
                            }
                        },
                        new TypeDefinitionRaw
                        {
                            qualifiedName = "usertypewithstring",
                            enumDefinitions = new EnumDefinitionRaw[0],
                            fieldDefinitions = new[]
                            {
                                new FieldDefinitionRaw
                                {
                                    singularType = new TypeReferenceRaw
                                    {
                                        builtInType = "string"
                                    }
                                }
                            }
                        }
                    },
                    componentDefinitions = new[]
                    {
                        new ComponentDefinitionRaw
                        {
                            name = "component-with-int",
                            dataDefinition = new TypeReferenceRaw
                            {
                                userType = "usertypewithint32"
                            }
                        },
                        new ComponentDefinitionRaw
                        {
                            name = "component-with-string",
                            dataDefinition = new TypeReferenceRaw
                            {
                                userType = "usertypewithstring"
                            }
                        }
                    }
                }
            }).ProcessedSchemaFiles;

            BlittableFlagger.SetBlittableFlags(processedSchemaFiles);

            Assert.IsTrue(
                processedSchemaFiles.First().ComponentDefinitions
                    .Find(definition => definition.Name == "component-with-int").IsBlittable,
                "Components with int types should be blittable");
            Assert.IsFalse(
                processedSchemaFiles.First().ComponentDefinitions
                    .Find(definition => definition.Name == "component-with-string").IsBlittable,
                "Components with string types should not be blittable");
        }
    }
}
