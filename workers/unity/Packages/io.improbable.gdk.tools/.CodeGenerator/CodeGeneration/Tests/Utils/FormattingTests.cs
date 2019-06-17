using System.Collections.Generic;
using System.Runtime.InteropServices;
using Improbable.Gdk.CodeGeneration.Utils;
using NUnit.Framework;

namespace CodeGeneration.Tests.Utils
{
    [TestFixture]
    public class FormattingTests
    {
        [TestCase("", "")]
        [TestCase("single", "Single")]
        [TestCase("some.snake_case", "Some.SnakeCase")]
        [TestCase("some.camelCase", "Some.CamelCase")]
        [TestCase("some.PascalCase", "Some.PascalCase")]
        [TestCase("lots_of.Entries.withDifferent.caps", "LotsOf.Entries.WithDifferent.Caps")]
        [TestCase("trailing.period.", "Trailing.Period")]
        [TestCase(".leading.period", "Leading.Period")]
        public void CapitaliseQualifiedNameParts_has_correct_behaviour(string input, string expectedOutput)
        {
            var output = Formatting.CapitaliseQualifiedNameParts(input);
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase("", "")]
        [TestCase("single", "Single")]
        [TestCase("with_snake_elements", "WithSnakeElements")]
        [TestCase("trailing_snake_", "TrailingSnake")]
        [TestCase("_leading_snake", "LeadingSnake")]
        public void SnakeCaseToPascalCase_has_correct_behaviour(string input, string expectedOutput)
        {
            var output = Formatting.SnakeCaseToPascalCase(input);
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase("", "")]
        [TestCase("single", "single")]
        [TestCase("some.snake_case", "some\\snakecase")]
        [TestCase("some.camelCase", "some\\camelcase")]
        [TestCase("some.PascalCase", "some\\pascalcase")]
        [TestCase("lots_of.Entries.withDifferent.caps", "lotsof\\entries\\withdifferent\\caps")]
        [TestCase("trailing.period.", "trailing\\period")]
        [TestCase(".leading.period", "leading\\period")]
        public void GetNamespacePath_has_correct_behaviour(string input, string expectedOutput)
        {
            // Platform dependent paths.
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                expectedOutput = expectedOutput.Replace("\\", "/");
            }

            var output = Formatting.GetNamespacePath(input);
            Assert.AreEqual(expectedOutput, output);
        }

        [TestCase("", "")]
        [TestCase("S", "s")]
        [TestCase("PascalCase", "pascalCase")]
        public void PascalCaseToCamelCase_has_correct_behaviour(string input, string expectedOutput)
        {
            var output = Formatting.PascalCaseToCamelCase(input);
            Assert.AreEqual(expectedOutput, output);
        }
    }
}
