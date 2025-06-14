using Enemy.Properties;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(SphereCollider))]
    public class EnemyListeningRadiusSetter : MonoBehaviour
    {
        [SerializeField] private EnemyGeneralProperties properties;
        
        void Start()
        {
            GetComponent<SphereCollider>().radius = properties.hearingRadius;
        }
    }
}
