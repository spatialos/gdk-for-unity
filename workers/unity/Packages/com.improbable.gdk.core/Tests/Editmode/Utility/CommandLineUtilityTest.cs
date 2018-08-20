using System.Collections.Generic;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class CommandLineUtilityTest
    {
        private enum TestEnum
        {
            Zero = 0,
            ValueOne,
            ValueTwo
        }

        [Test]
        public void GetCommandLineValue_should_convert_enum_values()
        {
            var testValue = CommandLineUtility.GetCommandLineValue(new Dictionary<string, string>
            {
                { "test-value", "ValueOne" }
            }, "test-value", TestEnum.Zero);

            Assert.AreEqual(TestEnum.ValueOne, testValue);
        }

        [Test]
        public void GetCommandLineValue_should_convert_primitive_values()
        {
            var dictionary = new Dictionary<string, string>
            {
                { "bool-value", "false" },
                { "int-value", "2" },
                { "float-value", "3.0" },
                { "double-value", "15.0" }
            };

            Assert.AreEqual(false, CommandLineUtility.GetCommandLineValue(dictionary, "bool-value", true));
            Assert.AreEqual(2, CommandLineUtility.GetCommandLineValue(dictionary, "int-value", 0));
            Assert.AreEqual(3.0f, CommandLineUtility.GetCommandLineValue(dictionary, "float-value", 0f));
            Assert.AreEqual(15.0f, CommandLineUtility.GetCommandLineValue(dictionary, "double-value", 0.0));
        }

        [Test]
        public void GetCommandLineValue_should_get_default_values_when_missing()
        {
            var testValue =
                CommandLineUtility.GetCommandLineValue(new Dictionary<string, string>(), "test-value",
                    TestEnum.ValueTwo);

            Assert.AreEqual(TestEnum.ValueTwo, testValue);
        }

        [Test]
        public void ParseCommandLineArgs_should_make_a_dictionary_when_string_array_is_provided()
        {
            var dictionary = CommandLineUtility.ParseCommandLineArgs(new List<string>
            {
                "+key1",
                "first-value",
                "+key2",
                "second-value"
            });

            Assert.AreEqual(2, dictionary.Count);
            Assert.AreEqual("first-value", dictionary["key1"]);
            Assert.AreEqual("second-value", dictionary["key2"]);
        }
    }
}
