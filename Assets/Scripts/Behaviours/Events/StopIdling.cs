using System;
using Unity.Behavior;
using UnityEngine;
using Unity.Properties;

#if UNITY_EDITOR
[CreateAssetMenu(menuName = "Behavior/Event Channels/StopIdling")]
#endif
[Serializable, GeneratePropertyBag]
[EventChannelDescription(name: "StopIdling", message: "Stopped Idling", category: "Events", id: "1ad869b5e26f994308b754cb73513027")]
public sealed partial class StopIdling : EventChannel { }

