using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.NetworkStats;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.NetworkStats
{
    [TestFixture]
    public class MessageTypeUnionTests
    {
        [Test]
        public void MessageTypeUnion_with_same_type_and_data_are_equal()
        {
            var update = MessageTypeUnion.Update(10);
            Assert.AreEqual(update, update);

            var commandRequest = MessageTypeUnion.CommandRequest(10, 10);
            Assert.AreEqual(commandRequest, commandRequest);

            var commandResponse = MessageTypeUnion.CommandResponse(10, 10);
            Assert.AreEqual(commandResponse, commandResponse);

            var worldCommandRequest = MessageTypeUnion.WorldCommandRequest(WorldCommand.CreateEntity);
            Assert.AreEqual(worldCommandRequest, worldCommandRequest);

            var worldCommandResponse = MessageTypeUnion.WorldCommandResponse(WorldCommand.CreateEntity);
            Assert.AreEqual(worldCommandResponse, worldCommandResponse);
        }

        [Test]
        public void MessageTypeUnion_are_not_equal_with_different_types()
        {
            var typeOne = MessageTypeUnion.Update(10);
            var typeTwo = MessageTypeUnion.CommandRequest(10, 10);

            Assert.AreNotEqual(typeOne, typeTwo);
        }

        [Test]
        public void MessageTypeUnion_update_types_are_not_equal_with_different_data()
        {
            var updateOne = MessageTypeUnion.Update(1);
            var updateTwo = MessageTypeUnion.Update(2);

            Assert.AreNotEqual(updateOne, updateTwo);
        }

        [Test]
        public void MessageTypeUnion_command_request_types_are_not_equal_with_different_data()
        {
            var commandRequestData = new List<MessageTypeUnion>
            {
                MessageTypeUnion.CommandRequest(1, 1),
                MessageTypeUnion.CommandRequest(1, 2),
                MessageTypeUnion.CommandRequest(2, 1),
                MessageTypeUnion.CommandRequest(2, 2)
            };

            Assert.IsFalse(AreAnyEqual(commandRequestData));
        }

        [Test]
        public void MessageTypeUnion_command_response_types_are_not_equal_with_different_data()
        {
            var commandResponseData = new List<MessageTypeUnion>
            {
                MessageTypeUnion.CommandResponse(1, 1),
                MessageTypeUnion.CommandResponse(1, 2),
                MessageTypeUnion.CommandResponse(2, 1),
                MessageTypeUnion.CommandResponse(2, 2)
            };

            Assert.IsFalse(AreAnyEqual(commandResponseData));
        }

        [Test]
        public void MessageTypeUnion_world_command_request_types_are_not_equal_with_different_data()
        {
            var worldCommandRequestOne = MessageTypeUnion.WorldCommandRequest(WorldCommand.CreateEntity);
            var worldCommandRequestTwo = MessageTypeUnion.WorldCommandRequest(WorldCommand.DeleteEntity);

            Assert.AreNotEqual(worldCommandRequestOne, worldCommandRequestTwo);
        }

        [Test]
        public void MessageTypeUnion_world_command_response_types_are_not_equal_with_different_data()
        {
            var worldCommandResponseOne = MessageTypeUnion.WorldCommandResponse(WorldCommand.CreateEntity);
            var worldCommandResponseTwo = MessageTypeUnion.WorldCommandResponse(WorldCommand.DeleteEntity);

            Assert.AreNotEqual(worldCommandResponseOne, worldCommandResponseTwo);
        }

        // Checks if any element in a list is equal to any other element in a list.
        private bool AreAnyEqual<T>(List<T> data) where T : IEquatable<T>
        {
            return data.Any(element => data.Except(new[] { element }).Any(other => other.Equals(element)));
        }
    }
}
