using RobotApp.Models;

namespace RobotApp.Services;

public static class ResultService
{
    public static ResultType Create(string result) => new(result);
}