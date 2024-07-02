using System.Linq;

namespace RobotApp.Services
{
    public static class CommandsService
    {
        public static string GetNextCommands(this string commands) => commands.Any() ? commands[1..] : string.Empty;

        public static bool IsFinal(this string commands) => commands.Length <= 1;

        public static bool IsForward(this char command) => command == 'F';

        public static bool IsLeft(this char command) => command == 'L';
    }
}
