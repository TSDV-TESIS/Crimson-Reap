using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;

namespace Utils
{
    public class BlockMenuManager : MonoBehaviour
    {
        [MenuItem("Blockings/Hide colliders")]
        static void HideColliders()
        {
            BlockColliderHider[] colliders = FindObjectsByType<BlockColliderHider>(FindObjectsSortMode.None);

            foreach (BlockColliderHider blockColliderHider in colliders)
            {
                blockColliderHider.Hide();
            }
        }

        [MenuItem("Blockings/Show colliders")]
        static void ShowColliders()
        {
            BlockColliderHider[] colliders = FindObjectsByType<BlockColliderHider>(FindObjectsSortMode.None);

            foreach (BlockColliderHider blockColliderHider in colliders)
            {
                blockColliderHider.Show();
            }
        }
    }
}
