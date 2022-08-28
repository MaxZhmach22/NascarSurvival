namespace NascarSurvival
{
    public interface IInitializer
    {
        RaceMovement RaceMovement { get; }
        string Name { get; }
    }
}