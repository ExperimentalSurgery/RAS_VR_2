using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
public class InstructionsHandler : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    GameObject dynamicUI_instructions;
    [SerializeField]
    Image updatedImage;
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
    private bool wasRightStylusCalibrated = false;
    [SerializeField]
    private bool wasLeftStylusCalibrated = false;
    public Action OnCompletedCalibration;
    #region GETTER AND SETTER

    public bool WasConsoleCalibrated
    {
        get { return wasConsoleCalibrated; }
    }

    #endregion
    private void Awake()
    {
        calibrationManager = GetComponent<CalibrationManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) //right stylus
        {
            calibrationManager.TryCreateDelayedSourcePoint(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)) //left stylus
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
        indexText.text = "";
        uiInstructionsText.text = "Take out the styluses";
        calibrationManager.CanCalibrate = false;
        wasConsoleCalibrated = false;
        updatedImage.gameObject.SetActive(false);
    }


    // Called when the user finishes reading the initial instructions for starting calibration
    public void DisplayFirstInstruction(bool isReset) 
    {
        dynamicUI_instructions.SetActive(false);
        dynamicUI_updatedText.SetActive(true);
        wasConsoleCalibrated = false;
        wasRightStylusCalibrated = false;
        wasLeftStylusCalibrated = false;
        if (!isReset)
        {
            startCalibrationButton.SetActive(false);
            resetCalibrationButton.SetActive(true);
            Debug.Log("Displaying first instruction");
            calibrationManager.CanCalibrate = true;
            indexText.text = "Next socket: 1";
            uiInstructionsText.text = instructions[0];
            updatedImage.sprite = images[0];
            updatedImage.gameObject.SetActive(true);
        }
        else
        {
            startCalibrationButton.SetActive(true);
            resetCalibrationButton.SetActive(false);
            indexText.text = "";
            uiInstructionsText.text = "Take out the styluses";
            calibrationManager.CanCalibrate = false;
            updatedImage.gameObject.SetActive(false);

        }
    }


    void SelectInstruction()
    {
        uiIndex = calibrationManager.CalibrationPointIndex;
        //indexText.text = "Next socket: "+ uiIndex.ToString();
        indexText.text = "Next socket: " + GetNextSocketNumber(uiIndex);
        Debug.Log("uiIndex" + uiIndex);
        if (calibrationManager == null) return;

        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[0] && !wasConsoleCalibrated) // right controller
        {
            switch (uiIndex)
            {
                case 0:
                    uiInstructionsText.text = instructions[0];
                    updatedImage.sprite = images[0];
                    break;
                case 1:
                    uiInstructionsText.text = instructions[1];
                    updatedImage.sprite = images[1];
                    break;
                case 2:
                    uiInstructionsText.text = instructions[2];
                    updatedImage.sprite = images[2];
                    break;
                case 3:
                    uiInstructionsText.text = instructions[3];
                    updatedImage.sprite = images[3];
                    wasConsoleCalibrated = true;
                    break;
                default:
                    uiInstructionsText.text = instructions[0];
                    updatedImage.sprite = images[0];
                    break;
            }
        }
        else if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[0] && wasConsoleCalibrated)
        {
            uiInstructionsText.text = instructions[4];
            updatedImage.sprite = images[4];
        }
        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[1] && !wasRightStylusCalibrated) // right stylus
        {

            if (!wasConsoleCalibrated)
            {
                uiInstructionsText.text = instructions[5];
                updatedImage.sprite = images[5];
                return;
            }

            switch (GetNextSocketNumber(uiIndex))
            {
                case 1:
                    uiInstructionsText.text = instructions[4];
                    updatedImage.sprite = images[4];
                    break;
                case 2:
                    uiInstructionsText.text = instructions[6];
                    updatedImage.sprite = images[6];
                    break;
                case 3:
                    uiInstructionsText.text = instructions[7];
                    updatedImage.sprite = images[7];
                    break;
                case 4:
                    uiInstructionsText.text = instructions[8];
                    updatedImage.sprite = images[8];
                    wasRightStylusCalibrated = true;
                    //if (wasLeftStylusCalibrated && wasRightStylusCalibrated)
                    //{
                    //    OnCompletedCalibration?.Invoke();
                    //    ShowStaticUI();
                    //}
                    //else
                    //{
                    //    uiInstructionsText.text = instructions[8];
                    //    updatedImage.sprite = images[8];
                    //}
                    break;
                default:
                    uiInstructionsText.text = instructions[4];
                    updatedImage.sprite = images[4];
                    break;
            }
        }

        else if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[1] && wasRightStylusCalibrated && !wasLeftStylusCalibrated && uiIndex == 0) // right stylus
        {
            uiInstructionsText.text = instructions[9];
            updatedImage.sprite = images[9];
        }

        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[2] && !wasLeftStylusCalibrated) // left stylus
        {
            if (!wasConsoleCalibrated)
            {
                uiInstructionsText.text = instructions[5];
                updatedImage.sprite = images[5];
                return;
            }
            switch (GetNextSocketNumber(uiIndex))
            {
                case 1:
                    uiInstructionsText.text = instructions[4];
                    updatedImage.sprite = images[4];
                    break;
                case 2:
                    uiInstructionsText.text = instructions[6];
                    updatedImage.sprite = images[6];

                    break;
                case 3:
                    uiInstructionsText.text = instructions[7];
                    updatedImage.sprite = images[7];
                    break;
                case 4:
                    uiInstructionsText.text = instructions[8];
                    updatedImage.sprite = images[8];
                    wasLeftStylusCalibrated = true;
                    //if (wasLeftStylusCalibrated && wasRightStylusCalibrated)
                    //{
                    //    OnCompletedCalibration?.Invoke();
                    //    ShowStaticUI();
                    //}
                    //else
                    //{
                    //    uiInstructionsText.text = instructions[8];
                    //    updatedImage.sprite = images[8];
                    //}
                    break;
                default:
                    uiInstructionsText.text = instructions[5];
                    updatedImage.sprite = images[5];
                    break;
            }
        }
        else if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[2] && wasLeftStylusCalibrated && !wasRightStylusCalibrated && uiIndex == 0) // left stylus
        {
            uiInstructionsText.text = instructions[9];
            updatedImage.sprite = images[9];
        }
        if (wasLeftStylusCalibrated && wasRightStylusCalibrated && uiIndex == 0) {
            OnCompletedCalibration?.Invoke();
            ShowStaticUI();
        }
    }

    public void ResetUI(bool isReset)
    {

        //sequenceHandler.onFinishedReading += DisplayFirstInstruction;
        //calibrationManager.CanCalibrate = false;
        //dynamicUI_instructions.SetActive(true);
        //dynamicUI_updatedText.SetActive(true);
        //wasConsoleCalibrated = false;
        //calibrationManager.CanCalibrate = true;
        //SelectInstruction();
        DisplayFirstInstruction(isReset);
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
        //return ((uiIndex + 1) % 4) + 1;
        return uiIndex + 1;
    }
}
