using System.Collections.Immutable;
using RobotApp.Models;
using RobotApp.Services;
using static RobotApp.Services.ParserService;

namespace RobotApp.Tests
{
    public class ParserServiceTests
    {
        [Theory]
        [InlineData("Sample.txt", 10)]
        [InlineData("Sample1.txt", 10)]
        [InlineData("Sample2.txt", 13)]
        public void ParseInstructions_ValidFile_ReturnsInstructions(string fileName, int instructionCount)
        {
            string[] fileNames = { fileName };

            var instructions = fileNames.ParseInstructions();

            Assert.NotNull(instructions);
            Assert.True(instructions.Count == instructionCount);
        }

        [Fact]
        public void ParseInstructions_InvalidValidFile_ReturnsError()
        {
            var expected = ErrorService.Create("Parsing Error: File specified `Sample3.txt` does not exist.");
            string[] fileNames = { "Sample3.txt" };

            var result = fileNames.ParseInstructions();

            Assert.Single(result);
            Assert.Equal(expected, result.First());
        }

        [Fact]
        public void ParseInstructions_EmptyFile_ReturnsError()
        {
            var expected = ErrorService.Create("Parsing Error: No file provided for validation.");
            string[] fileNames = { "" };

            var result = fileNames.ParseInstructions();

            Assert.Single(result);
            Assert.Equal(expected, result.First());
        }

        [Theory]
        [InlineData("Sample.txt", 10)]
        [InlineData("Sample1.txt", 10)]
        [InlineData("Sample2.txt", 13)]
        public void ParseVerifiedInstructions_ValidFile_ReturnsInstructions(string fileName, int instructionCount)
        {
            string[] fileNames = { fileName };
            var instructions = ImmutableList.Create<InstructionType>().ParseVerifiedInstructions(fileNames);

            Assert.NotNull(instructions);
            Assert.True(instructions.Count == instructionCount);
        }

        [Theory]
        [InlineData("Sample.txt", 10)]
        [InlineData("Sample1.txt", 10)]
        [InlineData("Sample2.txt", 13)]
        public void ParseFile_ValidFile_ReturnsInstructions(string fileName, int instructionCount)
        {
            var file = fileName.ParseFile();

            Assert.NotNull(file);
            Assert.True(file.Count == instructionCount);
        }

        [Theory]
        [InlineData("Sample.txt")]
        [InlineData("Sample1.txt")]
        [InlineData("Sample2.txt")]
        public void GetFileErrors_ValidFileName_ReturnsNoErrors(string fileName)
        {
            string[] fileNames = { fileName };

            var result = fileNames.GetFileErrors();

            Assert.Empty(result);
        }

        [Fact]
        public void GetFileErrors_InValidFile_ReturnsErrors()
        {
            var expected = ErrorService.Create("Parsing Error: File specified `Sample3.txt` does not exist.");
            string[] fileNames = { "Sample3.txt" };

            var result = fileNames.GetFileErrors();

            Assert.Single(result);
            Assert.Equal(expected, result.First());
        }

        [Fact]
        public void GetFileErrors_EmptyFileName_ReturnsErrors()
        {
            var expected = ErrorService.Create("Parsing Error: No file provided for validation.");
            string[] fileNames = { "" };

            var result = fileNames.GetFileErrors();

            Assert.Single(result);
            Assert.Equal(expected, result.First());

        }

        [Fact]
        public void GetFileErrors_NoFileName_ReturnsErrors()
        {
            var expected = ErrorService.Create("Parsing Error: No file provided for validation.");
            string[] fileNames = Array.Empty<string>();

            var result = fileNames.GetFileErrors();

            Assert.Single(result);
            Assert.Equal(expected, result.First());

        }

