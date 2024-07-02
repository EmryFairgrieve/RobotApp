namespace RobotApp.Models
{
    public record ErrorType(string Message) : InstructionType
    {
        public sealed override string ToString() => Message;
    }
}
