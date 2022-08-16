using UniRx;
using UniRx.Triggers;
using UnityEngine;
using NaughtyAttributes;

namespace Octavian.Runtime.Components
{
    public class FixedTransform : MonoBehaviour
    {
        [InfoBox("Fixes transform in it's initial position and/or rotation.", EInfoBoxType.Normal)]
        [SerializeField] private bool constrainPosition = true;
        [SerializeField] private bool constrainRotation = true;

        private Vector3 _initialPosition;
        private Quaternion _initialRotation;

        private void Awake()
        {
            _initialPosition = transform.position;
            _initialRotation = transform.rotation;
        }

        private void OnEnable()
        {
            var updateObservable = this.UpdateAsObservable();
            
            if (constrainPosition)
            {
                updateObservable
                    .Subscribe(_ => transform.position = _initialPosition)
                    .AddTo(this);
            }
            if (constrainRotation)
            {
                updateObservable
                    .Subscribe(_ => transform.rotation = _initialRotation)
                    .AddTo(this);
            }
        }
    } 
}