        [Fact]
        public void VerifyInstructions_NoInstructions_ReturnsError()
        {
            var expected = ImmutableList.Create<InstructionType>(ErrorService.Create("Parsing Error: File contains no instructions."));
            var instructions = ImmutableList.Create<InstructionType>();

            var result = instructions.VerifyInstructions();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void VerifyInstructions_NoGridInstruction_ReturnsError()
        {
            var expected = ImmutableList.Create<InstructionType>(ErrorService.Create("Parsing Error: No grid instructions found."));
            var instructions = ImmutableList.Create(InstructionService.Create(1, 3, 'S'));

            var result = instructions.VerifyInstructions();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void VerifyInstructions_ObstaclesIncorrectlyPlaced_ReturnsError()
        {
            var expected = ImmutableList.Create<InstructionType>(ErrorService.Create("Parsing Error: Obstacles must come before journey instructions."));
            var instructions = ImmutableList.Create(
                InstructionService.Create((1, 2)),
                InstructionService.Create(1, 1),
                InstructionService.Create("FFFF"),
                InstructionService.Create(0, 0));

            var result = instructions.VerifyInstructions();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void VerifyInstructions_MissingJourneyInstructions_ReturnsError()
        {
            var expected = ImmutableList.Create<InstructionType>(ErrorService.Create("Parsing Error: No journey instructions found."));
            var instructions = ImmutableList.Create(
                InstructionService.Create((1, 2)),
                InstructionService.Create(1, 1));

            var result = instructions.VerifyInstructions();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void VerifyInstructions_InvalidJourneyInstructions_ReturnsError()
        {
            var expected = ImmutableList.Create<InstructionType>(ErrorService.Create("Parsing Error: Journey instructions must contain a start location, commands and end location. In that order."));
            var instructions = ImmutableList.Create(
                InstructionService.Create((1, 2)),
                InstructionService.Create(1, 1),
                InstructionService.Create("FFFF"));

            var result = instructions.VerifyInstructions();

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateParsingErrorResponse_ErrorMessage_ReturnsErrorList()
        {
            var expected = ImmutableList.Create<InstructionType>(ErrorService.Create("Parsing Error: Error message"));

            var result = CreateParsingErrorResponse("Error message");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void CreateParsingError_ErrorMessage_ReturnsError()
        {
            var expected = ErrorService.Create("Parsing Error: Error message");

            var result = CreateParsingError("Error message");

            Assert.Equal(expected, result);
        }

        [Fact]
        public void IsMissingGridInstruction_HasNoGrid_ReturnsTrue()
        {
            var instructions = ImmutableList.Create<InstructionType>();

            var result = instructions.IsMissingGridInstruction();

            Assert.True(result);
        }

        [Fact]
        public void IsMissingGridInstruction_HasGrid_ReturnsFalse()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((1, 2)));

            var result = instructions.IsMissingGridInstruction();

            Assert.False(result);
        }

        [Fact]
        public void HasInvalidOrderOfObstacleInstructions_WrongOrderOfInstructions_ReturnsTrue()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((1, 2)),
                InstructionService.Create(1, 1),
                InstructionService.Create("FFFF"),
                InstructionService.Create(0, 0));

            var result = instructions.HasInvalidOrderOfObstacleInstructions();

            Assert.True(result);
        }

        [Fact]
        public void HasInvalidOrderOfObstacleInstructions_ValidInstructions_ReturnsFalse()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((1, 2)),
                InstructionService.Create(1, 1),
                InstructionService.Create("FFFF"));

            var result = instructions.HasInvalidOrderOfObstacleInstructions();

            Assert.False(result);
        }

        [Fact]
        public void IsMissingJourneyInstructions_ValidInstructions_ReturnsFalse()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((1, 2)),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LFRR"),
                InstructionService.Create(1, 1, 'E'));

            var result = instructions.IsMissingJourneyInstructions();

