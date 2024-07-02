using RobotApp.Services;

namespace RobotApp.Tests
{
    public class CommandsServiceTests
    {
        [Theory]
        [InlineData("LRF", "RF")]
        [InlineData("L", "")]
        [InlineData("", "")]
        public void GetNextCommands_GivenCommands_ReturnsRemainingCommands(string commands, string expected)
        {
            var result = commands.GetNextCommands();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("LRF", false)]
        [InlineData("L", true)]
        [InlineData("", true)]
        public void IsFinal_GivenCommands_ReturnsIfFinalCommand(string commands, bool expected)
        {
            var result = commands.IsFinal();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData('F', true)]
        [InlineData('L', false)]
        [InlineData('R', false)]
        public void IsForward_GivenCommands_ReturnsIfCommandIsForward(char command, bool expected)
        {
            var result = command.IsForward();

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData('F', false)]
        [InlineData('L', true)]
        [InlineData('R', false)]
        public void IsLeft_GivenCommands_ReturnsIfCommandIsLeft(char command, bool expected)
        {
            var result = command.IsLeft();

            Assert.Equal(expected, result);
        }
    }
}
