using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeldSpawnerV2 : MonoBehaviour
{
    [Header("Weld Settings")]
    [SerializeField] private GameObject weldBeadPrefab;
    [SerializeField] private Transform rayReference;
    [SerializeField] private float maxHitDistance = 10f;
    [SerializeField] private float weldResolution = 0.1f;

    [Header("Visual Effects")]
    [SerializeField] private GameObject sparksPrefab;
    [SerializeField] private GameObject weldFlashPrefab;
    [SerializeField] private AudioSource weldAudioSource;
    [SerializeField] private AudioClip weldSound;
    [SerializeField] private Light weldLight;

    [Header("Weld Bead Chain")]
    [SerializeField] private Material weldMaterial;
    [SerializeField] private float beadWidth = 0.02f;
    [SerializeField] private float beadHeight = 0.1f;

    //private variables
    private GameObject currentBead = null;
    private List<Vector3> weldPoints = new List<Vector3>();
    private LineRenderer weldLineRenderer;
    private RaycastHit weldHit;
    private Coroutine sparkEffectCoroutine;

    private void Start()
    {
        SetupWeldLineRenderer();
        SetupWeldLight();
    }

    private void SetupWeldLight()
    {
        if (weldLight == null)
        {
            GameObject lightObj = new GameObject("WeldLight");
            lightObj.transform.SetParent(rayReference);
            weldLight = lightObj.AddComponent<Light>();
            weldLight.type = LightType.Point;
            weldLight.color = Color.cyan;
            weldLight.intensity = 2f;
            weldLight.range = 3f;
            weldLight.enabled = false;
        }
    }

    private void SetupWeldLineRenderer()
    {
        weldLineRenderer = gameObject.AddComponent<LineRenderer>();
        weldLineRenderer.material = weldMaterial;
        weldLineRenderer.startWidth = beadWidth;
        weldLineRenderer.endWidth = beadWidth;
        weldLineRenderer.positionCount = 0;
        weldLineRenderer.useWorldSpace = true;
    }

    public bool CanSpawnWeld()
    {
        RaycastHit? hitResult = GetPositionRaycastVR(rayReference , maxHitDistance);

        if(!hitResult.HasValue) return false;

        weldHit = hitResult.Value;
        Vector3 spawnPoint = weldHit.point;
        string tag = weldHit.collider.tag;

        if (tag != "weldable" && tag != "weldPoint") return false;

        if (currentBead == null) return true;

        if (weldPoints.Count == 0)
        {
            return Vector3.Distance(weldPoints[weldPoints.Count - 1], spawnPoint) > weldResolution;
        }

        return true;
    }

    public void StartWelding()
    {
        if (!CanSpawnWeld()) return;

        Vector3 spawnPoint = weldHit.point;
        Vector3 surfaceNormal = weldHit.normal;

        weldPoints.Add(spawnPoint);
        UpdateWeldLine();

        SpawnWeldBead(spawnPoint, surfaceNormal);

        StartWeldEffects(spawnPoint);

        PlayWeldSound();
    }

    public void StopWelding()
    {
        StopWeldEffects();
        StopWeldSound();
        currentBead = null;
    }

    private void SpawnWeldBead(Vector3 spawnPoint, Vector3 surfaceNormal)
    {
        if(weldBeadPrefab != null)
        {
            Quaternion rotation = Quaternion.LookRotation(surfaceNormal);
            currentBead = Instantiate(weldBeadPrefab, spawnPoint, rotation);

            currentBead.transform.localScale = new Vector3(beadWidth, beadHeight , beadWidth);
        }
    }

    private void StopWeldEffects()
    {
        if (weldLight != null)
        {
            weldLight.enabled = false;
        }

        if(sparkEffectCoroutine != null)
        {
            StopCoroutine(sparkEffectCoroutine);
            sparkEffectCoroutine = null;
        }
    }

    private void StartWeldEffects(Vector3 position)
    {
        if (sparksPrefab != null)
        {
            GameObject sparks = Instantiate(sparksPrefab, position, Quaternion.identity);
            Destroy(sparks , 2f);
        }

        if (weldFlashPrefab != null)
        {
            GameObject flash = Instantiate(weldFlashPrefab, position, Quaternion.identity);
            Destroy(flash, 0.5f);
        }

        if (weldLight !=  null)
        {
            weldLight.enabled = true;
            weldLight.transform.position = position;
        }

        if (sparkEffectCoroutine != null)
        {
            StopCoroutine(sparkEffectCoroutine);
        }
        sparkEffectCoroutine = StartCoroutine(ContinuousSparkEffect());
    }

    private IEnumerator ContinuousSparkEffect()
    {
        while (true)
        {
            if(sparksPrefab != null && weldPoints.Count > 0)
            {
                Vector3 lastPoint = weldPoints[weldPoints.Count - 1];
                GameObject sparks = Instantiate(sparksPrefab, lastPoint, Quaternion.identity);
                Destroy(sparks, 1f);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void PlayWeldSound()
    {
        if (weldAudioSource != null && weldSound != null)
        {
            weldAudioSource.clip = weldSound;
            weldAudioSource.loop = true;
            if (!weldAudioSource.isPlaying)
            {
                weldAudioSource.Play();
            }
        }
    }

    private void StopWeldSound()
    {
        if (weldAudioSource != null)
        {
            weldAudioSource.Stop();
        }
    }

    private void UpdateWeldLine()
    {
        weldLineRenderer.positionCount = weldPoints.Count;
        weldLineRenderer.SetPositions(weldPoints.ToArray());
    }

    private RaycastHit? GetPositionRaycastVR(Transform rayReference, float maxHitDistance)
    {
        RaycastHit hit;
        if (Physics.Raycast(rayReference.position , rayReference.forward, out hit , maxHitDistance))
        {
            return hit;
        }
        return null;
    }

    public void ClearWeld()
    {
        weldPoints.Clear();
        weldLineRenderer.positionCount=0;
        StopWelding();
    }

    private void OnDrawGizmos()
    {
        if(rayReference != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayReference.position, rayReference.forward * maxHitDistance);
        }

        Gizmos.color = Color.red;
        foreach(Vector3 point in weldPoints)
        {
            Gizmos.DrawSphere(point, 0.01f);
        }
    }
}
