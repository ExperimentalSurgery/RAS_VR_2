using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    [Header("OVRPassthroughLayer")]
    [SerializeField] private OVRPassthroughLayer oVRPassthroughLayer;
    [SerializeField] float timeForToggleEnabling = 1f;
    [SerializeField] bool canToggle = true;

    [Header("Material Settings")]
    [SerializeField] Material glowMaterial;
    [SerializeField] Material defaultMat;
    [SerializeField] Renderer rightStylus;
    [SerializeField] Renderer leftStylus;

    [Header("Styluses Settings")]
    [SerializeField] Collider rightStylusCollider;
    [SerializeField] Collider leftStylusCollider;
    [SerializeField] bool isVirtualReality = false;

    [Header("Vibration Settings")]
    [SerializeField] HapticPlugin hapticPluginR;
    [SerializeField] HapticPlugin hapticPluginL;

    [Header("Cameras")]
    [SerializeField] Camera[] cameras;

    private void Awake()
    {
        oVRPassthroughLayer.passthroughLayerResumed.AddListener(OnPassthroughLayerResumed);
        // 1) We enable the passthrough layer to kick off its initialization process
        oVRPassthroughLayer.enabled = true;
    }



    private void OnDestroy()
    {
        oVRPassthroughLayer.passthroughLayerResumed.RemoveListener(OnPassthroughLayerResumed);
    }

    // 2) OnPassthroughLayerResumed is called once the layer is fully initialized and passthrough is visible
    private void OnPassthroughLayerResumed(OVRPassthroughLayer passthroughLayer)
    {
        // 3) Do something here after the passthrough layer has resumed
    }

    private void Start()
    {
        isVirtualReality = false;
        // rightStylusCollider.enabled = false;
        //  leftStylusCollider.enabled = false;
        cameras = FindObjectsByType<Camera>(FindObjectsInactive.Include,FindObjectsSortMode.None);
    }
    public void ToggleGameMode(bool isRightStylus)
    {
        StartCoroutine(StatTimerForToggling());
        if (!canToggle)
        {
            Debug.Log("Cannot toggle game mode yet.");
            return;
        }
        isVirtualReality = !isVirtualReality;
        if (isVirtualReality)
        {
            oVRPassthroughLayer.enabled = false;
            rightStylusCollider.enabled = true;
            leftStylusCollider.enabled = true;
            SwapMaterial(true, isRightStylus);
            StartCoroutine(EnableQuickVibration());

        }
        else
        {
            oVRPassthroughLayer.enabled = true;
            rightStylusCollider.enabled = false;
            leftStylusCollider.enabled = false;
            SwapMaterial(false, isRightStylus);
        }
    }

    public void SwapMaterial(bool isOn, bool isRightStylus)
    {
        if (isOn)
        {
            rightStylus.material = glowMaterial;
            leftStylus.material = glowMaterial;
        }
        else
        {
            rightStylus.material = defaultMat;
            leftStylus.material = defaultMat;
        }
    }

    IEnumerator EnableQuickVibration()
    {
        hapticPluginR.EnableVibration();
        hapticPluginL.EnableVibration();
        yield return new WaitForSeconds(.5f);
        hapticPluginR.DisableVibration();
        hapticPluginL.DisableVibration();
    }

    IEnumerator StatTimerForToggling()
    {
        canToggle = false;
        while (timeForToggleEnabling > 0)
        {
            Debug.Log($"Timer running: {timeForToggleEnabling} seconds remaining");
            yield return new WaitForSeconds(1f);
            timeForToggleEnabling -= 1f;
        }
        canToggle = true;
        Debug.Log("Timer finished!");
        // Add any logic here that should run after the timer ends
    }
}
