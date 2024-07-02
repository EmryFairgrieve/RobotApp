using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using RobotApp.Models;
using RobotApp.Resources;

namespace RobotApp.Services
{
    public static class ParserService
    {
        public static IImmutableList<InstructionType> ParseInstructions(this string[] input) =>
            input.GetFileErrors()
                .ParseVerifiedInstructions(input);

        public static IImmutableList<InstructionType> GetFileErrors(this string[] input) => input switch
        {
            _ when input.IsFileNameMissingOrEmpty() => CreateParsingErrorResponse("No file provided for validation."),
            _ when input.IsFileMissing() => CreateParsingErrorResponse($"File specified `{input.First()}` does not exist."),
            _ => ImmutableList.Create<InstructionType>()
        };

        public static IImmutableList<InstructionType> ParseVerifiedInstructions(this IImmutableList<InstructionType> fileErrors, string[] input) =>
            fileErrors.Any()
                ? fileErrors
                : input.First()
                    .ParseFile()
                    .VerifyInstructions();

        public static IImmutableList<InstructionType> ParseFile(this string fileName) =>
            File.ReadLines(fileName)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(ParseLine)
                .ToImmutableList();

        public static IImmutableList<InstructionType> VerifyInstructions(this IImmutableList<InstructionType> instructions) => instructions.HasErrors()
            ? instructions
            : instructions switch
            {
                _ when !instructions.Any() => CreateParsingErrorResponse("File contains no instructions."),
                _ when instructions.IsMissingGridInstruction() => CreateParsingErrorResponse("No grid instructions found."),
                _ when instructions.HasInvalidOrderOfObstacleInstructions() => CreateParsingErrorResponse("Obstacles must come before journey instructions."),
                _ when instructions.IsMissingJourneyInstructions() => CreateParsingErrorResponse("No journey instructions found."),
                _ when instructions.HasInvalidJourneyInstructions() => CreateParsingErrorResponse("Journey instructions must contain a start location, commands and end location. In that order."),
                _ => instructions
            };

        public static IImmutableList<InstructionType> CreateParsingErrorResponse(string errorMessage) =>
            ImmutableList.Create(CreateParsingError(errorMessage));

        public static InstructionType CreateParsingError(string errorMessage) =>
            ErrorService.Create($"Parsing Error: {errorMessage}");

        public static bool IsFileNameMissingOrEmpty(this string[] input) =>
            !input.Any()
            || string.IsNullOrWhiteSpace(input.First());

        public static bool IsFileMissing(this string[] fileName) =>
            !File.Exists(fileName.First());

        public static bool IsMissingGridInstruction(this IImmutableList<InstructionType> instructions) =>
            instructions.GetGrid().GetType() != typeof(GridType);

        public static bool HasInvalidOrderOfObstacleInstructions(this IImmutableList<InstructionType> instructions) =>
            instructions.GetJourneyInfo().Any(i => i.GetType() == typeof(ObstacleType));

        public static bool IsMissingJourneyInstructions(this IImmutableList<InstructionType> instructions) =>
            !instructions.GetJourneyInfo().Any();

        public static bool HasInvalidJourneyInstructions(this IImmutableList<InstructionType> instructions) =>
            instructions.GetJourneyGroups()
                .Any(journey =>
                    journey.Count != 3
                    || journey.First().GetType() != typeof(LocationType)
                    || journey.Skip(1).First().GetType() != typeof(CommandsType)
                    || journey.Last().GetType() != typeof(LocationType));

        public static InstructionType ParseLine(string input) => input switch
        {
            _ when Pattern.Grid.IsMatch(input) => ParseGrid(Pattern.Grid.Match(input).Groups),
            _ when Pattern.Obstacle.IsMatch(input) => ParseObstacle(Pattern.Obstacle.Match(input).Groups),
            _ when Pattern.Location.IsMatch(input) => ParseLocation(Pattern.Location.Match(input).Groups),
            _ when Pattern.Commands.IsMatch(input) => ParseCommands(Pattern.Commands.Match(input).Groups),
            _ => CreateParsingError($"Instruction `{input}` is not formatted correctly.")
        };

        private static InstructionType ParseGrid(GroupCollection groups) =>
            InstructionService.Create(groups.GetSize());

        private static InstructionType ParseObstacle(GroupCollection groups) =>
            InstructionService.Create(groups.GetX(), groups.GetY());

        private static InstructionType ParseLocation(GroupCollection groups) =>
            InstructionService.Create(groups.GetX(), groups.GetY(), groups.GetD());

        private static InstructionType ParseCommands(GroupCollection groups) =>
            InstructionService.Create(groups.GetC());

        private static (int, int) GetSize(this GroupCollection groups) =>
            (groups.GetInt("x"), groups.GetInt("y"));
        
        private static int GetX(this GroupCollection groups) =>
            groups.GetInt("x");

        private static int GetY(this GroupCollection groups) =>
            groups.GetInt("y");

        private static char GetD(this GroupCollection groups) =>
            char.Parse(groups.GetValue("d"));

        private static string GetC(this GroupCollection groups) =>
            groups.GetValue("c");

        private static int GetInt(this GroupCollection groups, string valueType) =>
            int.Parse(groups.GetValue(valueType));

        private static string GetValue(this GroupCollection groups, string value) =>
            groups[value].Value;
    }
}
