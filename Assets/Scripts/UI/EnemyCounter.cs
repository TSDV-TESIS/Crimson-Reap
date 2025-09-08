using Events;
using TMPro;
using UnityEngine;

namespace UI
{
    public class EnemyCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI counterText;
        [SerializeField] private IntEventChannelSO onEnemyCountUpdate;

        private void OnEnable()
        {
            onEnemyCountUpdate.onIntEvent.AddListener(UpdateEnemyCount);
        }

        private void OnDisable()
        {
            onEnemyCountUpdate.onIntEvent.RemoveListener(UpdateEnemyCount);
        }

        private void UpdateEnemyCount(int value)
        {
            counterText.text = value.ToString();
        }
    }
}