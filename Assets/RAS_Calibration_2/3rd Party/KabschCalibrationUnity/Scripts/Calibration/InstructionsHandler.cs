using UnityEngine;
using TMPro;
using System;
public class InstructionsHandler : MonoBehaviour
{
    [SerializeField]
    TMP_Text uiInstructionsText, indexText;
    [SerializeField]
    int uiIndex;
    [SerializeField]
    string[] instructions;
    CalibrationManager calibrationManager;
    [SerializeField]
    SequenceHandler sequenceHandler;
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


    private void DisplayFirstInstruction() // for right controller
    {
        uiIndex = calibrationManager.CalibrationPointIndex;
        uiInstructionsText.text = instructions[0];
    }

    private void DisplayInstrictionForStyluses()
    {

    }


    void SelectInstruction()
    {
        uiIndex = calibrationManager.CalibrationPointIndex;
        indexText.text = "Next socket: " + (uiIndex +2);
        if (calibrationManager == null) return;

        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[0])
        {
            switch (uiIndex)
            {
                //case 0:
                //    uiInstructionsText.text = instructions[0];
                //    break;
                default:
                    uiInstructionsText.text = instructions[3];
                    break;
            }
        }

        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[1])
        {
            switch (uiIndex)
            {
                //case 0:
                //    uiInstructionsText.text = instructions[1];
                //    break;
                default:
                    uiInstructionsText.text = instructions[3];
                    break;
            }
        }

        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[2])
        {
            switch (uiIndex)
            {
                //case 0:
                //    uiInstructionsText.text = instructions[1];
                //    break;
                default:
                    uiInstructionsText.text = instructions[3];
                    break;
            }
        }
    }

}
