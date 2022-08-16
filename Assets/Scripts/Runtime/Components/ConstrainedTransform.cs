using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace Octavian.Runtime.Components
{
    public class ConstrainedTransform : MonoBehaviour
    {
        [SerializeField] private bool constrainPosition = true;
        [SerializeField] private bool constrainRotation = true;

        private Vector3 _positionOffset;
        private Quaternion _rotationOffset;
        private Transform _parent;

        private void Awake()
        {
            _parent = transform.parent;
            _positionOffset = transform.position - _parent.position;
            _rotationOffset = transform.rotation;
            
            Bind();
        }

        private void Bind()
        {
            if (constrainPosition)
            {
                this.UpdateAsObservable()
                    .Subscribe(_ => transform.position = _parent.position + _positionOffset)
                    .AddTo(this);
            }
            if (constrainRotation)
            {
                this.UpdateAsObservable()
                    .Subscribe(_ => transform.rotation = _rotationOffset)
                    .AddTo(this);
            }
        }
    }
}





