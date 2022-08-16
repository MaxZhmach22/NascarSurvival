using System.Collections.Generic;
using Octavian.Runtime.Extensions;
using Octavian.Runtime.Pools.ObjectPool;
using UnityEngine;

namespace Octavian.Runtime.DestructibleBuilding.Messages
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(IDestructibleBuildingSettings))]
    public class DebrisCreator : MonoBehaviour
    {
        private readonly List<float> _fullDestructionPushForces = new List<float>();

        private IDestructibleBuildingSettings _destructibleBuildingSettings;
        
        private float _cubesMass;
        private Vector3 _cubesNumber;
        private Vector3 _cubesPivotDistance;
        private Vector3 _position;
        private Vector3 _cubesResultingSize;

        private AbstractPool _pool = null;                                                                                                                                                                                                                                      

        private void Start()
        {
            _destructibleBuildingSettings = GetComponent<IDestructibleBuildingSettings>();
            
            PrecalculateDebrisParameters(_destructibleBuildingSettings);
        }

        public void SetPool(AbstractPool pool)
        {
            _pool = pool;
        }

        public void CreateSingleDebris(int number)
        {
            for (var i = 0; i < number; i++)
            {
                var debris =  SpawnDebris();
                debris.transform.position = transform.position;

                var randomDirection = Random.insideUnitSphere;
                randomDirection.y = randomDirection.y.Abs();
                
                debris.Rigidbody.AddForce(randomDirection * _destructibleBuildingSettings.SinglePushForce.RandomFromRange, ForceMode.Impulse);
            }
        }
        
        public void BreakBuilding(Vector3 pushDirection)
        {
            _position = transform.position;
            gameObject.SetActive(false);
            
            var index = 0;
            for (var x = 0; x < _cubesNumber.x; x++)
            {
                for (var y = 0; y < _cubesNumber.y; y++)
                {
                    for (var z = 0; z < _cubesNumber.z; z++)
                    {
                        if (index % 2 != 0)
                        {
                            ++index;
                            continue;
                        }
                        var newDebris = SetDebrisPosition(SpawnDebris(), new Vector3(x, y, z));
                        newDebris.Rigidbody.AddForce(_fullDestructionPushForces[index++] * pushDirection, ForceMode.Impulse);
                    }
                }
            }
        }

        private void PrecalculateDebrisParameters(IDestructibleBuildingSettings settings)
        {
            _cubesNumber = transform.localScale.InverseScale(settings.CubeSize).Round();
            _cubesResultingSize = transform.localScale.InverseScale(_cubesNumber);
            _cubesPivotDistance = Vector3.Scale(settings.CubeSize, _cubesNumber) / 2 - settings.CubeSize / 2;
            _cubesMass = GetComponent<Rigidbody>().mass / _cubesNumber.AxisSum() / 2f;

            var cubesTotal = _cubesNumber.AxisProduct();
            for (var i = 0; i < cubesTotal; i++)
            {
                _fullDestructionPushForces.Add(settings.PushForce.RandomFromRange);
            }
        }

        private PoolObject SpawnDebris()
        {
            var debris = _pool.Get();
            debris.TurnOn();
            debris.transform.localScale = _cubesResultingSize;
            debris.Rigidbody.mass = _cubesMass;
            
            return debris;
        }

        private PoolObject SetDebrisPosition(PoolObject debris, Vector3 positionIndex)
        {
            debris.transform.position = _position + Vector3.Scale(_cubesResultingSize, positionIndex) - _cubesPivotDistance;

            return debris;
        }
    }
}

