namespace RobotApp.Models
{
    public record LocationType(int PositionX, int PositionY, char Direction) : InstructionType;
}
