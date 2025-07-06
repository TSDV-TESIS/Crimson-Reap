using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Shadow
{
    public class ShadowObjects : MonoBehaviour
    {
        [SerializeField] private GameObject deformationSystem;

        public GameObject GetDeformationSystem()
        {
            return deformationSystem;
        }
    }
}