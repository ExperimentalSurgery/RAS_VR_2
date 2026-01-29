using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using UnityEngine.XR;


public class CalibrationManager : MonoBehaviour
{
    public Transform[] tooltips;
    public Transform tooltip;
    public Color[] originalColorsForTips;

    [SerializeField] OVRManager ovrManager;

    [Header("Passthrough mode settings")]
    [SerializeField] private OVRPassthroughLayer oVRPassthroughLayer;
    [SerializeField] float timeForToggleEnabling = 1f;
    [SerializeField] bool canToggle = true;

    [Space(10)]

    // this is just to display the calibration process in the inspector
    [Header("Calibration points")]
    [SerializeField]
    int calibrationPointIndex;
    [SerializeField]
    private Vector3[] sourcePoints;
    [SerializeField]
    private Vector3[] targetPoints;

    [Space(10)]
    [Header("Currently selected object to align")]
    [SerializeField] 
    private bool canCalibrate = false;
    private CalibrateObject currentObjectToCalibrate;
    [SerializeField]

    private string[] alignObjectChoices;
    [SerializeField]
    CalibrateObject[] alignObjectsInScene;

    public int choiceIndex;
    private GameObject sourcePointTopParentInScene;

    [SerializeField] GameObject[] sourcePointParents;

    private int dummyIndex = 0;

    public float calibrationDistanceError = 0;


    public Action OnCalibrationComplete;
    private bool isCreatingSourcePoint = false;


    #region GETTER AND SETTER

    public bool CanCalibrate
    {
        get
        {
            if (currentObjectToCalibrate != null)
            {
                return canCalibrate;
            }
            else
            {
                return false;
            }
        }

        set => canCalibrate = value;
    }
    public CalibrateObject ObjectToCalibrate
    {
        get => currentObjectToCalibrate;
        set => currentObjectToCalibrate = value;
    }

    public string[] AlignObjectChoices
    {
        get => alignObjectChoices;
        set => alignObjectChoices = value;
    }

    public CalibrateObject[] AlignObjectsInScene
    {
        get => alignObjectsInScene;
        set => alignObjectsInScene = value;
    }

    public int ChoiceIndex
    {
        get => choiceIndex;
        set => choiceIndex = value;
    }

    public int CalibrationPointIndex
    {
        get => calibrationPointIndex;
    }

    #endregion

    void Awake()
    {
        
        alignObjectChoices = CreateCalibrationObjectsAsString(alignObjectsInScene);

        if (alignObjectsInScene.Length != 0)
        {
            currentObjectToCalibrate = alignObjectsInScene[0];
            tooltip = tooltips[0];
            //ChangeColorOfPointer(0);
        }

        sourcePointTopParentInScene = new GameObject("SourcePoints");
        //targetPointTopParentInScene.transform.SetParent(GameObject.FindGameObjectWithTag("SteamVR").transform);
        sourcePointParents = new GameObject[alignObjectsInScene.Length];

        for (int i = 0; i < alignObjectsInScene.Length; i++)
        {
            sourcePointParents[i] = new GameObject(alignObjectsInScene[i].name + "Sources");
            sourcePointParents[i].transform.SetParent(sourcePointTopParentInScene.transform);
        }
    }

    private void OnEnable()
    {
        oVRPassthroughLayer.passthroughLayerResumed.AddListener(OnPassthroughLayerResumed);
        OVRManager.TrackingOriginChangePending += LoadCalibrationFromFile;
        OVRManager.display.RecenteredPose += LoadCalibrationFromFile; // useful for “Reset View”/recenter flows
        OVRManager.TrackingOriginChangePending += LoadCalibrationFromFile;

    }

    private void Start()
    {
        foreach (Transform tip in tooltips)
        {
            originalColorsForTips = new Color[tooltips.Length];
            Renderer rendererTip = tip.GetComponent<Renderer>();
            originalColorsForTips[Array.IndexOf(tooltips, tip)] = rendererTip.sharedMaterial.color;
        }
        InitializePassthroughMode();
        ResetAllTargetPoints(true);
    }

    private void OnDisable()
    {
        oVRPassthroughLayer.passthroughLayerResumed.RemoveListener(OnPassthroughLayerResumed);
        //RevertTipColors();
    }


