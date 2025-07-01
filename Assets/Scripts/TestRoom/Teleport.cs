using System;
using Player;
using UnityEngine;

namespace TestRoom
{
    public class Teleport : MonoBehaviour
    {
        [SerializeField] private TestRoomInputHandler testRoomInputHandler;
        [SerializeField] private GameObject player;
        [SerializeField] private Transform tutorialPoint;
        [SerializeField] private Transform enemyPoint;
        [SerializeField] private Transform movementPoint;
        [SerializeField] private Transform doorPoint;
        private void OnEnable()
        {
            testRoomInputHandler.onTeleportToTutorial.AddListener(HandleTeleportToTutorial);
            testRoomInputHandler.onTeleportToEnemy.AddListener(HandleTeleportToEnemy);
            testRoomInputHandler.onTeleportToMovement.AddListener(HandleTeleportToMovement);
            testRoomInputHandler.onTeleportToDoor.AddListener(HandleTeleportToDoor);
        }

        private void HandleTeleportToMovement()
        {
            SetPositionTo(movementPoint.position);
        }

        private void HandleTeleportToEnemy()
        {
            SetPositionTo(enemyPoint.position);
        }

        private void HandleTeleportToTutorial()
        {
            SetPositionTo(tutorialPoint.position);
        }

        private void HandleTeleportToDoor()
        {
            SetPositionTo(doorPoint.position);
        }

        private void SetPositionTo(Vector3 position)
        {
            CharacterController controller = player.GetComponent<CharacterController>();
            controller.enabled = false;
            player.transform.position = position;
            controller.enabled = true;
        }
    }
}
