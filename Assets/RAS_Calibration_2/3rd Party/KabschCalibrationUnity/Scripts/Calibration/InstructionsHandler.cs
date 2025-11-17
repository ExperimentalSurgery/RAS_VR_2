using UnityEngine;
using TMPro;
using System;
public class InstructionsHandler : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    GameObject dynamicUI;
    [SerializeField]
    GameObject staticUI;
    [SerializeField]
    TMP_Text uiInstructionsText, indexText;
    [SerializeField]
    int uiIndex;
    [SerializeField]
    string[] instructions;
    CalibrationManager calibrationManager;
    [SerializeField]
    SequenceHandler sequenceHandler;
    [SerializeField]
    private bool wasConsoleCalibrated = false;
    [SerializeField]
    private bool wasRightStylusActivated = false;
    [SerializeField]
    private bool wasLeftStylusActivated = false;
    [SerializeField]
    private bool wasRightStylusCalibrated = false;
    [SerializeField]
    private bool wasLeftStylusCalibrated = false;
    public Action OnCompletedCalibration;
    #region GETTER AND SETTER

    public bool WasConsoleCalibrated
    {
        get { return wasConsoleCalibrated; }
    }
    public bool WasRightStylusActivated
    {
        get { return wasRightStylusActivated; }
        set { wasRightStylusActivated = value; }
    }

    public bool WasLeftStylusActivated
    {
        get { return wasLeftStylusActivated; }
        set { wasLeftStylusActivated = value; }
    }

    #endregion
    private void Awake()
    {
        calibrationManager = GetComponent<CalibrationManager>();
    }

    private void OnEnable()
    {
        sequenceHandler.onFinishedReading += DisplayFirstInstruction;
        calibrationManager.OnCalibrationComplete += SelectInstruction;
    }

    private void OnDisable()
    {
        sequenceHandler.onFinishedReading -= DisplayFirstInstruction;
        calibrationManager.OnCalibrationComplete -= SelectInstruction;
    }

    void Start()
    {
        dynamicUI.SetActive(true);
        staticUI.SetActive(false);
        indexText.text = "Next socket: " + 0;
        uiInstructionsText.text = "Read instructions before calibrating.";
        calibrationManager.CanCalibrate = true;
    }


    // Called when the user finishes reading the initial instructions for starting calibration
    public void DisplayFirstInstruction() // for right controller 
    {
        Debug.Log("Displaying first instruction");
        sequenceHandler.onFinishedReading -= DisplayFirstInstruction;
        calibrationManager.CanCalibrate = true;
        uiIndex = calibrationManager.CalibrationPointIndex;
        indexText.text = "Next socket: " + (uiIndex + 1);
        uiInstructionsText.text = instructions[0];
        wasConsoleCalibrated = false;
    }

    public void DisplayFirstInstrictionForStylus(bool isRightStykus)
    {
        if (isRightStykus) // right stylus
        {
            uiInstructionsText.text = instructions[4];
            indexText.text = "Next socket: " + (uiIndex + 1);
        }
        else  // left stylus
        {
            uiInstructionsText.text = instructions[5];
            indexText.text = "Next socket: " + (uiIndex + 1);
        }
    }


    void SelectInstruction()
    {
        uiIndex = calibrationManager.CalibrationPointIndex;
        indexText.text = "Next socket: " + GetNextSocketNumber(uiIndex);
        if (calibrationManager == null) return;

        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[0]) // right controller
        {
            switch (uiIndex)
            {
                case 0:
                    uiInstructionsText.text = instructions[1];
                    break;
                case 1:
                    uiInstructionsText.text = instructions[1];
                    break;
                case 2:
                    uiInstructionsText.text = instructions[1];
                    break;
                case 3:
                    uiInstructionsText.text = instructions[3];
                    wasConsoleCalibrated = true;
                    break;
                default:
                    uiInstructionsText.text = instructions[0];
                    break;
            }
        }

        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[1]) // right stylus
        {
            if (wasRightStylusActivated == false)
            {
                return;
            }

            if (!wasConsoleCalibrated && wasRightStylusActivated)
            {
                uiInstructionsText.text = instructions[2];
                return;
            }
            switch (uiIndex)
            {
                case 0:
                    uiInstructionsText.text = instructions[1];
                    break;
                case 1:
                    uiInstructionsText.text = instructions[1];
                    break;
                case 2:
                    uiInstructionsText.text = instructions[1];
                    break;
                case 3:
                    wasRightStylusCalibrated = true;
                    if (wasLeftStylusCalibrated && wasRightStylusCalibrated)
                    {
                        OnCompletedCalibration?.Invoke();
                        ToggleUI();
                    }
                    else
                    {
                        uiInstructionsText.text = instructions[6];
                    }
                    break;
                default:
                    uiInstructionsText.text = instructions[4];
                    break;
            }
        }

        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[2]) // left stylus
        {
            if (wasLeftStylusActivated == false)
            {
                return;
            }
            if (!wasConsoleCalibrated && wasLeftStylusActivated)
            {
                uiInstructionsText.text = instructions[2];
                return;
            }
            switch (uiIndex)
            {
                case 0:
                    uiInstructionsText.text = instructions[1];
                    break;
                case 1:
                    uiInstructionsText.text = instructions[1];
                    break;
                case 2:
                    uiInstructionsText.text = instructions[1];
                    break;
                case 3:
                    wasLeftStylusCalibrated = true;
                    if (wasLeftStylusCalibrated && wasRightStylusCalibrated)
                    {
                        OnCompletedCalibration?.Invoke();
                        ToggleUI();
                    }
                    else
                    {
                        uiInstructionsText.text = instructions[6];
                    }
                    break;
                default:
                    uiInstructionsText.text = instructions[5];
                    break;
            }
        }
    }

    void ToggleUI()
    {
        dynamicUI.SetActive(!dynamicUI.activeSelf);
        staticUI.SetActive(!staticUI.activeSelf);
    }

    private int GetNextSocketNumber(int uiIndex)
    {
        return ((uiIndex + 1) % 4) + 1;
    }
}