    public void RevertTipColors()
    {
        foreach (Transform tip in tooltips)
        {
            if (tip == null) continue;
            Renderer rendererTip = tip.GetComponent<Renderer>();
            rendererTip.sharedMaterial.color = originalColorsForTips[Array.IndexOf(tooltips, tip)];
        }
    }

    // 2) OnPassthroughLayerResumed is called once the layer is fully initialized and passthrough is visible
    private void OnPassthroughLayerResumed(OVRPassthroughLayer passthroughLayer)
    {
        // 3) Do something here after the passthrough layer has resumed
    }

    void InitializePassthroughMode()
    {
        Debug.Log("Initializing Passthrough Mode...");
        oVRPassthroughLayer.enabled = true;
        oVRPassthroughLayer.textureOpacity = 1f; // set the opacity to 0 to hide the passthrough layer

    }

    void Update()
    {
        if (!CanCalibrate) return;
        FetchSourceAndTargetPointsToDisplay();
        ChangeColorOfPointer();

        //throw new Exception("No input method implemented yet.");

        /*
        TODO: Add calibration input here, depending on VR system used - example is for SteamVR 1.0.        
        if (SteamVR_Input._default.inActions.InteractUI.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            currentObjectToCalibrate.AddTargetPoint(tooltip.position, targetPointParents[choiceIndex].transform);
            ChangeColorOfPointer();
        }
        */

    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void PressButtonRight()
    {
        Debug.Log("PRESS Button RIGHT");
    }

    public void PressButtonLeft()
    {
        Debug.Log("PRESS Button LEFT");
    }

    public void CreateSourcePoint(int objectId)
    {
        if (!CanCalibrate) return;
        // if the objectId is not the same as the current object to calibrate, set it
        if (currentObjectToCalibrate != alignObjectsInScene[objectId])
        {
            SetCallibrationObject(objectId);
        }
        Debug.Log("AddSourcePoint " + tooltip.position);
        Debug.Log("ChoiceIndex: " + choiceIndex);
        currentObjectToCalibrate.AddSourcePoint(tooltip.position, sourcePointParents[choiceIndex].transform, choiceIndex);
        ChangeColorOfPointer();
        FetchSourceAndTargetPointsToDisplay();
        OnCalibrationComplete?.Invoke();
        SaveCalibrationToFile();
    }
    public void TryCreateDelayedSourcePoint(int objectId)
    {
        // Block if already in the middle of creating one
        if (isCreatingSourcePoint) return;

        StartCoroutine(CreateSourcePointDelayed(objectId));
    }

    private IEnumerator CreateSourcePointDelayed(int objectId)
    {
        isCreatingSourcePoint = true;
        CreateSourcePoint(objectId);
        //small delay to debounce / avoid double-press
        yield return new WaitForSeconds(.25f);
        isCreatingSourcePoint = false;
    }


    public void CreateTargetPoint(int objectId)
    {
        if (!CanCalibrate) return;
        // if the objectId is not the same as the current object to calibrate, set it
        if (currentObjectToCalibrate != alignObjectsInScene[objectId])
        {
            SetCallibrationObject(objectId);
        }
        Debug.Log("AddTargetPoint " + tooltip.position);
        Debug.Log("can calibrate " + CanCalibrate);
        Debug.Log("ChoiceIndex: " + choiceIndex);
        currentObjectToCalibrate.AddTargetPoint(tooltip.position, sourcePointParents[choiceIndex].transform, choiceIndex);

        ChangeColorOfPointer();
        FetchSourceAndTargetPointsToDisplay();
        OnCalibrationComplete?.Invoke();
        SaveCalibrationToFile();

    }


    public void Calibrate()
    {
        currentObjectToCalibrate.Calibrate();
    }

    private Vector3 CreateDummySourcePoint(int number)
    {
        switch (number % sourcePoints.Length)
        {
            // blue
            case 0:
                return new Vector3(0, 1.25f, 0);
            // red
            case 1:
                return new Vector3(0.5f, 1.25f, 0);
            // yellow
            case 2:
                return new Vector3(0.5f, 1.25f, -0.5f);
            // green
            case 3:
                return new Vector3(0, 1.25f, -0.5f);
            // magenta
            case 4:
                return new Vector3(0, 0.75f, 0);
            default:
                return Vector3.zero;
        }
    }

    #region CUSTOM EDITOR UI

    public string[] CreateCalibrationObjectsAsString(CalibrateObject[] input)
    {
        string[] result = new string[input.Length];

        for (int i = 0; i < input.Length; i++)
        {
            result[i] = input[i].ToString();
        }

        return result;
    }

    public void FetchSourceAndTargetPointsToDisplay()
    {
        calibrationDistanceError = currentObjectToCalibrate.calibrationDistanceError;
        calibrationPointIndex = currentObjectToCalibrate.calibrationPointIndex;
        
        Debug.Log("calibrationPointIndex: " + currentObjectToCalibrate + ": " + currentObjectToCalibrate.calibrationPointIndex);
        sourcePoints = GetVectorsFromTransforms(currentObjectToCalibrate.sourcePoints);
        targetPoints = GetVectorsFromTransforms(currentObjectToCalibrate.targetPoints);
    }



    private Vector3[] GetVectorsFromTransforms(Transform[] transforms)
    {
        Vector3[] result = new Vector3[transforms.Length];

        for (int i = 0; i < transforms.Length; i++)
        {
            if (transforms[i] != null)
            {
                result[i] = transforms[i].position;
            }
        }

        return result;
    }

    public void SetCallibrationObject(int objectId)
    {
        if (objectId <= alignObjectsInScene.Length)
        {
            choiceIndex = objectId;
            currentObjectToCalibrate = alignObjectsInScene[choiceIndex];
            tooltip = tooltips[choiceIndex];
            Debug.Log("Set currentObjectToCalibrate to: " + currentObjectToCalibrate.name);
            ChangeColorOfPointer();
        }
    }


    public void ResetAllTargetPoints(bool isReset)
    {
        Debug.Log("ResetAllTargetPoints");
        alignObjectsInScene[0].ResetAllTargetPoints();
        alignObjectsInScene[1].calibrationPointIndex = 0;
        alignObjectsInScene[2].calibrationPointIndex = 0;
        GetComponent<InstructionsHandler>().ResetUI(isReset);

    }

    public void ResetIndices()
    {
        alignObjectsInScene[0].ResetAllTargetPoints();
        alignObjectsInScene[1].calibrationPointIndex = 0;
        alignObjectsInScene[2].calibrationPointIndex = 0;
    }

    public void ResetTargetPoints()
    {
        currentObjectToCalibrate.ResetAllTargetPoints();
   
    }

    public void ResetLastTargetPoint()
    {
        currentObjectToCalibrate.ResetLastTargetPoint();
    }

    public void SaveCalibrationToFile()
    {
        TransformPersistence.GetInstance().SaveToFile();
    }

    public void LoadCalibrationFromFile()
    {
        TransformPersistence.GetInstance().LoadAndApplyTransformationFromFile();
        Debug.Log("Load Calibration");
    }

    public void LoadCalibrationFromFile(OVRManager.TrackingOrigin origin, OVRPose? pose)
    {
        TransformPersistence.GetInstance().LoadAndApplyTransformationFromFile();
        Debug.Log("Load Calibration OnTrackingOriginChangePending");
    }

    public void ChangeColorOfPointer()
    {
        int colorNumber = (sourcePoints.Length != 0) ? calibrationPointIndex : 0;
        Debug.Log("colorNumber: " + colorNumber);
        //int colorNumber = calibrationPointIndex;

        Renderer rendererController = tooltips[0].GetComponent<Renderer>(); // to change color of controller pointer
        rendererController.material = new Material(Shader.Find("UI/Unlit/Detail"));
        rendererController.sharedMaterial.color = ColorOrder.GetColor(colorNumber);

        //if (choiceIndex == 0) { return; }
        //Renderer rendererRight = tooltips[choiceIndex].GetComponent<Renderer>(); // to change color of another pointer
        //rendererRight.material = new Material(Shader.Find("UI/Unlit/Detail"));
        //rendererRight.sharedMaterial.color = ColorOrder.GetColor(colorNumber);
    }

    #endregion
}
