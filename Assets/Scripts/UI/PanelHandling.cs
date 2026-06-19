using System;
using Events.Scriptables;
using Player;
using UnityEngine;

public class PanelHandling : MonoBehaviour
{
    [SerializeField] private InputHandler input;

    [SerializeField] private GameObjectEventChannelSO onNewSelectedObject;
    [SerializeField] private GameObject startingObject;
    [SerializeField] private GameObject defaultObject;

    private GameObject activePanel = null;

    private void OnEnable()
    {
        input.onCancel.AddListener(HandleCancel);
    }

    private void Start()
    {
        onNewSelectedObject?.RaiseEvent(startingObject);
    }

    private void HandleCancel()
    {
        if (activePanel == null)
            return;

        Debug.Log("EscapePressed");
        activePanel.SetActive(false);
        onNewSelectedObject?.RaiseEvent(defaultObject);
        activePanel = null;
    }

    public void SetPanel(GameObject activePanel)
    {
        this.activePanel = activePanel;
    }

    public void RemovePanel()
    {
        activePanel = null;
    }
}