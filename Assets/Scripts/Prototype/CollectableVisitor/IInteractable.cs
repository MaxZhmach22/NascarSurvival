namespace NascarSurvival.Collectable
{
    public interface IInteractable
    {
        void Visit(AccelerateBonus accelerateBonus, RaceMovement raceMovement);
        void Visit(DeccelerateBonus accelerateBonus, RaceMovement raceMovement);
        void Visit(BombBonus bombBonus, RaceMovement raceMovement);
    }
}