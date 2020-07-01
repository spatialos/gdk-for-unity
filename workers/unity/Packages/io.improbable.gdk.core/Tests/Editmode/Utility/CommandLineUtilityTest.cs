using System;
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
            var args = CommandLineArgs.From(new Dictionary<string, string>
            {
                { "test-value", "ValueOne" }
            });

            var testValue = args.GetCommandLineValue("test-value", TestEnum.Zero);
            Assert.AreEqual(TestEnum.ValueOne, testValue);
        }

        [Test]
        public void GetCommandLineValue_should_convert_primitive_values()
        {
            var args = CommandLineArgs.From(new Dictionary<string, string>
            {
                { "bool-value", "false" },
                { "int-value", "2" },
                { "float-value", "3.0" },
                { "double-value", "15.0" }
            });

            Assert.AreEqual(false, args.GetCommandLineValue("bool-value", true));
            Assert.AreEqual(2, args.GetCommandLineValue("int-value", 0));
            Assert.AreEqual(3.0f, args.GetCommandLineValue("float-value", 0f));
            Assert.AreEqual(15.0f, args.GetCommandLineValue("double-value", 0.0));
        }

        [Test]
        public void GetCommandLineValue_should_get_default_values_when_missing()
        {
            var args = CommandLineArgs.From(new Dictionary<string, string>());

            var testValue = args.GetCommandLineValue("test-value", TestEnum.ValueTwo);

            Assert.AreEqual(TestEnum.ValueTwo, testValue);
        }

        [Test]
        public void ParseCommandLineArgs_should_process_args_when_string_array_is_provided()
        {
            var args = CommandLineArgs.From(new List<string>
            {
                "+key1",
                "first-value",
                "+key2",
                "second-value"
            });

            Assert.AreEqual("first-value", args.GetCommandLineValue("key1", "not-value"));
            Assert.AreEqual("second-value", args.GetCommandLineValue("key2", "not-value"));
        }

        [Test]
        public void ParseCommandLineArgs_can_parse_flag_arguments()
        {
            var args = CommandLineArgs.From(new List<string>
            {
                "+key1",
                "first-value",
                "+flag",
                "+key2",
                "second-value"
            });
            Assert.True(args.Contains("key1"));
            Assert.True(args.Contains("key2"));
            Assert.True(args.Contains("flag"));
        }

        [Test]
        public void ParseCommandLineArgs_can_parse_flag_arguments_at_end_of_arg_list()
        {
            var args = CommandLineArgs.From(new List<string>
            {
                "+key1",
                "first-value",
                "+key2",
                "second-value",
                "+flag"
            });
            Assert.True(args.Contains("key1"));
            Assert.True(args.Contains("key2"));
            Assert.True(args.Contains("flag"));
        }

        [Test]
        public void TryGetCommandLineValue_throws_when_accessing_flag_only_args()
        {
            var args = CommandLineArgs.From(new List<string>
            {
                "+key1",
                "first-value",
                "+key2",
                "second-value",
                "+flag"
            });

            var boolVal = false;
            Assert.Throws<InvalidOperationException>(() => args.TryGetCommandLineValue("flag", ref boolVal));
            var stringVal = string.Empty;
            Assert.Throws<InvalidOperationException>(() => args.TryGetCommandLineValue("flag", ref stringVal));
        }

        [Test]
        public void TryGetCommandLineValue_is_valid_for_empty_arg()
        {
            var args = CommandLineArgs.From(new List<string>
            {
                "+key1",
                "",
                "+key2",
                "second-value"
            });

            var stringVal = string.Empty;
            Assert.DoesNotThrow(() => args.TryGetCommandLineValue("key1", ref stringVal));
            Assert.IsEmpty(stringVal);
        }
    }
}
