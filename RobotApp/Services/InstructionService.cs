using System.Collections.Immutable;
using System.Linq;
using RobotApp.Models;

namespace RobotApp.Services
{
    public static class InstructionService
    {
        public static InstructionType Create((int width, int height) size) =>
            size.width <= 0 || size.height <= 0
                ? CreateArgumentError($"Grid width and height `({size.width}, {size.height})` most be positive numbers.")
                : new GridType(size);

        public static InstructionType Create(int positionX, int positionY) =>
            positionX < 0 || positionY < 0
                ? CreateArgumentError($"Obstacle x and y coordinates `({positionX}, {positionY})` can not be negative numbers.")
                : new ObstacleType(positionX, positionY);

        public static InstructionType Create(int positionX, int positionY, char direction) =>
            !char.IsLetter(direction)
                ? CreateArgumentError($"Direction `{direction}` must be a character.")
                : new LocationType(positionX, positionY, direction);

        public static InstructionType Create(string commands) =>
            !commands.Any()
                ? CreateArgumentError("No commands have been specified.")
                : new CommandsType(commands);

        public static InstructionType CreateArgumentError(string message) =>
            ErrorService.Create($"Instruction Error: {message}");

        public static int GetGridInfoCount(this IImmutableList<InstructionType> instructions) =>
            instructions.GetGridCount() + instructions.GetObstacleCount();

        public static int GetGridCount(this IImmutableList<InstructionType> instructions) =>
            instructions.OfType<GridType>().Count();

        public static int GetObstacleCount(this IImmutableList<InstructionType> instructions) =>
            instructions.OfType<ObstacleType>().Count();

        public static IImmutableList<InstructionType> GetGridInfo(this IImmutableList<InstructionType> instructions) => instructions
            .Take(instructions.GetGridInfoCount())
            .ToImmutableList();

        public static InstructionType GetGrid(this IImmutableList<InstructionType> instructions) =>
            instructions.Any(i => i.GetType() == typeof(GridType))
                ? instructions[0]
                : CreateArgumentError("No grid has been specified.");

        public static IImmutableList<InstructionType> GetObstacles(this IImmutableList<InstructionType> instructions) =>
            instructions
                .Where(i => i.GetType() == typeof(ObstacleType))
                .ToImmutableList();

        public static IImmutableList<InstructionType> GetJourneyInfo(this IImmutableList<InstructionType> instructions) =>
            instructions
                .Skip(instructions.GetGridInfoCount())
                .ToImmutableList();

        public static IImmutableList<ImmutableList<InstructionType>> GetJourneyGroups(this IImmutableList<InstructionType> instructions) => 
            instructions
                .GetJourneyInfo()
                .Chunk(3)
                .Select(c => c.ToImmutableList())
                .ToImmutableList();

        public static InstructionType GetFinalLocation(this IImmutableList<InstructionType> movements)
            => movements[^1];
    }
}
