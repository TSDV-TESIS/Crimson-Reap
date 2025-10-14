using Events.Scriptables;
using Player;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayHandler : MonoBehaviour
    {
        [SerializeField] private StringEventChannelSO onLoadSceneHandler;
        [SerializeField] private PlayerName playerName;
        [SerializeField] private string firstLevelName;
        [SerializeField] private TMP_InputField inputField;

        public void OnClick()
        {
            if (inputField.text != "")
            {
                playerName.playerName = inputField.text;
                onLoadSceneHandler?.RaiseEvent(firstLevelName);
            }
        }
    }
}
