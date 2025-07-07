using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Health;
using Player.Shadow;
using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(HealthPoints))]
    public class ShadowStep : MonoBehaviour
    {
        [Header("Shadow")]
        [SerializeField] private GameObject shadowPrefab;
        [SerializeField] private float spawnRate;
        [SerializeField] private float fadeOutSeconds = 0.2f;
        [SerializeField] private Material materialToUse;
        [SerializeField] private GameObject deformationSystem;
        [SerializeField] private GameObject model;
        
        private List<GameObject> _shadows;
        private Coroutine _spawnShadows;
        private bool _isShadow;

        void OnEnable()
        {
            _shadows ??= new List<GameObject>();
        }

        private void OnDisable()
        {
            StopShadows();
            ClearShadows();
        }

        public void InitShadowStepShadows()
        {
            _isShadow = true;
            if (_spawnShadows != null)
                StopCoroutine(_spawnShadows);
            _spawnShadows = StartCoroutine(SpawnShadows());
        }

        public void StopShadows()
        {
            _isShadow = false;
        }

        private int CompareName(Transform a, Transform b)
        {
            return String.Compare(a.name, b.name, StringComparison.Ordinal);
        }

        private IEnumerator SpawnShadows()
        {
            ClearShadows();
            while (_isShadow)
            {
                yield return new WaitForSeconds(spawnRate);

                GameObject shadow = Instantiate(shadowPrefab, transform.position, transform.rotation);
                SetRotationToModel(shadow);
                foreach (SkinnedMeshRenderer meshRenderer in shadow.GetComponentsInChildren<SkinnedMeshRenderer>())
                {
                    meshRenderer.material = materialToUse;
                }
                _shadows.Add(shadow);
                StartCoroutine(FadeOut(shadow));
            }
        }

        private void SetRotationToModel(GameObject shadow)
        {
            Vector3 angles = shadow.transform.rotation.eulerAngles;
            angles.y = model.transform.rotation.eulerAngles.y;
            Quaternion rotation = shadow.transform.rotation;
            rotation.eulerAngles = angles;
            shadow.transform.rotation = rotation;
        }

        private IEnumerator FadeOut(GameObject afterImage)
        {
            CopyBones(afterImage);
            Material oldMat = afterImage.GetComponentInChildren<SkinnedMeshRenderer>().material;
            Material diffuseMaterial = Instantiate(oldMat);
            float elapsedTime = 0f;
            Color initialColor = diffuseMaterial.color;
            SkinnedMeshRenderer[] meshRenderers = afterImage.GetComponentsInChildren<SkinnedMeshRenderer>();
            while (elapsedTime < fadeOutSeconds)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(initialColor.a, 0f, elapsedTime / fadeOutSeconds);
                diffuseMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
                foreach (SkinnedMeshRenderer meshRenderer in meshRenderers)
                {
                    meshRenderer.material = diffuseMaterial;
                }

                yield return null;
            }

            foreach (SkinnedMeshRenderer meshRenderer in meshRenderers)
            {
                meshRenderer.material = oldMat;
            }

            _shadows.Remove(afterImage);
            Destroy(afterImage);
            Destroy(diffuseMaterial);
        }

        private void CopyBones(GameObject afterImage)
        {
            ShadowObjects objects = afterImage.GetComponent<ShadowObjects>();
            GameObject shadowBonesRoot = objects.GetDeformationSystem();

            Transform[] shadowBones = shadowBonesRoot.GetComponentsInChildren<Transform>();
            Transform[] actualBones = deformationSystem.GetComponentsInChildren<Transform>();
            
            Array.Sort(shadowBones, CompareName);
            Array.Sort(actualBones, CompareName);

            for (int i = 0; i < shadowBones.Length; i++)
            {
                shadowBones[i].localPosition = actualBones[i].localPosition;
                shadowBones[i].localRotation = actualBones[i].localRotation;
                shadowBones[i].localScale = actualBones[i].localScale;
            }
        }

        private void ClearShadows()
        {
            if (_shadows == null)
                return;

            for (int i = 0; i < _shadows.Count; i++)
            {
                Destroy(_shadows[i]);
            }

            _shadows.Clear();
        }
    }
}