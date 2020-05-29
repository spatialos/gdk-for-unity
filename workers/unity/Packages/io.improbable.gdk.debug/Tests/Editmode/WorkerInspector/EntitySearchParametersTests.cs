using Improbable.Gdk.Debug.WorkerInspector;
using NUnit.Framework;

namespace Improbable.Gdk.Debug.EditmodeTests.WorkerInspector
{
    [TestFixture]
    public class EntitySearchParametersTests
    {
        [TestCase("1", 1)]
        [TestCase(" 1 ", 1)]
        [TestCase("1    ", 1)]
        public void FromSearchString_parses_out_numbers(string searchString, long resultingId)
        {
            var searchParams = EntitySearchParameters.FromSearchString(searchString);
            Assert.IsTrue(searchParams.EntityId.HasValue);
            Assert.AreEqual(resultingId, searchParams.EntityId.Value.Id);
        }

        [TestCase("0")]
        [TestCase("-1")]
        [TestCase("300 spartans")]
        [TestCase("9223372036854775808")] // Max long + 1
        public void FromSearchString_rejects_non_entity_id_values(string searchString)
        {
            var searchParams = EntitySearchParameters.FromSearchString(searchString);
            Assert.IsFalse(searchParams.EntityId.HasValue);
        }

        [TestCase("some value", "some value")]
        [TestCase(" with leading space", "with leading space")]
        [TestCase("with trailing space ", "with trailing space")]
        [TestCase(" with both! ", "with both!")]
        public void FromSearchString_ignores_leading_and_trailing_whitespace(string searchString, string expectedFragment)
        {
            var searchParams = EntitySearchParameters.FromSearchString(searchString);
            Assert.IsNotNull(searchParams.SearchFragment);
            Assert.AreEqual(expectedFragment, searchParams.SearchFragment);
        }

        [TestCase]
        public void FromSearchString_converts_all_to_lower()
        {
            var searchParams = EntitySearchParameters.FromSearchString("CAPS");
            Assert.IsNotNull(searchParams.SearchFragment);
            Assert.AreEqual("caps", searchParams.SearchFragment);
        }
    }
}
