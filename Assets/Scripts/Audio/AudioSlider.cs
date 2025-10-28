using System;
using AK.Wwise;
using UnityEngine;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] private RTPC rtpc;
    [SerializeField] private AkWwiseRTPCEventChannelSO rtpcChannel;

    public void OnSliderUpdate(Single value)
    {
        if (rtpc == null)
            return;

        rtpcChannel?.RaiseEvent((rtpc, (int)value));
    }
}