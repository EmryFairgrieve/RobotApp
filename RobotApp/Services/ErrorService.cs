using System.Collections.Immutable;
using System.Linq;
using RobotApp.Models;

namespace RobotApp.Services
{
    public static class ErrorService
    {
        public static ErrorType Create(string errorMessage) =>
            new(errorMessage);

        public static bool HasErrors(this IImmutableList<InstructionType> instructions) =>
            instructions.Any(i => i.GetType() == typeof(ErrorType));
    }
}
