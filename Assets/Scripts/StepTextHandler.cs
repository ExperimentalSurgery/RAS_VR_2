using TMPro;
using UnityEngine;
using TMPro;
using UnityEngine;

public class StepTextHandler : MonoBehaviour
{
    public TextMeshProUGUI textLabelObject;
    public TextMeshProUGUI textBodyObject;
    public string[] stepLabelTexts;
    public string[] stepBodyTexts;
    int m_CurrentStepIndex = 0;

    public void Next()
    {
        //label
        textLabelObject.text = stepLabelTexts[m_CurrentStepIndex];
        // body
        textBodyObject.text = stepBodyTexts[m_CurrentStepIndex];

        m_CurrentStepIndex = (m_CurrentStepIndex + 1) % stepBodyTexts.Length;
    }
}

