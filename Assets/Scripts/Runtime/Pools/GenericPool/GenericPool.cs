using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Octavian.Runtime.Pools.GenericPool
{
    public class GenericPool<T> : IEnumerable
        where T : MonoBehaviour
    {
        private readonly Stack<T> _pool = new Stack<T>();
        private readonly T _prefab;
        private readonly Transform _parent;
        private readonly int emptyPoolRefillCount;

        private int _index;

        public GenericPool(int startingSize, T prefab, Transform parent = null, int refillCount = 10)
        {
            _prefab = prefab;
            _parent = parent;
            FillPool(startingSize);
            emptyPoolRefillCount = refillCount;
        }

        public IEnumerator GetEnumerator()
        {
            return _pool.GetEnumerator();
        }

        public T GetFromPool()
        {
            if (_pool.Count == 0)
            {
                FillPool(emptyPoolRefillCount);
            }

            return _pool.Pop();
        }

        public void ReturnToPool(T poolObject)
        {
            if (_pool.Contains(poolObject)) return;

            poolObject.gameObject.SetActive(false);
            _pool.Push(poolObject);
        }

        private void FillPool(int startingSize)
        {
            for (var i = 0; i < startingSize; i++)
            {
                CreatePoolObject();
            }
        }

        private void CreatePoolObject()
        {
            var newPoolObject = Object.Instantiate(_prefab, _parent, true);
            newPoolObject.gameObject.name = $"{newPoolObject.gameObject.name}: {_index++}";
            newPoolObject.gameObject.SetActive(false);
            _pool.Push(newPoolObject);
        }
    }
}