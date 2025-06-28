using UnityEngine;

public class EnemyObjectsHandler : MonoBehaviour
{
    [SerializeField] private GameObject listeningObject;
    [SerializeField] private GameObject knightAttackObject;
    [SerializeField] private GameObject knightWindupObject;
    [SerializeField] private GameObject archerBowObject;
    
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
}
