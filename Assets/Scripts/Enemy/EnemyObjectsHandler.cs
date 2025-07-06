using UnityEngine;

namespace Enemy
{
    public class EnemyObjectsHandler : MonoBehaviour
    {
        [SerializeField] private GameObject listeningObject;
        [SerializeField] private GameObject knightAttackObject;
        [SerializeField] private GameObject knightWindupObject;
        [SerializeField] private GameObject attackVfxObject;
        [SerializeField] private GameObject archerBowObject;
        [SerializeField] private Animator knightAnimator;
        
        public GameObject GetListeningObject()
        {
            return listeningObject;
        }
    
        public GameObject GetKnightAttackObject()
        {
            return knightAttackObject;
        }
    
        public GameObject GetKnightWindupObject()
        {
            return knightWindupObject;
        }

        public GameObject GetArcherBowObject()
        {
            return archerBowObject;
        }

        public GameObject GetAttackVfxObject()
        {
            return attackVfxObject;
        }

        public Animator GetKnightAnimator()
        {
            return knightAnimator;
        }
    }
}
