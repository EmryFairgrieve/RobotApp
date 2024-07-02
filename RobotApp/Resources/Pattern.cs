using System.Text.RegularExpressions;

namespace RobotApp.Resources
{
    public static class Pattern
    {
        public static Regex Grid => GetRegex(@"^GRID (?<x>\d+)x(?<y>\d+$)");
        public static Regex Obstacle => GetRegex(@"^OBSTACLE (?<x>\d+) (?<y>\d+$)");
        public static Regex Location => GetRegex(@"(?<x>^\d+) (?<y>\d+) (?<d>[SEWN]$)");
        public static Regex Commands => GetRegex("(?<c>^[LRF]+$)");
        private static Regex GetRegex(string input) => new(input);
    }
}
