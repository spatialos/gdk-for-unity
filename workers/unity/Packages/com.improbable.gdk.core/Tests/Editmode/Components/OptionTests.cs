using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void Parameterless_constructor_creates_empty_option_for_value_types()
        {
            var option = new Option<bool>();
            Assert.AreEqual(false, (bool) option.HasValue);
        }

        [Test]
        public void Parameterless_constructor_creates_empty_option_for_reference_types()
        {
            var option = new Option<string>();
            Assert.AreEqual(false, (bool) option.HasValue);
        }

        [Test]
        public void Constructor_with_parameters_creates_option_with_payload_for_value_types()
        {
            const bool payload = true;
            var option = new Option<bool>(payload);
            Assert.AreEqual(true, (bool) option.HasValue);
            Assert.AreEqual(payload, option.Value);
        }

        [Test]
        public void Constructor_with_parameters_creates_option_with_payload_for_reference_types()
        {
            const string payload = "";
            var option = new Option<string>(payload);
            Assert.AreEqual(true, (bool) option.HasValue);
            Assert.AreEqual(payload, option.Value);
        }

        [Test]
        public void Option_empty_returns_an_empty_option_for_value_types()
        {
            var emptyOption = Option<bool>.Empty;
            Assert.AreEqual(new Option<bool>(), emptyOption);
        }

        [Test]
        public void Option_empty_returns_an_empty_option_for_reference_types()
        {
            var emptyOption = Option<string>.Empty;
            Assert.AreEqual(new Option<string>(), emptyOption);
        }

        [Test]
        public void Creating_option_with_null_payload_throws()
        {
            Assert.Throws<CreatedOptionWithNullPayloadException>(() =>
            {
                var _ = new Option<string>(null);
            });
        }

        [Test]
        public void Reading_value_of_empty_option_throws_for_value_types()
        {
            var option = new Option<bool>();
            Assert.Throws<CalledValueOnEmptyOptionException>(() =>
            {
                var _ = option.Value;
            });
        }

        [Test]
        public void Reading_value_of_empty_option_throws_for_reference_types()
        {
            var option = new Option<string>();
            Assert.Throws<CalledValueOnEmptyOptionException>(() =>
            {
                var _ = option.Value;
            });
        }

        [Test]
        public void TryGetValue_returns_false_when_option_empty()
        {
            var option = new Option<bool>();
            var result = option.TryGetValue(out var value);
            Assert.AreEqual(false, result);
            Assert.AreEqual(default(bool), value);
        }

        [Test]
        public void TryGetValue_returns_true_when_option_has_payload()
        {
            const bool payload = true;
            var option = new Option<bool>(payload);
            var result = option.TryGetValue(out var value);
            Assert.AreEqual(true, result);
            Assert.AreEqual(payload, value);
        }

        [Test]
        public void GetValueOrDefault_returns_default_value_when_option_empty()
        {
            var option = new Option<bool>();
            const bool defaultValue = false;
            var value = option.GetValueOrDefault(defaultValue);
            Assert.AreEqual(defaultValue, value);
        }

        [Test]
        public void GetValueOrDefault_returns_payload_when_option_has_payload()
        {
            const bool payload = true;
            var option = new Option<bool>(payload);
            const bool defaultValue = false;
            var value = option.GetValueOrDefault(defaultValue);
            Assert.AreEqual(payload, value);
        }

        [Test]
        public void Equality_comparison_with_equal_option_returns_true()
        {
            const bool payload = true;
            var option = new Option<bool>(payload);
            Assert.AreEqual(true, option.Equals(new Option<bool>(payload)));
        }

        [Test]
        public void Equality_comparison_with_unequal_option_returns_false()
        {
            var option = new Option<bool>(true);
            Assert.AreEqual(false, option.Equals(new Option<bool>(false)));
        }

        [Test]
        public void Equality_comparison_with_option_with_different_generic_parameter_returns_false()
        {
            var option = new Option<bool>(true);
            Assert.AreNotEqual(option, new Option<int>(1));
        }

        [Test]
        public void Equality_operator_works()
        {
            const bool payload = true;
            var option = new Option<bool>(payload);
            Assert.AreEqual(true, option == new Option<bool>(payload));
            Assert.AreEqual(true, option == payload);
        }

        [Test]
        public void Inequality_operator_works()
        {
            const bool payload = true;
            var option = new Option<bool>(payload);
            Assert.AreEqual(false, option != new Option<bool>(payload));
            Assert.AreEqual(false, option != payload);
        }

        [Test]
        public void Implicit_conversion_to_option_type_works()
        {
            const bool payload = true;
            Option<bool> option = payload;
            Assert.AreEqual(payload, option.Value);
        }

        [Test]
        public void Implicit_conversion_to_payload_type_works()
        {
            const bool payload = true;
            var option = new Option<bool>(payload);
            bool value = option;
            Assert.AreEqual(payload, value);
        }

        [Test]
        public void Implicit_conversion_to_payload_type_using_empty_option_throws()
        {
            var option = new Option<bool>();
            Assert.Throws<CalledValueOnEmptyOptionException>(() =>
            {
                var _ = option.Value;
            });
        }
    }
}
