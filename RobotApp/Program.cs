using static RobotApp.Services.ValidationService;
using static System.Console;

namespace RobotApp
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var result in args.ValidateInstructions())
            {
                WriteLine(result.ToString());
            }
        }
    }
}
