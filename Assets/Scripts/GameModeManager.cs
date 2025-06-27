using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; }
    [Header("UI")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private TMP_Text titleGamePanel;

    [Header("Passthrough mode settings")]
    [SerializeField] private OVRPassthroughLayer oVRPassthroughLayer;
    [SerializeField] private OVRScreenFade oVRScreenFade;
    [SerializeField] float timeForToggleEnabling = 1f;
    [SerializeField] bool canToggle = true;
    [SerializeField] bool isHapticFeedbackEnabled = false;
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

    [Header("Cameras")]
    [SerializeField] Camera[] cameras;

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
        InitializePassthroughMode();
    }

    private void Start()
    {
        cameras = FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCalibrationMode();
        }
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
            oVRPassthroughLayer.textureOpacity = 0f; // Set the opacity to 0 to hide the passthrough layer

            if (isHapticFeedbackEnabled)
            {
                rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = 0;
                leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = 0;
                SwapMaterial(true);
                StartCoroutine(EnableQuickVibration());
            }
            foreach (GameObject vrObject in VRObjects)
            {
                vrObject.SetActive(true);
            }

        foreach(GameObject mrObject in MRObjects)
            {
                mrObject.SetActive(false);
            }

        }
        else
        {
            oVRPassthroughLayer.enabled = true;
            oVRPassthroughLayer.textureOpacity = 1f;
            rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~0;
            //rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform");
            leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~0;
            //leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform");
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
                SwapMaterial(true);
                StartCoroutine(EnableQuickVibration());
            }
            else
            {
                rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~0;
                //rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform");
                leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~0;
                //leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform");
                SwapMaterial(false);
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
        // This method can be used to initialize the passthrough mode if needed
        isVirtualReality = false;
        IsCalibrationMode = false;
        // Set the initial state of UI panels and passthrough layer
        menuPanel.SetActive(true);
        gamePanel.SetActive(false);
        oVRPassthroughLayer.enabled = true;
        oVRPassthroughLayer.passthroughLayerResumed.AddListener(OnPassthroughLayerResumed);
        rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~0;
        //rightStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform");
        leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~0;
        //leftStylusCollider.gameObject.GetComponent<Rigidbody>().excludeLayers = ~LayerMask.GetMask("Deform");
        SwapMaterial(false);
        foreach (GameObject vrObject in VRObjects)
        {
            vrObject.SetActive(false);
        }
        foreach (GameObject mrObject in MRObjects)
        {
            mrObject.SetActive(false);
        }
        oVRPassthroughLayer.textureOpacity = 0f; // // Hide the passthrough layer


    }

    public void StartSimulationMode()
    {
        IsCalibrationMode = false;
        titleGamePanel.text = "Simulation Mode";
        oVRPassthroughLayer.textureOpacity = 1f;

        
        gamePanel.SetActive(menuPanel.activeSelf);
        menuPanel.SetActive(!menuPanel.activeSelf);

    }
    public void StartCalibrationMode()
    {
        IsCalibrationMode = true;
        titleGamePanel.text = "Calibration Mode";
        oVRPassthroughLayer.textureOpacity = 1f;

        gamePanel.SetActive(menuPanel.activeSelf);
        menuPanel.SetActive(!menuPanel.activeSelf);
        foreach (GameObject mrObject in MRObjects)
        {
            mrObject.SetActive(true);
        }
    }

    public void EndSimulationMode()
    {
      Application.Quit();
    }

    public void ToggleMenuPanel()
    {
        gamePanel.SetActive(menuPanel.activeSelf);
        menuPanel.SetActive(!menuPanel.activeSelf);
        if (menuPanel.activeSelf)
        {
            oVRPassthroughLayer.textureOpacity = 0f;
        }
        else
        {
            IsCalibrationMode = false; // after closing the menu, usec can be only in simulation mode
            titleGamePanel.text = "Simulation Mode";
            oVRPassthroughLayer.textureOpacity = 1f;
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
