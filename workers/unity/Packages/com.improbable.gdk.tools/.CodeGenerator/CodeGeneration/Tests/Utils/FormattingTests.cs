using Improbable.Gdk.CodeGeneration.Utils;
using NUnit.Framework;

namespace Improbable.Gdk.CodeGeneration.Tests.Utils
{
    [TestFixture]
    public class FormattingTests
    {
        private const string PascalCase = "TestPascalCase";
        private const string CamelCase = "testPascalCase";
        private const string SnakeCase = "test_pascal_case";

        private const string QualifiedName = "improbable.gdk.tests.nonblittable_types.NonBlittableComponent.second_event";

        private const string QualifiedPascalCase =
            "Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondEvent";

        private const string FullyQualifiedPascalCase =
            "global::Improbable.Gdk.Tests.NonblittableTypes.NonBlittableComponent.SecondEvent";

        [Test]
        public void PascalCaseToCamelCase_should_uncapitalize_first_letter()
        {
            // Normal case
            Assert.AreEqual(CamelCase, Formatting.PascalCaseToCamelCase(PascalCase));

            // Special case - single letter.
            const string UppercaseLetter = "T";
            const string LowercaseLetter = "t";
            Assert.AreEqual(LowercaseLetter, Formatting.PascalCaseToCamelCase(UppercaseLetter));

            // Special case - empty string
            Assert.AreEqual(string.Empty, Formatting.PascalCaseToCamelCase(string.Empty));
        }

        [Test]
        public void SnakeCaseToPascalCase_should_convert_snake_case_to_pascal_case()
        {
            Assert.AreEqual(PascalCase, Formatting.SnakeCaseToPascalCase(SnakeCase));
        }

        [Test]
        public void QualifiedNameToPascalCase_should_convert_qualified_snake_case_to_qualified_pascal_case()
        {
            Assert.AreEqual(QualifiedPascalCase, Formatting.QualifiedNameToPascalCase(QualifiedName));
        }

        [Test]
        public void FullyQualify_should_fully_qualify_a_name()
        {
            Assert.AreEqual(FullyQualifiedPascalCase, Formatting.FullyQualify(QualifiedName));
        }
    }
}
