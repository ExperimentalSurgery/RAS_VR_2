using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameModeManager : MonoBehaviour
{
    [SerializeField] bool isStiffnessSetting = false; // Flag to check if we are in stiffness setting mode
    public static GameModeManager Instance { get; private set; }

    [Header("Passthrough mode settings")]
    [SerializeField] private OVRPassthroughLayer oVRPassthroughLayer;
    [SerializeField] float timeForToggleEnabling = 1f;
    [SerializeField] bool canToggle = true;
    [SerializeField] bool isHapticFeedbackEnabled = false;
    [Header("Game Objects Settings")]
    [SerializeField] Transform controllerTip;
    [SerializeField] MeshRenderer[] stylusesRenders; // Objects that should be enabled in the menu panel
    [SerializeField] GameObject[] VRObjects; // Objects that should be enabled in VR mode
    [SerializeField] GameObject[] MRObjects; // Objects that should be enabled in MR mode
    public bool IsCalibrationMode { get; private set; } = false;
    [Header("Material Settings")]
    [SerializeField] Material glowMaterial;
    [SerializeField] Material defaultMat;
    [SerializeField] Renderer rightStylus;
    [SerializeField] Renderer leftStylus;

    [Header("Styluses Settings")]
    [SerializeField] Collider rightStylusCollider;
    [SerializeField] Collider leftStylusCollider;
    public bool isVirtualReality = false;

    [Header("Vibration Settings")]
    [SerializeField] HapticPlugin hapticPluginR;
    [SerializeField] HapticPlugin hapticPluginL;

    public event Action<bool> onHapticEnabled;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        oVRPassthroughLayer.passthroughLayerResumed.AddListener(OnPassthroughLayerResumed);
        IsCalibrationMode = false;
    }

    private void Start()
    {
        // Initialize the passthrough mode when the script is enabled
        if (isStiffnessSetting)
        {
            oVRPassthroughLayer.textureOpacity = 0f;
            InitializePassthroughMode();
        }
        else
        {
            oVRPassthroughLayer.textureOpacity = 1f;
            StartSimulationMode();
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCalibrationMode();
        }
    }



    private void OnDisable()
    {
        InitializePassthroughMode();
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
            oVRPassthroughLayer.textureOpacity = 0f; // Set the opacity to 0 to hide the passthrough layer

            if (isHapticFeedbackEnabled)
            {
                rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = 0;
                leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = 0;
                rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = LayerMask.GetMask("FatTissue");
                leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = LayerMask.GetMask("FatTissue");
                SwapMaterial(true);
                StartCoroutine(EnableQuickVibration());
            }


            foreach (GameObject mrObject in MRObjects)
            {
                mrObject.SetActive(false);
            }

            controllerTip.gameObject.SetActive(false);

            foreach (MeshRenderer stylusRenderer in stylusesRenders)
            {
                stylusRenderer.enabled = true;
            }
            foreach (GameObject vrObject in VRObjects)
            {
                vrObject.SetActive(true);
            }

        }
        else
        {
            oVRPassthroughLayer.enabled = true;
            oVRPassthroughLayer.textureOpacity = 1f;
            rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform", "FatTissue");
            leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform", "FatTissue");
            SwapMaterial(false);
            foreach (GameObject vrObject in VRObjects)
            {
                vrObject.SetActive(false);
            }
            foreach (GameObject mrObject in MRObjects)
            {
                if (IsCalibrationMode)
                {
                    mrObject.SetActive(true);
                }
                else
                {
                    mrObject.SetActive(false);
                }

            }

            foreach (MeshRenderer stylusRenderer in stylusesRenders)
            {
                if (IsCalibrationMode)
                {
                    stylusRenderer.enabled = true;
                }
                else
                {
                    stylusRenderer.enabled = false;
                }
            }

            if (IsCalibrationMode)
            {
                controllerTip.gameObject.SetActive(true);
            }
            else
            {
                controllerTip.gameObject.SetActive(false);
            }
        }
    }

    public void ToggleHapticFeedback()
    {
        if (isVirtualReality)
        {
            isHapticFeedbackEnabled = !isHapticFeedbackEnabled;
            if (isHapticFeedbackEnabled)
            {
                rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = 0;
                leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = 0;
                rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = LayerMask.GetMask("FatTissue");
                leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = LayerMask.GetMask("FatTissue");
                SwapMaterial(true);
                StartCoroutine(EnableQuickVibration());
                onHapticEnabled?.Invoke(true);

            }
            else
            {
                rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform", "FatTissue");
                leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform", "FatTissue");
                SwapMaterial(false);
                onHapticEnabled?.Invoke(false);
            }
            Debug.Log("Haptic feedback toggled: " + isHapticFeedbackEnabled);
        }
        else
        {
            Debug.LogWarning("Haptic feedback is only available in Virtual Reality mode.");
        }
    }

    void InitializePassthroughMode()
    {
        Debug.Log("Initializing Passthrough Mode...");
        if (oVRPassthroughLayer == null)
        {
            Debug.LogError("OVRPassthroughLayer reference is missing!");
            return;
        }
        // This method can be used to initialize the passthrough mode if needed
        isVirtualReality = false;
        IsCalibrationMode = false;
        oVRPassthroughLayer.enabled = true;

        if (rightStylusCollider != null) { rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform", "FatTissue"); }

        if (leftStylusCollider != null) { leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform", "FatTissue"); }
        SwapMaterial(false);
        foreach (GameObject vrObject in VRObjects)
        {
            vrObject.SetActive(false);
        }
        foreach (GameObject mrObject in MRObjects)
        {
            mrObject.SetActive(false);
        }
        foreach (MeshRenderer stylusRenderer in stylusesRenders)
        {
            stylusRenderer.enabled = false;
        }
        controllerTip.gameObject.SetActive(false); // Hide the controller tip in main menu at the start

        oVRPassthroughLayer.textureOpacity = 1f; // set the opacity to 0 to hide the passthrough layer


    }

    public void StartSimulationMode()
    {
        IsCalibrationMode = false;
        oVRPassthroughLayer.textureOpacity = 1f;

        foreach (GameObject mrObject in MRObjects)
        {
            mrObject.SetActive(false);
        }
        controllerTip.gameObject.SetActive(false);
    }
    public void StartCalibrationMode()
    {
        IsCalibrationMode = true;
        oVRPassthroughLayer.textureOpacity = 1f;

        foreach (GameObject mrObject in MRObjects)
        {
            mrObject.SetActive(true);
        }
        foreach (MeshRenderer stylusRenderer in stylusesRenders)
        {
            stylusRenderer.enabled = true;
        }
        controllerTip.gameObject.SetActive(true);
    }

    public void ResetSimulation()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void EndSimulationMode()
    {
        Application.Quit();
    }

    public void RestartSimulation()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0);
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
