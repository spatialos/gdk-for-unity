using System;
using Improbable.Gdk.Core.NetworkStats;
using NUnit.Framework;

namespace Improbable.Gdk.Core.EditmodeTests.NetworkStats
{
    public class NetStatsTests
    {
        [Test]
        public void GetSummary_throws_if_requested_frames_is_longer_than_sequence_stored()
        {
            var netStats = new NetStats(5);
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                netStats.GetSummary(MessageTypeUnion.Update(54), 10, Direction.Incoming));
        }

        [Test]
        public void FrameTime_is_summed_in_summary_stats()
        {
            var netStats = new NetStats(5);

            netStats.SetFrameTime(1);
            netStats.FinishFrame();

            netStats.SetFrameTime(0.5f);
            netStats.FinishFrame();

            netStats.SetFrameTime(0.25f);
            netStats.FinishFrame();

            var (_, singleFrameTime) = netStats.GetSummary(MessageTypeUnion.Update(54), 1, Direction.Incoming);
            Assert.AreEqual(0.25f, singleFrameTime, float.Epsilon);

            var (_, twoFrameTime) = netStats.GetSummary(MessageTypeUnion.Update(54), 2, Direction.Incoming);
            Assert.AreEqual(0.75f, twoFrameTime, float.Epsilon);

            var (_, threeFrameTime) = netStats.GetSummary(MessageTypeUnion.Update(54), 3, Direction.Incoming);
            Assert.AreEqual(1.75f, threeFrameTime, float.Epsilon);
        }

        [Test]
        public void FrameTime_summation_wraps_correctly()
        {
            var netStats = new NetStats(5);

            for (var i = 1; i <= 5; i++)
            {
                netStats.SetFrameTime(i);
                netStats.FinishFrame();
            }

            // After the for loop:
            // FrameTime buffer - [5, 4, 3, 2, 1]
            //                 nextInsertIndex ^

            var (_, threeFrameTime) = netStats.GetSummary(MessageTypeUnion.Update(54), 3, Direction.Incoming);
            Assert.AreEqual(5 + 4 + 3, threeFrameTime, float.Epsilon);
        }

