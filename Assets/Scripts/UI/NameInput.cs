using System;
using Events.Scriptables;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameInput : MonoBehaviour
{
    [SerializeField] private StringEventChannelSO onLoadSceneHandler;
    [SerializeField] private PlayerName playerName;
    [SerializeField] private string firstLevelName;

    private TMP_InputField _inputField;

    private void OnEnable()
    {
        _inputField ??= GetComponent<TMP_InputField>();
    }

    public void OnInputFieldEndEdit(string input)
    {
        if (input.Length == 0)
            return;

        playerName.playerName = _inputField.text;
        onLoadSceneHandler?.RaiseEvent(firstLevelName);
    }
}