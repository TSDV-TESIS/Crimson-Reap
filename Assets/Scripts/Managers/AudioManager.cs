using System;
using AK.Wwise;
using Events;
using Events.Scriptables;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AK.Wwise.Event stopAllSoundEvent;

        [Header("Events")]
        [SerializeField] private AkWwiseEventChannelSO onPlayEvent;
        [SerializeField] private AkWwiseEventChannelSO onStopEvent;
        [SerializeField] private AkWwiseEventWithCallbackChannelSO onPlayEventWithCallback;
        [SerializeField] private AkWwiseSwitchEventChannelSO onSwitch;
        [SerializeField] private AkWwiseRTPCEventChannelSO onRTPC;

        public void OnEnable()
        {
            onPlayEvent?.onTypedEvent.AddListener(PlayEvent);
            onPlayEventWithCallback?.onTypedEvent.AddListener(PlayEvent);
            onStopEvent?.onTypedEvent.AddListener(StopEvent);
            onSwitch?.onTypedEvent.AddListener(ToggleSwitch);
            onRTPC?.onTypedEvent.AddListener(SetRTPC);
        }

        public void OnDisable()
        {
            onPlayEvent?.onTypedEvent.RemoveListener(PlayEvent);
            onPlayEventWithCallback?.onTypedEvent.RemoveListener(PlayEvent);
            onStopEvent?.onTypedEvent.RemoveListener(StopEvent);
            stopAllSoundEvent?.Post(gameObject);
        }

        private void StopEvent(AK.Wwise.Event anEvent)
        {
            anEvent.Stop(gameObject);
        }

        private void PlayEvent(AK.Wwise.Event anEvent)
        {
            anEvent.Post(gameObject);
        }

        private void PlayEvent(AK.Wwise.Event anEvent, AkCallbackManager.EventCallback callback)
        {
            anEvent.Post(gameObject,
            (uint)(AkCallbackType.AK_MusicSyncAll | AkCallbackType.AK_EnableGetMusicPlayPosition), callback);
        }

        private void ToggleSwitch(AK.Wwise.Switch inSwitch)
        {
            inSwitch.SetValue(gameObject);
        }

        private void SetRTPC((RTPC rtpc, int value) tuple)
        {
            tuple.rtpc.SetValue(gameObject, tuple.value);
        }
    }
}