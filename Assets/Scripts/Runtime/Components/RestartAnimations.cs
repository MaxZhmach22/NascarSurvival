using DG.Tweening;
using UnityEngine;

namespace Octavian.Runtime.Components
{
    public class RestartAnimations : MonoBehaviour
    {
        private DOTweenAnimation _doTweenAnimation;

        private void Awake()
        {
            _doTweenAnimation = GetComponent<DOTweenAnimation>();
        }

        private void OnEnable()
        {
            _doTweenAnimation.DORewind();
        }

        private void Update()
        {
            _doTweenAnimation.DOPlay();
        }
    }
}
