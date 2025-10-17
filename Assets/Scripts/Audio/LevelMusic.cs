using System;
using Events.Scriptables;
using Managers;
using UnityEngine;

public class LevelMusic : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event musicEvent;

    [SerializeField] private AkWwiseEventChannelSO playChannel;
    [SerializeField] private AkWwiseEventChannelSO stopChannel;

    void Start()
    {
        playChannel.RaiseEvent(musicEvent);
    }
}