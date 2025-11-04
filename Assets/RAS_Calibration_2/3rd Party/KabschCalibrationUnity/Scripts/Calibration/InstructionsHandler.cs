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

    private void Awake()
    {
        calibrationManager = GetComponent<CalibrationManager>();
    }

    private void OnEnable()
    {
        calibrationManager.OnCalibrationComplete += SelectInstruction;
    }


    void SelectInstruction()
    {
        uiIndex = calibrationManager.CalibrationPointIndex;
        indexText.text= uiIndex.ToString();
        if (calibrationManager == null) return;

        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[0])
        {
            switch (uiIndex)
            {
                case 0:
                    uiInstructionsText.text = instructions[0];
                    break;
                default:
                    uiInstructionsText.text = instructions[3];
                    break;
            }
        }

        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[1])
        {
            switch (uiIndex)
            {
                case 0:
                    uiInstructionsText.text = instructions[1];
                    break;
                default:
                    uiInstructionsText.text = instructions[3];
                    break;
            }
        }

        if (calibrationManager.ObjectToCalibrate == calibrationManager.AlignObjectsInScene[2])
        {
            switch (uiIndex)
            {
                case 0:
                    uiInstructionsText.text = instructions[1];
                    break;
                default:
                    uiInstructionsText.text = instructions[3];
                    break;
            }
        }
    }
  
}
