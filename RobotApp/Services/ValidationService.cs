using System;
using System.Collections.Immutable;
using System.Linq;
using RobotApp.Models;
using RobotApp.Resources;

namespace RobotApp.Services
{
    public static class ValidationService
    {
        public static IImmutableList<InstructionType> ValidateInstructions(this string[] input) => 
            input
                .ParseInstructions()
                .ValidateJourneys();

        public static IImmutableList<InstructionType> ValidateJourneys(this IImmutableList<InstructionType> instructions) =>
            instructions.HasErrors()
                ? instructions
                : instructions
                    .PerformJourneys();

        public static IImmutableList<InstructionType> PerformJourneys(this IImmutableList<InstructionType> instructions) =>
            instructions
                .GetJourneyGroups()
                .Aggregate(ImmutableList.Create<InstructionType>(), (current, journeyGroup) =>
                    current.Add(journeyGroup.PerformMovements(instructions.GetGridInfo())[0]));

        public static IImmutableList<InstructionType> PerformMovements(
            this IImmutableList<InstructionType> journeyGroup, IImmutableList<InstructionType> gridInfo) =>
            journeyGroup.Count != 3
                ? ImmutableList.Create<InstructionType>(ErrorService.Create("Invalid journey information provided."))
                : ImmutableList.Create(journeyGroup[0]).PerformMovement(journeyGroup[1].GetCommands(), journeyGroup[2], gridInfo);

        public static IImmutableList<InstructionType> PerformMovement(this IImmutableList<InstructionType> movements, string commands, InstructionType expectedFinish, IImmutableList<InstructionType> gridInfo) =>
            movements.HasErrors()
                ? movements
                : movements
                    .Add(commands[0].Move(movements[^1].GetX(), movements[^1].GetY(), movements[^1].GetDirection()))
                    .GetJourneyOutcome(gridInfo, commands, expectedFinish);

        public static IImmutableList<InstructionType> GetJourneyOutcome(this IImmutableList<InstructionType> movements,
            IImmutableList<InstructionType> gridInfo, string commands, InstructionType expectedFinish) =>
            movements.CheckForJourneyOutcome(gridInfo, commands, expectedFinish, movements.GetFinalLocation());

        public static IImmutableList<InstructionType> CheckForJourneyOutcome(this IImmutableList<InstructionType> movements, IImmutableList<InstructionType> gridInfo, string commands, InstructionType expectedFinish, InstructionType actualFinish) => gridInfo switch
        {
            _ when gridInfo.GetGrid().HasLeftGrid(actualFinish.GetX(), actualFinish.GetY()) =>
                CreateOutcomeResponse(Outcome.OutOfBounds),
            _ when gridInfo.HasCrashedIntoObstacle(actualFinish.GetX(), actualFinish.GetY()) =>
                CreateOutcomeResponse(Outcome.Crashed(actualFinish.GetX(), actualFinish.GetY())),
            _ when commands.IsFinal() => 
                CreateOutcomeResponse(Outcome.Finished(actualFinish.GetX(), actualFinish.GetY(), actualFinish.GetDirection(), expectedFinish == movements.GetFinalLocation())),
            _ => movements.PerformMovement(commands.GetNextCommands(), expectedFinish, gridInfo)
        };

        public static IImmutableList<InstructionType> CreateOutcomeResponse(string message) =>
            ImmutableList.Create<InstructionType>(ResultService.Create(message));


        public static bool HasLeftGrid(this InstructionType grid, int x, int y) =>
            x < 0
            || y < 0
            || x > grid.GetSize().width
            || y > grid.GetSize().height;

        public static bool HasCrashedIntoObstacle(this IImmutableList<InstructionType> gridInfo, int x, int y) =>
            gridInfo
                .GetObstacles()
                .Any(o => o.GetX() == x && o.GetY() == y);

        public static InstructionType Move(this char command, int x, int y, char direction) =>
            command.IsForward()
                ? direction.MoveForward(x, y)
                : direction.Turn(command, x, y);

        public static InstructionType MoveForward(this char direction, int x, int y) => direction switch
        {
            'N' => InstructionService.Create(x, y + 1, direction),
            'S' => InstructionService.Create(x, y - 1, direction),
            'W' => InstructionService.Create(x - 1, y, direction),
            'E' => InstructionService.Create(x + 1, y, direction),
            _ => InstructionService.Create(x, y, direction)
        };

        public static InstructionType Turn(this char direction, char command, int x, int y) =>
            InstructionService.Create(x, y, command.GetDirectionAfterTurn(direction));

        public static char GetDirectionAfterTurn(this char command, char direction) =>
            command.GetDirections()[GetDirectionIndex(command.GetDirections().IndexOf(direction) - 1)];

        public static string GetDirections(this char command) => command.IsLeft()
            ? "ESWN"
            : "NWSE";

        public static Index GetDirectionIndex(int index) =>
            index == -1 
                ? ^1
                : index;

        private static (int width, int height) GetSize(this InstructionType instruction) => instruction switch
        {
            GridType g => g.Size,
            _ => throw new ArgumentException("Can not get Size for this type of instruction.")
        };

        private static int GetX(this InstructionType instruction) => instruction switch
        {
            ObstacleType o => o.PositionX,
            LocationType j => j.PositionX,
            _ => throw new ArgumentException("Can not get PositionX for this type of instruction.")
        };

        private static int GetY(this InstructionType instruction) => instruction switch
        {
            ObstacleType o => o.PositionY,
            LocationType j => j.PositionY,
            _ => throw new ArgumentException("Can not get PositionY for this type of instruction.")
        };

        private static char GetDirection(this InstructionType instruction) => instruction switch
        {
            LocationType l => l.Direction,
            _ => throw new ArgumentException("Can not get Direction for this type of instruction.")
        };

        private static string GetCommands(this InstructionType instruction) => instruction switch
        {
            CommandsType c => c.Commands,
            _ => throw new ArgumentException("Can not get Commands for this type of instruction.")
        };
    }
}
