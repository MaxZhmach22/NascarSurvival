namespace Octavian.Runtime.Pools.ObjectPool.ConcretePools
{
    public class ExamplePool : AbstractPool
    {
        public static ExamplePool Instance { get; private set; }

        private void OnEnable()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }
    }
}