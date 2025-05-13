using TMPro;
using UnityEngine;
using TMPro;
using UnityEngine;

public class StepTextHandler : MonoBehaviour
    {
        public TextMeshProUGUI textObject;
        public string[] stepTexts;
        int m_CurrentStepIndex = 0;

        public void Next()
        {
            textObject.text = stepTexts[m_CurrentStepIndex];
            m_CurrentStepIndex = (m_CurrentStepIndex + 1) % stepTexts.Length;
        }
    }

