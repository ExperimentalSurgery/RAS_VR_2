using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;

public class SequenceHandler : MonoBehaviour
{
    [Serializable]
    class SequencStep
    {
        [SerializeField]
        public GameObject stepObject;

        [SerializeField]
        public string buttonText;
    }

    [SerializeField]
    public TextMeshProUGUI m_StepButtonTextField;

    [SerializeField]
    List<SequencStep> m_StepList = new List<SequencStep>();

    public Action onFinishedReading;

    int m_CurrentStepIndex = 0;

    public void Next()
    {
        m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
        m_CurrentStepIndex = (m_CurrentStepIndex + 1) % m_StepList.Count;
        if (m_CurrentStepIndex == 0)
        {
            onFinishedReading?.Invoke();
        }
        m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
        m_StepButtonTextField.text = m_StepList[m_CurrentStepIndex].buttonText;
    }

    public void Previous()
    {
        m_StepList[m_CurrentStepIndex].stepObject.SetActive(false);
        m_CurrentStepIndex = (m_CurrentStepIndex - 1 + m_StepList.Count) % m_StepList.Count;
        m_StepList[m_CurrentStepIndex].stepObject.SetActive(true);
        m_StepButtonTextField.text = m_StepList[m_CurrentStepIndex].buttonText;

    }
}