            Assert.False(result);
        }

        [Fact]
        public void IsMissingJourneyInstructions_NoJourneyInstructions_ReturnsTrue()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((1, 2)));

            var result = instructions.IsMissingJourneyInstructions();

            Assert.True(result);
        }

        [Fact]
        public void IsMissingJourneyInstructions_NoInstructions_ReturnsTrue()
        {
            var instructions = ImmutableList.Create<InstructionType>();

            var result = instructions.IsMissingJourneyInstructions();

            Assert.True(result);
        }

        [Fact]
        public void HasInvalidJourneyInstructions_ValidInstructions_ReturnsFalse()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((1,2)),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LFRR"),
                InstructionService.Create(1, 1, 'E'));

            var result = instructions.HasInvalidJourneyInstructions();

            Assert.False(result);
        }

        [Fact]
        public void HasInvalidJourneyInstructions_TooManyInstructions_ReturnsTrue()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((1, 2)),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create("LFRR"),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'));

            var result = instructions.HasInvalidJourneyInstructions();

            Assert.True(result);
        }

        [Fact]
        public void HasInvalidJourneyInstructions_TooFewInstructions_ReturnsTrue()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((1, 2)),
                InstructionService.Create(1, 1, 'E'));

            var result = instructions.HasInvalidJourneyInstructions();

            Assert.True(result);
        }

        [Fact]
        public void HasInvalidJourneyInstructions_NoCommands_ReturnsTrue()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((1, 2)),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'),
                InstructionService.Create(1, 1, 'E'));

            var result = instructions.HasInvalidJourneyInstructions();

            Assert.True(result);
        }

        [Fact]
        public void HasInvalidJourneyInstructions_NoLocations_ReturnsTrue()
        {
            var instructions = ImmutableList.Create(
                InstructionService.Create((1, 2)),
                InstructionService.Create("LFRR"),
                InstructionService.Create("LFRR"),
                InstructionService.Create("LFRR"));

            var result = instructions.HasInvalidJourneyInstructions();

            Assert.True(result);
        }

        [Theory]
        [InlineData("GRID 4x3", 4, 3)]
        public void ParseLine_ValidGrid_ReturnsInstructionOfCorrectType(string input, int x, int y)
        {
            var instruction = ParseLine(input);

            Assert.NotNull(instruction);
            Assert.Equal(typeof(GridType), instruction.GetType());
            Assert.Equal(InstructionService.Create((x, y)), instruction);
        }

        [Theory]
        [InlineData("grid 4x3")]
        [InlineData("grid 4x-3")]
        [InlineData("GRID 4 3")]
        [InlineData("GRID 4")]
        [InlineData("GRID 4x3!")]
        [InlineData("aGRID 4x3")]
        [InlineData("GRID#1 4x3")]
        public void ParseLine_InvalidInstruction_ReturnsErrorType(string input)
        {
            var expected = ErrorService.Create($"Parsing Error: Instruction `{input}` is not formatted correctly.");
            
            var result = ParseLine(input);

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("OBSTACLE 1 2", 1, 2)]
        public void ParseLine_ValidObstacle_ReturnsInstructionOfCorrectType(string input, int x, int y)
        {
            var instruction = ParseLine(input);

            Assert.NotNull(instruction);
            Assert.Equal(typeof(ObstacleType), instruction.GetType());
            Assert.Equal(InstructionService.Create(x, y), instruction);
        }

        [Theory]
        [InlineData("1 1 E", 1, 1, 'E')]
        public void ParseLine_ValidLocation_ReturnsInstructionOfCorrectType(string input, int x, int y, char d)
        {
            var instruction = ParseLine(input);

            Assert.NotNull(instruction);
            Assert.Equal(typeof(LocationType), instruction.GetType());
            Assert.Equal(InstructionService.Create(x, y, d), instruction);
        }

        [Theory]
        [InlineData("LLFRLFF", "LLFRLFF")]
        public void ParseLine_ValidCommand_ReturnsInstructionOfCorrectType(string input, string c)
        {
            var instruction = ParseLine(input);

            Assert.NotNull(instruction);
            Assert.Equal(typeof(CommandsType), instruction.GetType());
            Assert.Equal(InstructionService.Create(c), instruction);
        }
    }
}