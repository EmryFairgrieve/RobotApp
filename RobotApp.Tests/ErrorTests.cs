using System.Collections.Immutable;
using RobotApp.Models;
using RobotApp.Services;

namespace RobotApp.Tests
{
    public class ErrorTests
    {
        [Theory]
        [InlineData(0, false)]
        [InlineData(1, true)]
        [InlineData(2, true)]
        public void HasErrors(int errorCount, bool expected)
        {
            var result = ImmutableList.Create<InstructionType>()
                .AddRange(Enumerable.Repeat(ErrorService.Create("Error"), errorCount))
                .HasErrors();

            Assert.Equal(expected, result);
        }
    }
}
