using RobotApp.Models;
using RobotApp.Services;
using System.Collections.Immutable;
using static RobotApp.Services.ValidationService;

namespace RobotApp.Tests
{
    public class ValidationServiceTests
    {
        [Fact]
        public void ValidateInstructions_ValidInput_CorrectOutcomesAreReturned()
        {
            var inputs = new[] { "Sample.txt" };

            var expected = ImmutableList.Create<InstructionType>(
                ResultService.Create("SUCCESS 1 0 W"),
                ResultService.Create("FAILURE 0 0 W"),
                ResultService.Create("OUT OF BOUNDS"));

            var result = inputs.ValidateInstructions();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ValidateJourneys_ValidSetOfInstructions_CorrectOutcomesAreReturned()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((4, 3)),
                InstructionService.Create(4, 3),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("RFR"),
                InstructionService.Create(1, 0, 'W'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("RFRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("RFF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(4, 3, 'N'),
                InstructionService.Create("RRRR"),
                InstructionService.Create(4, 3, 'N'));

            var expected = ImmutableList.Create<InstructionType>(
                ResultService.Create("SUCCESS 1 0 W"),
                ResultService.Create("FAILURE 0 0 W"),
                ResultService.Create("OUT OF BOUNDS"),
                ResultService.Create("CRASHED 4 3"));

            var result = instructions.ValidateJourneys();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void ValidateJourneys_InstructionsContainError_ReturnsParsingError()
        {
            var expected = ImmutableList.Create<InstructionType>(ErrorService.Create("Error message"));
            var instructions = ImmutableList.Create<InstructionType>(ErrorService.Create("Error message"));

            var result = instructions.ValidateJourneys();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void PerformJourneys_ValidSetOfInstructions_CorrectOutcomesAreReturned()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((4, 3)),
                InstructionService.Create(4, 3),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("RFR"),
                InstructionService.Create(1, 0, 'W'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("RFRF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("RFF"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(4, 3, 'N'),
                InstructionService.Create("RRRR"),
                InstructionService.Create(4, 3, 'N'));

            var expected = ImmutableList.Create<InstructionType>(
                ResultService.Create("SUCCESS 1 0 W"),
                ResultService.Create("FAILURE 0 0 W"),
                ResultService.Create("OUT OF BOUNDS"),
                ResultService.Create("CRASHED 4 3"));

            var result = instructions.PerformJourneys();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void PerformJourneys_InstructionsContainError_ReturnsParsingError()
        {
            var expected = ImmutableList.Create<InstructionType>(ErrorService.Create("Invalid journey information provided."));
            var instructions = ImmutableList.Create<InstructionType>(ErrorService.Create("Error message"));

            var result = instructions.PerformJourneys();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void PerformMovements_SuccessfulJourney_ReturnsSuccessResult()
        {
            var expected = ImmutableList.Create(ResultService.Create("SUCCESS 1 0 W"));
            var gridInfo = ImmutableList.Create(InstructionService.Create((4, 3)));
            var journey = ImmutableList.Create(
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("RFR"),
                InstructionService.Create(1, 0, 'W'));

            var result = journey.PerformMovements(gridInfo);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void PerformMovements_FailingJourney_ReturnsFailureResult()
        {
            var expected = ImmutableList.Create(ResultService.Create("FAILURE 0 0 W"));
            var gridInfo = ImmutableList.Create(InstructionService.Create((4, 3)));
            var journey = ImmutableList.Create(
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("RFRF"),
                InstructionService.Create(1, 1, 'E'));

            var result = journey.PerformMovements(gridInfo);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void PerformMovements_OutOfBoundsJourney_ReturnsOutOfBoundsResult()
        {
            var expected = ImmutableList.Create(ResultService.Create("OUT OF BOUNDS"));
            var gridInfo = ImmutableList.Create(InstructionService.Create((4, 3)));
            var journey = ImmutableList.Create(
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("RFF"),
                InstructionService.Create(1, 1, 'E'));

            var result = journey.PerformMovements(gridInfo);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void PerformMovements_CrashedJourney_ReturnsCrashedResult()
        {
            var expected = ImmutableList.Create(ResultService.Create("CRASHED 4 3"));
            var gridInfo = ImmutableList.Create(
                InstructionService.Create((4, 3)),
                InstructionService.Create(4, 3));
            var journey = ImmutableList.Create(
                InstructionService.Create(4, 3, 'N'),
                InstructionService.Create("RRRR"),
                InstructionService.Create(4, 3, 'N'));

            var result = journey.PerformMovements(gridInfo);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void PerformMovement_SuccessfulJourney_ReturnsSuccessResult()
        {
            var expected = ImmutableList.Create(ResultService.Create("SUCCESS 1 0 W"));
            var gridInfo = ImmutableList.Create(InstructionService.Create((4, 3)));
            var start = ImmutableList.Create(InstructionService.Create(1, 1, 'E'));
            var end = InstructionService.Create(1, 0, 'W');

            var result = start.PerformMovement("RFR", end, gridInfo);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void PerformMovement_FailedJourney_ReturnsFailureResult()
        {
            var expected = ImmutableList.Create(ResultService.Create("FAILURE 0 0 W"));
            var gridInfo = ImmutableList.Create(InstructionService.Create((4, 3)));
            var start = ImmutableList.Create(InstructionService.Create(1, 1, 'E'));
            var end = InstructionService.Create(1, 1, 'E');

            var result = start.PerformMovement("RFRF", end, gridInfo);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void PerformMovement_OutOfBoundsJourney_ReturnsOutOfBoundsResult()
        {
            var expected = ImmutableList.Create(ResultService.Create("OUT OF BOUNDS"));
            var gridInfo = ImmutableList.Create(InstructionService.Create((4, 3)));
            var start = ImmutableList.Create(InstructionService.Create(1, 1, 'E'));
            var end = InstructionService.Create(1, 1, 'E');

            var result = start.PerformMovement("RFFFFFFR", end, gridInfo);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void PerformMovement_CrashedJourney_ReturnsCrashedResult()
        {
            var expected = ImmutableList.Create(ResultService.Create("CRASHED 1 1"));
            var gridInfo = ImmutableList.Create(
                InstructionService.Create((4, 3)),
                InstructionService.Create(1, 1));
            var start = ImmutableList.Create(InstructionService.Create(1, 1, 'E'));
            var end = InstructionService.Create(1, 0, 'W');

            var result = start.PerformMovement("RRRR", end, gridInfo);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetJourneyOutcome_SuccessfulJourney_ReturnsSuccessResult()
        {
            var expected = ImmutableList.Create(ResultService.Create("SUCCESS 1 0 W"));
            var gridInfo = ImmutableList.Create(InstructionService.Create((4, 3)));
            var journey = ImmutableList.Create(
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'S'));
            var end = InstructionService.Create(1, 0, 'W');

            var result = journey.GetJourneyOutcome(gridInfo, "RFR", end);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CheckForJourneyOutcome_SuccessfulJourney_ReturnsSuccessResult()
        {
            var expected = ImmutableList.Create(ResultService.Create("SUCCESS 1 0 W"));
            var gridInfo = ImmutableList.Create(InstructionService.Create((4, 3)));
            var journey = ImmutableList.Create(
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'S'));
            var end = InstructionService.Create(1, 0, 'W');

            var result = journey.CheckForJourneyOutcome(gridInfo, "RFR", end, journey[^1]);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CheckForJourneyOutcome_FailedJourney_ReturnsFailureResult()
        {
            var expected = ImmutableList.Create(ResultService.Create("FAILURE 1 0 W"));
            var gridInfo = ImmutableList.Create(InstructionService.Create((4, 3)));
            var journey = ImmutableList.Create(
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'S'));
            var end = InstructionService.Create(1, 0, 'N');

            var result = journey.CheckForJourneyOutcome(gridInfo, "RFR", end, journey[^1]);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CheckForJourneyOutcome_OutOfBoundsJourney_ReturnsFailureResult()
        {
            var expected = ImmutableList.Create(ResultService.Create("OUT OF BOUNDS"));
            var gridInfo = ImmutableList.Create(InstructionService.Create((4, 3)));
            var journey = ImmutableList.Create(
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'S'));
            var end = InstructionService.Create(1, 0, 'N');

            var result = journey.CheckForJourneyOutcome(gridInfo, "RFFFFFR", end, journey[^1]);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CheckForJourneyOutcome_CrashedJourney_ReturnsCrashResult()
        {
            var expected = ImmutableList.Create(ResultService.Create("CRASHED 1 1"));
            var gridInfo = ImmutableList.Create(
                InstructionService.Create((4, 3)),
                InstructionService.Create(1, 1));
            var journey = ImmutableList.Create(
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'S'));
            var end = InstructionService.Create(1, 0, 'N');

            var result = journey.CheckForJourneyOutcome(gridInfo, "RFR", end, journey[^1]);

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateOutcomeResponse_OutcomeMessage_ReturnsOutcome()
        {
            var expected = ResultService.Create("SUCCESS");

            var result = CreateOutcomeResponse("SUCCESS")[0];

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(1, 1, false)]
        [InlineData(10, 10, false)]
        [InlineData(-1, 0, true)]
        [InlineData(0, -1, true)]
        [InlineData(-1, -1, true)]
        [InlineData(11, 10, true)]
        [InlineData(10, 11, true)]
        [InlineData(11, 11, true)]
        public void HasLeftGrid_LocationsInGrid_ReturnsIfTheyAreOutsideOfTheGrid(int x, int y, bool expected)
        {
            var grid = InstructionService.Create((10, 10));

            var result = grid.HasLeftGrid(x, y);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(0, 0, false)]
        [InlineData(1, 1, true)]
        public void HasCrashedIntoObstacle_LocationsInGrid_ReturnsIfCrashedIntoObstacle(int x, int y, bool expected)
        {
            var obstacles = ImmutableList.Create(InstructionService.Create((10, 10)),
                InstructionService.Create(1, 1));

            var result = obstacles.HasCrashedIntoObstacle(x, y);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, 1, 'N', 'L', 1, 1, 'W')]
        [InlineData(1, 1, 'N', 'R', 1, 1, 'E')]
        [InlineData(1, 1, 'N', 'F', 1, 2, 'N')]
        [InlineData(1, 1, 'W', 'L', 1, 1, 'S')]
        [InlineData(1, 1, 'W', 'R', 1, 1, 'N')]
        [InlineData(1, 1, 'W', 'F', 0, 1, 'W')]
        public void Move_GivenLocation_ReturnsNextLocation(int x, int y, char direction, char command, int nextX, int nextY, char nextDirection)
        {
            var expected = InstructionService.Create(nextX, nextY, nextDirection);

            var result = command.Move(x, y, direction);

            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(1, 1, 'N', 1, 2)]
        [InlineData(1, 1, 'E', 2, 1)]
        [InlineData(1, 1, 'S', 1, 0)]
        [InlineData(1, 1, 'W', 0, 1)]
        public void MoveForward_ValidLocationInformation_ReturnsLocationAfterMovement(int startX, int startY, char direction, int endX, int endY)
        {
            var result = direction.MoveForward(startX, startY);

            Assert.NotNull(result);
            Assert.Equal(InstructionService.Create(endX, endY, direction), result);
        }

        [Fact]
        public void MoveForward_InValidDirection_ReturnsLocationWithoutMovement()
        {
            var result = char.MinValue.MoveForward(1, 1);

            Assert.NotNull(result);
            Assert.Equal(InstructionService.Create(1, 1, char.MinValue), result);
        }

        [Theory]
        [InlineData('N', 'L', 'W')]
        [InlineData('E', 'L', 'N')]
        [InlineData('S', 'L', 'E')]
        [InlineData('W', 'L', 'S')]
        [InlineData('N', 'R', 'E')]
        [InlineData('E', 'R', 'S')]
        [InlineData('S', 'R', 'W')]
        [InlineData('W', 'R', 'N')]
        public void Turn_ValidInformation_ReturnsLocationAfterTurn(char direction, char command, char turnedDirection)
        {
            var expected = InstructionService.Create(1, 1, turnedDirection);
            var result = direction.Turn(command, 1, 1);

            Assert.Equal(result, expected);
        }

        [Theory]
        [InlineData('N', 'L', 'W')]
        [InlineData('E', 'L', 'N')]
        [InlineData('S', 'L', 'E')]
        [InlineData('W', 'L', 'S')]
        [InlineData('N', 'R', 'E')]
        [InlineData('E', 'R', 'S')]
        [InlineData('S', 'R', 'W')]
        [InlineData('W', 'R', 'N')]
        public void GetDirectionAfterTurn_ValidDirectionAndTurnCommand_ReturnsDirectionAfterTurn(char direction, char command, char expected)
        {
            var result = command.GetDirectionAfterTurn(direction);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData('L', "ESWN")]
        [InlineData('R', "NWSE")]
        public void GetDirections_ValidCommand_ReturnsCorrectOrder(char command, string expected)
        {
            var result = command.GetDirections();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetDirectionIndex_NegativeValue_ReturnsLastIndex()
        {
            var result = GetDirectionIndex(-1);

            Assert.Equal(^1, result);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        public void GetDirectionIndex_PositiveIndex_ReturnsSameIndex(int index, int expected)
        {
            var result = GetDirectionIndex(index);

            Assert.Equal(expected, result);
        }
    }
}
