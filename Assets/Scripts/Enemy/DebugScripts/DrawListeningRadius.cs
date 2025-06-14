using System;
using Enemy.Properties;
using UnityEngine;

namespace Enemy.DebugScripts
{
    public class DrawListeningRadius : MonoBehaviour
    {
        [SerializeField] private EnemyGeneralProperties properties;

        private void OnDrawGizmos()
        {
            if (properties.shouldDrawGizmos)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, properties.hearingRadius);
            }
        }
    }
}
