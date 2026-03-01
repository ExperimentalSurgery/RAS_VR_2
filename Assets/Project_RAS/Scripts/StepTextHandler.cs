using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using TMPro;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

enum Setting
{
    setting_1,
    setting_2,
    setting_3

}
public class StepTextHandler : MonoBehaviour
{
    [SerializeField] Setting currentSetting;
    [Header("UI Elements")]
    [SerializeField] TextMeshProUGUI textLabelObject;
    [SerializeField] TextMeshProUGUI textBodyObject;
    [SerializeField] TextMeshProUGUI stepButtonTextField;
    [Header("Step Texts")]
    [SerializeField] float initialFadingDuration = 9;
    [SerializeField] float fadingDuration = 6;   
    [SerializeField] string[] stepLabelTexts;
    [SerializeField] GameObject[] stepArrows;
    int m_StepIndex = 0;
    int m_CurrentStepIndex = 0;
    List<Renderer> allArrowRenders = new List<Renderer>();

    private void Start()
    {
        foreach (var arrow in stepArrows)
        {
            arrow.SetActive(false);
        }

        if (currentSetting == Setting.setting_1)
        {
            SelectArrows_1();
        }
        if (currentSetting == Setting.setting_2)
        {
            SelectArrows_2();
        }
        if (currentSetting == Setting.setting_3)
        {
            SelectArrows_3();
        }

        m_StepIndex = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextSetting();
        }
    }

    public void NextSettingOnButtonClick()
    {
        if(currentSetting == Setting.setting_1 && m_CurrentStepIndex == 2)
        {
            NextSetting();
        } 

        if (currentSetting == Setting.setting_2 && m_CurrentStepIndex == 1)
        {
            NextSetting();
        }

        if (currentSetting == Setting.setting_3 && m_CurrentStepIndex == 1)
        {
            NextSetting();
        }
    }
    public void NextSetting()
    {
        if (!GameModeManager.Instance.DelayFinished) { return; }


        if (currentSetting == Setting.setting_1 )
        {
            //label
            textLabelObject.text = stepLabelTexts[m_StepIndex];
            if(m_StepIndex < stepLabelTexts.Length-1)
            {
                stepButtonTextField.text = "Continue";
                SelectArrows_1();
            }
            else
            {
                stepButtonTextField.text = "Repeat";
            }
            m_StepIndex = (m_StepIndex + 1) % stepLabelTexts.Length;
        }
        if (currentSetting == Setting.setting_2)
        {

            SelectArrows_2();
            if(m_StepIndex <= stepLabelTexts.Length - 1)
            {   //label
                textLabelObject.text = stepLabelTexts[m_StepIndex];
                m_StepIndex = (m_StepIndex + 1);
            }
        }

        if (currentSetting == Setting.setting_3)
        {

            SelectArrows_3();
            if (m_StepIndex <= stepLabelTexts.Length - 1)
            {   //label
                textLabelObject.text = stepLabelTexts[m_StepIndex];
                m_StepIndex = (m_StepIndex + 1);
            }
        }
    }


    void LoadNextScene()
    {
        // Load the next scene here
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        Debug.Log("Load next scene");
    }

    void SelectArrows_1()
    {
        DeactivateAllArrows(stepArrows);

        foreach (var arrow in stepArrows)
        {
            arrow.SetActive(false);
        }
        if (allArrowRenders != null)
        {
            foreach (var rend in allArrowRenders)
            {
                Color c = rend.material.color;
                c.a = 1f;
                rend.material.color = c;
            }
        }  
        switch (m_StepIndex)
        {
            case 0:
                m_CurrentStepIndex = 0;
                stepArrows[0].SetActive(true);
                var renderers_0 = stepArrows[0].GetComponentsInChildren<Renderer>();
                allArrowRenders.AddRange(renderers_0);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderers_0), initialFadingDuration));
                break;
            case 1:
                m_CurrentStepIndex = 1;
                stepArrows[1].SetActive(true);
                var renderers_1 = stepArrows[1].GetComponentsInChildren<Renderer>();
                allArrowRenders.AddRange(renderers_1);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderers_1), fadingDuration));
                break;
            case 2:
                m_CurrentStepIndex = 2;
                stepArrows[2].SetActive(true);
                stepArrows[3].SetActive(true);
                var renderers_2 = stepArrows[2].GetComponentsInChildren<Renderer>();
                var renderers_3 = stepArrows[3].GetComponentsInChildren<Renderer>();
                var renderersList_2_3 = renderers_2.Concat(renderers_3).ToArray();
                allArrowRenders.AddRange(renderersList_2_3);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderersList_2_3), fadingDuration));
                break;
            case 3:
                m_CurrentStepIndex = 3;
                stepArrows[4].SetActive(true);
                var renderers_4 = stepArrows[4].GetComponentsInChildren<Renderer>();
                allArrowRenders.AddRange(renderers_4);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderers_4), fadingDuration));
                break;
            case 4:
                stepArrows[5].SetActive(true);
                var renderers_5 = stepArrows[5].GetComponentsInChildren<Renderer>();
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderers_5), fadingDuration));
                break;
            case 5:
                m_CurrentStepIndex = 5;
                stepArrows[6].SetActive(true);
                stepArrows[7].SetActive(true);
                var renderers_6 = stepArrows[6].GetComponentsInChildren<Renderer>();
                var renderers_7 = stepArrows[7].GetComponentsInChildren<Renderer>();
                var renderersList_6_7 = renderers_6.Concat(renderers_7).ToArray();
                allArrowRenders.AddRange(renderersList_6_7);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderersList_6_7), fadingDuration));
                break;
            default:
                break;
        }
    }

    void SelectArrows_2()
    {
        DeactivateAllArrows(stepArrows);

        if (allArrowRenders != null)
        {
            foreach (var rend in allArrowRenders)
            {
                Color c = rend.material.color;
                c.a = 1f;
                rend.material.color = c;
            }
        }
        switch (m_StepIndex)
        {
            case 0:
                m_CurrentStepIndex = 0;
                stepArrows[0].SetActive(true);
                stepArrows[1].SetActive(true);
                var renderers_0 = stepArrows[0].GetComponentsInChildren<Renderer>();
                var renderers_1 = stepArrows[1].GetComponentsInChildren<Renderer>();
                var renderersList_0_1 = renderers_0.Concat(renderers_1).ToArray();
                allArrowRenders.AddRange(renderersList_0_1);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderersList_0_1), initialFadingDuration));
                break;
            case 1:
                m_CurrentStepIndex = 1;
                //stepArrows[0].SetActive(false);
                //stepArrows[1].SetActive(false);
                stepArrows[2].SetActive(true);
                var renderers_2 = stepArrows[2].GetComponentsInChildren<Renderer>();
                allArrowRenders.AddRange(renderers_2);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderers_2), fadingDuration));
                break;
            default:
                break;
        }
    }

    void SelectArrows_3()
    {
        DeactivateAllArrows(stepArrows);
       
        if (allArrowRenders != null)
        {
            foreach (var rend in allArrowRenders)
            {
                Color c = rend.material.color;
                c.a = 1f;
                rend.material.color = c;
            }
        }
        switch (m_StepIndex)
        {
            case 0:
                m_CurrentStepIndex = 0;
                stepArrows[0].SetActive(true);
                stepArrows[1].SetActive(true);
                var renderers_0 = stepArrows[0].GetComponentsInChildren<Renderer>();
                var renderers_1 = stepArrows[1].GetComponentsInChildren<Renderer>();
                var renderersList_0_1 = renderers_0.Concat(renderers_1).ToArray();
                allArrowRenders.AddRange(renderersList_0_1);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderersList_0_1), initialFadingDuration));
                break;
            case 1:
                m_CurrentStepIndex = 1;
               // stepArrows[0].SetActive(false);
                //stepArrows[1].SetActive(false);
                stepArrows[2].SetActive(true);
                var renderers_2 = stepArrows[2].GetComponentsInChildren<Renderer>();
                allArrowRenders.AddRange(renderers_2);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderers_2), fadingDuration));
                break;
            default:
                break;
        }
    }

    void DeactivateAllArrows(GameObject[] stepArrowsArray) {        
        foreach (var arrow in stepArrowsArray)
        {
            arrow.SetActive(false);
        }
    }



    public IEnumerator FadeOutObjects(List<Renderer> renderers, float duration)
    {
        float fadingDuration = duration / 2;
        float delay = duration / 2;
        yield return new WaitForSeconds(delay);
        // Materialien einsammeln
        List<Material> mats = new List<Material>();
        foreach (var rend in renderers)
        {
            foreach (var mat in rend.materials) // kopiert automatisch Instanzen
            {
                mats.Add(mat);
            }
        }

        float t = 0f;
        while (t < fadingDuration)
        {
            t += Time.deltaTime;
            float normalized = t / fadingDuration;

            foreach (var mat in mats)
            {
                Color c = mat.color;
                c.a = Mathf.Lerp(1f, 0f, normalized);
                mat.color = c;
            }

            yield return null;
        }

        // Alpha sicher auf 0
        foreach (var mat in mats)
        {
            Color c = mat.color;
            c.a = 0f;
            mat.color = c;
        }
    }


    public IEnumerator FadeInObjects(List<Renderer> renderers, float duration)
    {
        // Materialien einsammeln
        List<Material> mats = new List<Material>();
        foreach (var rend in renderers)
        {
            foreach (var mat in rend.materials)
            {
                mats.Add(mat);
            }
        }

        // Alpha zu Beginn auf 0 setzen
        foreach (var mat in mats)
        {
            Color c = mat.color;
            c.a = 0f;
            mat.color = c;
        }

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;

            foreach (var mat in mats)
            {
                Color c = mat.color;
                c.a = Mathf.Lerp(0f, 1f, normalized);
                mat.color = c;
            }

            yield return null;
        }

        // Sicherheit: Alpha am Ende auf 1 setzen
        foreach (var mat in mats)
        {
            Color c = mat.color;
            c.a = 1f;
            mat.color = c;
        }
    }
}

