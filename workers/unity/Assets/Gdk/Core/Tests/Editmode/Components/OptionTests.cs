using System;
using NUnit.Framework;
using Unity.Mathematics;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void Parameterless_constructor_creates_empty_option()
        {
            var option = new Option<bool>();
            Assert.AreEqual((bool1)false, option.HasValue);
        }

        [Test]
        public void Constructor_with_parameters_creates_option_with_payload()
        {
            var payload = true;
            var option = new Option<bool>(payload);
            Assert.AreEqual((bool1)true, option.HasValue);
            Assert.AreEqual(payload, option.Value);
        }

        [Test]
        public void Value_can_be_read()
        {
            var payload = true;
            var option = new Option<bool>(payload);
            Assert.AreEqual(payload, option.Value);
        }

        [Test]
        public void Value_can_be_set()
        {
            var payload = true;
            var option = new Option<bool>();
            option.Value = payload;
            Assert.AreEqual((bool1)true, option.HasValue);
            Assert.AreEqual(payload, option.Value);
        }

        [Test]
        public void Accessing_value_of_empty_option_throws()
        {
            var option = new Option<bool>();
            Assert.AreEqual((bool1)false, option.HasValue);
            Assert.Throws<InvalidOperationException>(() =>
            {
                var value = option.Value;
            });
        }

        [Test]
        public void Setting_null_value_throws()
        {
            var option = new Option<string>();
            Assert.AreEqual((bool1)false, option.HasValue);
            Assert.Throws<ArgumentException>(() =>
            {
                option.Value = null;
            });
        }

        [Test]
        public void Option_payload_can_be_cleared()
        {
            var option = new Option<bool>(true);
            Assert.AreEqual((bool1)true, option.HasValue);
            option.Clear();
            Assert.AreEqual((bool1)false, option.HasValue);
        }

        [Test]
        public void TryGetValue_on_empty_option()
        {
            var option = new Option<bool>();
            bool value;
            var result = option.TryGetValue(out value);
            Assert.AreEqual(false, result);
            Assert.AreEqual(default(bool), value);
        }

        [Test]
        public void TryGetValue_on_option_with_payload()
        {
            var payload = true;
            var option = new Option<bool>(payload);
            bool value;
            var result = option.TryGetValue(out value);
            Assert.AreEqual(true, result);
            Assert.AreEqual(payload, value);
        }

        [Test]
        public void GetValueOrDefault_on_empty_option()
        {
            var option = new Option<bool>();
            var defaultValue = false;
            var value = option.GetValueOrDefault(defaultValue);
            Assert.AreEqual(defaultValue, value);
        }

        [Test]
        public void GetValueOrDefault_on_option_with_payload()
        {
            var payload = true;
            var option = new Option<bool>(payload);
            var defaultValue = false;
            var value = option.GetValueOrDefault(defaultValue);
            Assert.AreEqual(payload, value);
        }

        [Test]
        public void Equality_comparison_with_equal_option()
        {
            var payload = true;
            var option = new Option<bool>(payload);
            Assert.AreEqual(true, option.Equals(new Option<bool>(payload)));
        }

        [Test]
        public void Equality_comparison_with_unequal_option()
        {
            var option = new Option<bool>(true);
            Assert.AreEqual(false, option.Equals(new Option<bool>(false)));
        }

        [Test]
        public void Equality_comparison_with_option_with_different_generic_parameter()
        {
            var option = new Option<bool>(true);
            Assert.AreEqual(false, option.Equals(new Option<int>(1)));
        }

        [Test]
        public void Equality_comparison_with_equal_payload()
        {
            var payload = true;
            var option = new Option<bool>(payload);
            Assert.AreEqual(true, option.Equals(payload));
        }

        [Test]
        public void Equality_operator()
        {
            var payload = true;
            var option = new Option<bool>(payload);
            Assert.AreEqual(true, option == option);
        }

        [Test]
        public void Inequality_operator()
        {
            var payload = true;
            var option = new Option<bool>(payload);
            Assert.AreEqual(false, option != option);
        }

        [Test]
        public void Implicit_conversion_to_option_type()
        {
            var payload = true;
            Option<bool> option = payload;
            Assert.AreEqual(payload, option.Value);
        }

        [Test]
        public void Implicit_conversion_to_payload_type()
        {
            var payload = true;
            var option = new Option<bool>(payload);
            bool value = option;
            Assert.AreEqual(payload, value);
        }

        [Test]
        public void Implicit_conversion_to_payload_type_using_empty_option_throws()
        {
            var option = new Option<bool>();
            Assert.Throws<InvalidOperationException>(() =>
            {
                bool value = option.Value;
            });
        }
    }
}
