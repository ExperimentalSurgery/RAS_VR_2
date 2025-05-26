using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    [Header("OVRPassthroughLayer")]
    [SerializeField] private OVRPassthroughLayer oVRPassthroughLayer;
    [SerializeField] float timeForToggleEnabling = 1f;
    [SerializeField] bool canToggle = true;
    [SerializeField]
    GameObject[] VRObjects; // Objects that should be enabled in VR mode
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
        InitializePassthroughMode();
    }

    private void Start()
    {
        cameras = FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None);
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

    public void ToggleGameMode()
    {
        if (!canToggle)
        {
            Debug.Log("Cannot toggle game mode yet.");
            return;
        }
        StartCoroutine(StatTimerForToggling());
        isVirtualReality = !isVirtualReality;
        if (isVirtualReality)
        {
            oVRPassthroughLayer.enabled = false;
            rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = 0;
            leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = 0;
            SwapMaterial(true);
            StartCoroutine(EnableQuickVibration());
            foreach (GameObject vrObject in VRObjects)
            {
                vrObject.SetActive(true);
            }

        }
        else
        {
            oVRPassthroughLayer.enabled = true;
            rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~0;
            rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform");
            leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~0;
            leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform");
            SwapMaterial(false);
            foreach (GameObject vrObject in VRObjects)
            {
                vrObject.SetActive(false);
            }
        }
    }

    void InitializePassthroughMode()
    {
        // This method can be used to initialize the passthrough mode if needed
        isVirtualReality = false;
        oVRPassthroughLayer.enabled = true;
        oVRPassthroughLayer.passthroughLayerResumed.AddListener(OnPassthroughLayerResumed);
        rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~0;
        rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform");
        leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~0;
        leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform");
        SwapMaterial(false);
        foreach (GameObject vrObject in VRObjects)
        {
            vrObject.SetActive(false);
        }

    }

    public void SwapMaterial(bool isOn)
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
            yield return new WaitForSeconds(1f);
            timeForToggleEnabling -= 1f;
        }
        canToggle = true;
        // Add any logic here that should run after the timer ends
    }


}
