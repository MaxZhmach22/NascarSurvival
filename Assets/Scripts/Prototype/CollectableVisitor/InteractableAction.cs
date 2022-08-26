namespace NascarSurvival.Collectable
{
    public class InteractableAction : IInteractable
    {
        public void Visit(AccelerateBonus accelerateBonus, RaceMovement raceMovement)
        {
            raceMovement.BonusSpeedEffect(accelerateBonus.Value, accelerateBonus.TimeBeforeStart, accelerateBonus.Duration);
        }
    }
}