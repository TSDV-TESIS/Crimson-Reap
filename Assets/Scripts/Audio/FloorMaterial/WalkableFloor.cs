using UnityEngine;

public class WalkableFloor : MonoBehaviour
{
    [SerializeField] private FloorMaterials material;
    public FloorMaterials Material => material;
}