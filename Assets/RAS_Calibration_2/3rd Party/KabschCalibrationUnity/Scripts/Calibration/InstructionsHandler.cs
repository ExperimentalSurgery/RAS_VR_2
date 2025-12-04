using UnityEngine;
using TMPro;
using System;
public class InstructionsHandler : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    GameObject dynamicUI_instructions;
    [SerializeField]
    Sprite updatedSprite;
    [SerializeField]
    GameObject dynamicUI_updatedText, startCalibrationButton, resetCalibrationButton;
    [SerializeField]
    GameObject staticUI;
    [SerializeField]
    TMP_Text uiInstructionsText, indexText;
    [SerializeField]
    int uiIndex;
    [SerializeField]
    string[] instructions;
    [SerializeField]
    Sprite[] images;
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

    private void Update()
    {
        if (Input.GetButtonDown(KeyCode.Alpha1.ToString())) //right stylus
        {
            calibrationManager.TryCreateDelayedSourcePoint(1);
        }
        if (Input.GetButtonDown(KeyCode.Alpha2.ToString())) //left stylus
        {
            calibrationManager.TryCreateDelayedSourcePoint(2);
        }
    }
    private void OnEnable()
    {
       // sequenceHandler.onFinishedReading += DisplayFirstInstruction;
        calibrationManager.OnCalibrationComplete += SelectInstruction;
    }

    private void OnDisable()
    {
        //sequenceHandler.onFinishedReading -= DisplayFirstInstruction;
        calibrationManager.OnCalibrationComplete -= SelectInstruction;
    }

    void Start()
    {
        dynamicUI_instructions.SetActive(false);
        dynamicUI_updatedText.SetActive(true);
        startCalibrationButton.SetActive(true);
        resetCalibrationButton.SetActive(false);
        staticUI.SetActive(false);
        indexText.text = "Next socket: ";
        uiInstructionsText.text = "Take out the styluses";
        calibrationManager.CanCalibrate = false;
        wasConsoleCalibrated = false;
        wasRightStylusActivated = false;
        wasLeftStylusActivated = false;
    }


    // Called when the user finishes reading the initial instructions for starting calibration
    public void DisplayFirstInstruction() // for right controller 
    {
        startCalibrationButton.SetActive(false);
        resetCalibrationButton.SetActive(true);
        Debug.Log("Displaying first instruction");
        calibrationManager.CanCalibrate = true;
        wasConsoleCalibrated = false;
        //SelectInstruction();
        //uiIndex = calibrationManager.CalibrationPointIndex;
        indexText.text = "Next socket: 1";
        uiInstructionsText.text = instructions[0];
        
    }


    void SelectInstruction()
    {
        uiIndex = calibrationManager.CalibrationPointIndex;
        indexText.text = "Next socket: " + GetNextSocketNumber(uiIndex);
        Debug.Log("uiIndex" + uiIndex);
        if (calibrationManager == null) return;

        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[0] && !wasConsoleCalibrated) // right controller
        {
            switch (uiIndex)
            {
                case 0:
                    uiInstructionsText.text = instructions[0];
                    break;
                case 1:
                    uiInstructionsText.text = instructions[1];
                    break;
                case 2:
                    uiInstructionsText.text = instructions[2];
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
        else if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[0] && wasConsoleCalibrated)
        {
            uiInstructionsText.text = instructions[4];
        }
        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[1] && !wasRightStylusCalibrated) // right stylus
        {

            if (!wasConsoleCalibrated)
            {
                uiInstructionsText.text = instructions[5];
                return;
            }

            switch (GetNextSocketNumber(uiIndex))
            {
                case 1:
                    uiInstructionsText.text = instructions[4];
                    break;
                case 2:
                    uiInstructionsText.text = instructions[6];
                    break;
                case 3:
                    uiInstructionsText.text = instructions[7];
                    break;
                case 4:
                    uiInstructionsText.text = instructions[8];
                    wasRightStylusCalibrated = true;
                    if (wasLeftStylusCalibrated && wasRightStylusCalibrated)
                    {
                        OnCompletedCalibration?.Invoke();
                        ShowStaticUI();
                    }
                    else
                    {
                        uiInstructionsText.text = instructions[8];
                    }
                    break;
                default:
                    uiInstructionsText.text = instructions[4];
                    break;
            }
        }

        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[2]) // left stylus
        {
            if (!wasConsoleCalibrated)
            {
                uiInstructionsText.text = instructions[5];
                return;
            }
            switch (GetNextSocketNumber(uiIndex))
            {
                case 1:
                    uiInstructionsText.text = instructions[4];
                    break;
                case 2:
                    uiInstructionsText.text = instructions[6];
                    break;
                case 3:
                    uiInstructionsText.text = instructions[7];
                    break;
                case 4:
                    uiInstructionsText.text = instructions[8];
                    wasLeftStylusCalibrated = true;
                    if (wasLeftStylusCalibrated && wasRightStylusCalibrated)
                    {
                        OnCompletedCalibration?.Invoke();
                        ShowStaticUI();
                    }
                    else
                    {
                        uiInstructionsText.text = instructions[9];
                    }
                    break;
                default:
                    uiInstructionsText.text = instructions[5];
                    break;
            }
        }
    }

    public void ResetUI()
    {

        //sequenceHandler.onFinishedReading += DisplayFirstInstruction;
        //calibrationManager.CanCalibrate = false;
        //dynamicUI_instructions.SetActive(true);
        //dynamicUI_updatedText.SetActive(true);
        //wasConsoleCalibrated = false;
        //calibrationManager.CanCalibrate = true;
        //SelectInstruction();
        DisplayFirstInstruction();
        calibrationManager.RevertTipColors();
        staticUI.SetActive(false);
    }

    void ShowStaticUI()
    {
        calibrationManager.CanCalibrate = false;
        calibrationManager.RevertTipColors();
        dynamicUI_instructions.SetActive(false);
        dynamicUI_updatedText.SetActive(false);
        staticUI.SetActive(true);
    }

    public void ShowInstructionsUI(bool toShowInstruction)
    {

        if (toShowInstruction)
        {
            calibrationManager.RevertTipColors();
            calibrationManager.CanCalibrate = false;
            dynamicUI_instructions.SetActive(true);
            dynamicUI_updatedText.SetActive(false);
            staticUI.SetActive(false);
        }
       else
        {
            calibrationManager.CanCalibrate = true;
            dynamicUI_instructions.SetActive(false);
            dynamicUI_updatedText.SetActive(true);
            staticUI.SetActive(false);
            //SelectInstruction();

        }
    }

    private int GetNextSocketNumber(int uiIndex)
    {
        return ((uiIndex + 1) % 4) + 1;
    }
}
