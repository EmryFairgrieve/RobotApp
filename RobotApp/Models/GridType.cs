namespace RobotApp.Models
{
    public record GridType((int Height, int Width) Size) : InstructionType;
}
