namespace RobotApp.Models
{
    public record ResultType(string Message) : InstructionType
    {
        public sealed override string ToString() => Message;
    }
}
