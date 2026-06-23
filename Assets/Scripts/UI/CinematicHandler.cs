using System;
using Events;
using Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(RawImage))]
public class CinematicHandler : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO onCinematicEnd;
    [SerializeField] private PlayerName playerName;
    private VideoPlayer videoPlayer;
    private RawImage rawImage;

    private void OnEnable()
    {
        videoPlayer ??= GetComponent<VideoPlayer>();
        rawImage ??= GetComponent<RawImage>();
        videoPlayer.loopPointReached += OnCinematicNaturalFinish;
    }

    private void OnDisable()
    {
        videoPlayer.loopPointReached -= OnCinematicNaturalFinish;
    }

    public void Play()
    {
        rawImage.enabled = true;
        videoPlayer.Play();
    }

    public void OnCinematicSkip()
    {
        onCinematicEnd?.RaiseEvent();
        //playerName.isInitialized = true;
    }

    private void OnCinematicNaturalFinish(VideoPlayer source)
    {
        onCinematicEnd?.RaiseEvent();
    }
}