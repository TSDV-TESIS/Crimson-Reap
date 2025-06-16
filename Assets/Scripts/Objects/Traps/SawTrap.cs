using UnityEngine;

namespace Objects.Traps
{
    public class SawTrap : BaseTrap
    {
        [SerializeField] private GameObject saw;
        [SerializeField] private GameObject WaypointsParent;

        [Tooltip("If Loops, last waypoint returns back to first waypoint, otherwise it starts moving backwards per each waypoint")]
        [SerializeField] private bool shouldLoop;

    }
}