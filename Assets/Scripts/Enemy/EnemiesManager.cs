using System;
using System.Collections.Generic;
using Events;
using Events.Scriptables;
using UnityEngine;

namespace Enemy
{
    public class EnemiesManager : MonoBehaviour
    {
        [Header("Events")] [SerializeField] private GameObjectEventChannelSO onEnemyEnabled;
        [SerializeField] private GameObjectEventChannelSO onEnemyDisabled;
        [SerializeField] private VoidEventChannelSO onAllEnemiesDisabled;
        [SerializeField] private IntEventChannelSO onEnemyCountUpdate;

        private List<GameObject> _enemies;

        private void OnEnable()
        {
            _enemies = new List<GameObject>();
            onEnemyEnabled?.onTypedEvent.AddListener(HandleNewEnemy);
            onEnemyDisabled?.onTypedEvent.AddListener(HandleEnemyDisabled);
        }

        private void OnDisable()
        {
            onEnemyEnabled?.onTypedEvent.RemoveListener(HandleNewEnemy);
            onEnemyDisabled?.onTypedEvent.RemoveListener(HandleEnemyDisabled);
        }

        private void HandleEnemyDisabled(GameObject enemy)
        {
            _enemies.Remove(enemy);
            onEnemyCountUpdate?.RaiseEvent(_enemies.Count);
            if (_enemies.Count == 0)
            {
                onAllEnemiesDisabled?.RaiseEvent();
            }
        }

        private void HandleNewEnemy(GameObject enemy)
        {
            _enemies.Add(enemy);
            onEnemyCountUpdate?.RaiseEvent(_enemies.Count);
        }
    }
}