using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace NascarSurvival
{
    public class TestSphere : MonoBehaviour
    {
        [Inject]
        private void SetPlayer()
        {
            Debug.Log("Player");
        }
        
        public class Factory : PlaceholderFactory<TestSphere>
        {
            
        }
    }
}
