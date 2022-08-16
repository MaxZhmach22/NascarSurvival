using UnityEngine;

namespace Octavian.Runtime.Extensions
{
    public static class AnimatorExtensions
    {
        public static void ResetAllTriggers(this Animator animator)
        {
            var parameters = animator.parameters;
            for (var i = 0; i < animator.parameters.Length; i++)
            {
                if (parameters[i].type == AnimatorControllerParameterType.Trigger)
                {
                    animator.ResetTrigger(parameters[i].name);
                }
            }
        }
    }
}