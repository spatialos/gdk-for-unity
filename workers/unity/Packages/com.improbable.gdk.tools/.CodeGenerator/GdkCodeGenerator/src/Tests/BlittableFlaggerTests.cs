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

        private UnityComponentDefinition BlittableComponentDefinition;
        private UnityComponentDefinition NonBlittableComponentDefinition;

        private UnityFieldDefinition BlittableFieldDefinition;
        private UnityFieldDefinition NonBlittableFieldDefinition;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            // This file needs to be created because UnitySchemaProcessor asserts that the SchemaFileRaw has a reference to a file that exists.
            fakeFileName = Path.GetTempFileName();
            File.WriteAllText(fakeFileName, "this is a fake file for testing");

            BuildTestProcessedSchemaFiles();
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
        public void Components_should_have_correct_blittable_state()
        {
            Assert.IsTrue(BlittableComponentDefinition.IsBlittable,
                "Components with int types should be blittable");

            Assert.IsFalse(NonBlittableComponentDefinition.IsBlittable,
                "Components with string types should not be blittable");
        }

        [Test]
        public void Fields_should_have_correct_blittable_state()
        {
            Assert.IsTrue(BlittableFieldDefinition.IsBlittable,
                "Field with int type should be blittable");

            Assert.IsFalse(NonBlittableFieldDefinition.IsBlittable,
                "Field with string type should be non-blittable");
        }

        private void BuildTestProcessedSchemaFiles()
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

            BlittableComponentDefinition = processedSchemaFiles.First().ComponentDefinitions
                .Find(definition => definition.Name == "component-with-int");
            NonBlittableComponentDefinition = processedSchemaFiles.First().ComponentDefinitions
                .Find(definition => definition.Name == "component-with-string");

            BlittableFieldDefinition = BlittableComponentDefinition.DataDefinition.typeDefinition.FieldDefinitions
                .First();
            NonBlittableFieldDefinition = NonBlittableComponentDefinition.DataDefinition.typeDefinition.FieldDefinitions
                .First();
        }
    }
}
