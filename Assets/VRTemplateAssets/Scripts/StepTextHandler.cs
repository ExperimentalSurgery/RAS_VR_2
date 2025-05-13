using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class StepTextHandler : MonoBehaviour
{
    public TextMeshPro textObject;
    public string[] stepTexts;
    int m_CurrentStepIndex = 0;

    public void Next()
    {
        textObject.text = stepTexts[m_CurrentStepIndex].
        m_CurrentStepIndex = (m_CurrentStepIndex + 1) % stepTexts.Length;
        m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
        m_StepButtonTextField.text = m_StepList[m_CurrentStepIndex].buttonText;
    }
}
}
