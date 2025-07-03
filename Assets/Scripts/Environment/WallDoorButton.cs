using System;
using Events.Scriptables;
using UnityEngine;

namespace Environment
{
    public class WallDoorButton : Button
    {
        [SerializeField] private GameObject door;

        protected override void Interacted(bool value)
        {
            door.SetActive(false);
        }
    }
}