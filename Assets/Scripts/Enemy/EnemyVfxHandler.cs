using UnityEngine;
using UnityEngine.VFX;

namespace Enemy
{
    public class EnemyVfxHandler : MonoBehaviour
    {
        [SerializeField] private VisualEffect deathExplosion;

        public void OnDead()
        {
            deathExplosion.SendEvent("explosion");
        }
    }
}