        [TestCase(Direction.Incoming)]
        [TestCase(Direction.Outgoing)]
        public void SetFrameStats_inserts_into_the_correct_direction_and_message_type(Direction direction)
        {
            var netStats = new NetStats(5);
            var netFrameStats = NetFrameStats.Pool.Rent();
            var dataInjector = netFrameStats.TestInjector;

            dataInjector.AddComponentUpdate(5, 3);
            dataInjector.AddComponentUpdate(10, 3);
            dataInjector.AddComponentUpdate(10, 3);

            dataInjector.AddCommandRequest(10, 5, 3);
            dataInjector.AddCommandRequest(5, 10, 3);
            dataInjector.AddCommandRequest(5, 10, 3);

            dataInjector.AddCommandResponse(10, 5, 3);
            dataInjector.AddCommandResponse(5, 10, 3);
            dataInjector.AddCommandResponse(5, 10, 3);

            // To eliminate the possibility of cross-talk we need to insert these in more than once or twice to get
            // unique counts.

            for (var i = 0; i < 3; i++)
            {
                dataInjector.AddWorldCommandRequest(WorldCommand.CreateEntity);
                dataInjector.AddWorldCommandRequest(WorldCommand.DeleteEntity);
                dataInjector.AddWorldCommandRequest(WorldCommand.DeleteEntity);
            }

            for (var i = 0; i < 5; i++)
            {
                dataInjector.AddWorldCommandResponse(WorldCommand.EntityQuery);
                dataInjector.AddWorldCommandResponse(WorldCommand.ReserveEntityIds);
                dataInjector.AddWorldCommandResponse(WorldCommand.ReserveEntityIds);
            }

            netStats.SetFrameStats(netFrameStats, direction);
            netStats.FinishFrame();

            var (updateOne, _) = netStats.GetSummary(MessageTypeUnion.Update(5), 1, direction);
            Assert.AreEqual(1, updateOne.Count);
            Assert.AreEqual(3, updateOne.Size);

            var (updateTwo, _) = netStats.GetSummary(MessageTypeUnion.Update(10), 1, direction);
            Assert.AreEqual(2, updateTwo.Count);
            Assert.AreEqual(6, updateTwo.Size);

            var (commandRequestOne, _) = netStats.GetSummary(MessageTypeUnion.CommandRequest(10, 5), 1, direction);
            Assert.AreEqual(1, commandRequestOne.Count);
            Assert.AreEqual(3, commandRequestOne.Size);

            var (commandRequestTwo, _) = netStats.GetSummary(MessageTypeUnion.CommandRequest(5, 10), 1, direction);
            Assert.AreEqual(2, commandRequestTwo.Count);
            Assert.AreEqual(6, commandRequestTwo.Size);

            var (commandResponseOne, _) = netStats.GetSummary(MessageTypeUnion.CommandResponse(10, 5), 1, direction);
            Assert.AreEqual(1, commandResponseOne.Count);
            Assert.AreEqual(3, commandResponseOne.Size);

            var (commandResponseTwo, _) = netStats.GetSummary(MessageTypeUnion.CommandResponse(5, 10), 1, direction);
            Assert.AreEqual(2, commandResponseTwo.Count);
            Assert.AreEqual(6, commandResponseTwo.Size);

            var (worldCommandRequestOne, _) = netStats.GetSummary(MessageTypeUnion.WorldCommandRequest(WorldCommand.CreateEntity), 1, direction);
            Assert.AreEqual(3, worldCommandRequestOne.Count);

            var (worldCommandRequestTwo, _) = netStats.GetSummary(MessageTypeUnion.WorldCommandRequest(WorldCommand.DeleteEntity), 1, direction);
            Assert.AreEqual(6, worldCommandRequestTwo.Count);

            var (worldCommandResponseOne, _) = netStats.GetSummary(MessageTypeUnion.WorldCommandResponse(WorldCommand.EntityQuery), 1, direction);
            Assert.AreEqual(5, worldCommandResponseOne.Count);

            var (worldCommandResponseTwo, _) = netStats.GetSummary(MessageTypeUnion.WorldCommandResponse(WorldCommand.ReserveEntityIds), 1, direction);
            Assert.AreEqual(10, worldCommandResponseTwo.Count);
        }

        [Test]
        public void FrameStats_is_summed_in_summary_stats_and_wraps_correctly()
        {
            var netStats = new NetStats(5);
            var netFrameStats = NetFrameStats.Pool.Rent();
            var dataInjector = netFrameStats.TestInjector;

            var messageType = MessageTypeUnion.Update(10);
            var direction = Direction.Incoming;

            for (uint i = 1; i <= 5; i++)
            {
                dataInjector.AddComponentUpdate(10, i);
                netStats.SetFrameStats(netFrameStats, direction);
                netStats.FinishFrame();
                netFrameStats.Clear();
            }

            // After the for loop, for the correct direction and message type.
            //           buffer - [5, 4, 3, 2, 1]
            //                 nextInsertIndex ^


            var (singleFrameData, _) = netStats.GetSummary(messageType, 1, direction);
            Assert.AreEqual(1, singleFrameData.Count);
            Assert.AreEqual(5, singleFrameData.Size);

            var (twoFrameData, _) = netStats.GetSummary(messageType, 2, direction);
            Assert.AreEqual(2, twoFrameData.Count);
            Assert.AreEqual(5 + 4, twoFrameData.Size);

            var (threeFrameData, _) = netStats.GetSummary(messageType, 3, direction);
            Assert.AreEqual(3, threeFrameData.Count);
            Assert.AreEqual(5 + 4 + 3, threeFrameData.Size);
        }
    }
}
