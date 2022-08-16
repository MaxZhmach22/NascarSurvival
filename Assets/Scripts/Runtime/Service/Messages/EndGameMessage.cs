using Octavian.Runtime.GeneralUseInterfaces;

namespace Octavian.Runtime.Service.Messages
{
    public struct EndGameMessage : IMessage
    {
        public EndGameMessage(bool isWin)
        {
            IsWin = isWin;
        }

        public bool IsWin { get; }
    }
}