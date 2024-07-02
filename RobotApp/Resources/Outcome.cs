namespace RobotApp.Resources
{
    public static class Outcome
    {
        public static string Finished(int x, int y, char direction, bool success) => $"{Result(success)} {x} {y} {direction}";
        public static string OutOfBounds => "OUT OF BOUNDS";
        public static string Crashed(int x, int y) => $"CRASHED {x} {y}";
        private static string Result(bool success) => success ? "SUCCESS" : "FAILURE";
    }
}
