using RobotApp.Services;
using System.Collections.Immutable;

namespace RobotApp.Tests
{
    public class InstructionServiceTests
    {
        [Fact]
        public static void CreateArgumentError_ValidArgumentErrorMessage_ReturnsError()
        {
            var expected = ErrorService.Create("Instruction Error: Error message");

            var result = InstructionService.CreateArgumentError("Error message");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetGridInfoCount_WithoutObstacles_ReturnsCount()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((10, 10)),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'));

            var results = instructions.GetGridInfoCount();

            Assert.Equal(1, results);
        }

        [Fact]
        public void GetGridInfoCount_WithObstacles_ReturnsCount()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((10, 10)),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'));

            var results = instructions.GetGridInfoCount();

            Assert.Equal(5, results);
        }

        [Fact]
        public void GetGridCount_WithoutObstacles_ReturnsCount()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((10, 10)),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'));

            var results = instructions.GetGridCount();

            Assert.Equal(1, results);
        }

        [Fact]
        public void GetGridCount_WithObstacles_ReturnsCount()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((10, 10)),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'));

            var results = instructions.GetGridCount();

            Assert.Equal(1, results);
        }

        [Fact]
        public void GetObstacleCount_WithoutObstacles_ReturnsCount()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((10, 10)),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'));

            var results = instructions.GetObstacleCount();

            Assert.Equal(0, results);
        }

        [Fact]
        public void GetObstacleCount_WithObstacles_ReturnsCount()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((10, 10)),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'));

            var results = instructions.GetObstacleCount();

            Assert.Equal(4, results);
        }

        [Fact]
        public void GetGridInfo_WithoutObstacles_ReturnsGridInstructions()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((10, 10)),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'));

            var results = instructions.GetGridInfo();

            Assert.Single(results);
        }

        [Fact]
        public void GetGridInfo_WithObstacles_ReturnsGridInstructions()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((10, 10)),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'));

            var results = instructions.GetGridInfo();

            Assert.Equal(5, results.Count);
        }

        [Fact]
        public void GetGrid_Instructions_ReturnsGrid()
        {
            var expected = InstructionService.Create((10, 10));

            var instructions = ImmutableList.Create(
                InstructionService.Create((10, 10)),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'));

            var result = instructions.GetGrid();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetObstacles_Instructions_ReturnsObstacles()
        {
            var expected = ImmutableList.Create(
                InstructionService.Create(1, 1),
                InstructionService.Create(4, 1),
                InstructionService.Create(1, 7),
                InstructionService.Create(6, 1));

            var instructions = ImmutableList.Create(
                InstructionService.Create((10, 10)),
                InstructionService.Create(1, 1),
                InstructionService.Create(4, 1),
                InstructionService.Create(1, 7),
                InstructionService.Create(6, 1),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'));

            var result = instructions.GetObstacles();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetJourneyInfo_Instructions_ReturnsJourneyInfo()
        {
            var expected = ImmutableList.Create(
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'));

            var instructions = ImmutableList.Create(
                InstructionService.Create((10, 10)),
                InstructionService.Create(1, 1),
                InstructionService.Create(4, 1),
                InstructionService.Create(1, 7),
                InstructionService.Create(6, 1),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'));

            var result = instructions.GetJourneyInfo();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetJourneyGroups_ValidInstructionsWithoutObstacle_ReturnsJourneyInstructionsInGroupingsOfThree()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((10, 10)),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'));

            var result = instructions.GetJourneyGroups();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(3, result.First().Count);
            Assert.Equal(3, result.Last().Count);
        }

        [Fact]
        public void GetJourneyGroups_ValidInstructionsWithObstacles_ReturnsJourneyInstructionsInGroupingsOfThree()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((10, 10)),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LRF"),
                InstructionService.Create(1, 1, 'E'));

            var result = instructions.GetJourneyGroups();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(3, result.First().Count);
            Assert.Equal(3, result.Last().Count);
        }

        [Fact]
        public void GetFinalLocation_ListOfLocations_ReturnsFinalLocation()
        {
            var expected = InstructionService.Create(1, 1, 'N');
            var instructions = ImmutableList.Create(
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'S'),
                InstructionService.Create(1, 1, 'W'),
                InstructionService.Create(1, 1, 'N'));

            var result = instructions.GetFinalLocation();

            Assert.Equal(expected, result);
        }
    }
}
