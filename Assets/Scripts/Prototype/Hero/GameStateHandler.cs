using System;

namespace NascarSurvival
{
    public class GameStateHandler
    {
        public static int Counter;
        public Action<GameStates> OnChangeState { get; set; }
        public GameStates CurrentGameState { get; private set; }

        public GameStateHandler()
        {
            Counter++;
        }
        
        public void ChangeState(GameStates state)
        {
            CurrentGameState = state;
            OnChangeState?.Invoke(state);
        }
    }
}