namespace NascarSurvival.Collectable
{
    public class InteractableAction : IInteractable
    {
        public void Visit(AccelerateBonus accelerateBonus, RaceMovement raceMovement)
        {
            raceMovement.BonusSpeedEffect(accelerateBonus.Value, accelerateBonus.TimeBeforeStart, accelerateBonus.Duration);
        }
        
        public void Visit(DeccelerateBonus deccelerateBonus, RaceMovement raceMovement)
        {
            raceMovement.BonusSpeedEffect(-deccelerateBonus.Value, deccelerateBonus.TimeBeforeStart, deccelerateBonus.Duration);
        }
        
        public void Visit(BombBonus bombBonus, RaceMovement raceMovement)
        {
            raceMovement.BoomBonus(bombBonus.transform.position);
        }
    }
}