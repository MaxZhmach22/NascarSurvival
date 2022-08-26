namespace NascarSurvival.Collectable
{
    public interface IInteractable
    {
        void Visit(AccelerateBonus accelerateBonus, RaceMovement raceMovement);
    }
}