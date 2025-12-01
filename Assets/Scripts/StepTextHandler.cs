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
    [SerializeField] string[] stepLabelTexts;
    [SerializeField] GameObject[] stepArrows;
    //[SerializeField] string[] stepBodyTexts;
    [SerializeField] TMP_Text nextButton;
    int m_StepIndex = 0;
    int m_CurrentStepIndex = 0;
    [SerializeField] private bool toLoadNextScene = false;
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

        m_StepIndex = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextSetting();
        }
    }
    public void NextSetting()
    {
        // If we are at the last step and the button is clicked, load the next scene
        if (toLoadNextScene)
        {
            LoadNextScene();
        }

        if(currentSetting == Setting.setting_1 )
        {
            //label
            textLabelObject.text = stepLabelTexts[m_StepIndex];
            SelectArrows_1();
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
        StopAllCoroutines();
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
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderers_0), 6f));
                break;
            case 1:
                m_CurrentStepIndex = 1;
                stepArrows[1].SetActive(true);
                var renderers_1 = stepArrows[1].GetComponentsInChildren<Renderer>();
                allArrowRenders.AddRange(renderers_1);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderers_1), 3f));
                break;
            case 2:
                m_CurrentStepIndex = 2;
                stepArrows[2].SetActive(true);
                var renderers_2 = stepArrows[2].GetComponentsInChildren<Renderer>();
                allArrowRenders.AddRange(renderers_2);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderers_2), 3f));
                break;
            case 3:
                m_CurrentStepIndex = 3;
                stepArrows[3].SetActive(true);
                stepArrows[4].SetActive(true);
                var renderers_3 = stepArrows[3].GetComponentsInChildren<Renderer>();
                var renderers_4 = stepArrows[4].GetComponentsInChildren<Renderer>();
                var renderersList_3_4 = renderers_3.Concat(renderers_4).ToArray();
                allArrowRenders.AddRange(renderersList_3_4);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderersList_3_4), 3f));
                break;
            case 4:
                stepArrows[5].SetActive(true);
                var renderers_5 = stepArrows[5].GetComponentsInChildren<Renderer>();
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderers_5), 3f));
                break;
            case 5:
                m_CurrentStepIndex = 5;
                stepArrows[6].SetActive(true);
                var renderers_6 = stepArrows[6].GetComponentsInChildren<Renderer>();
                allArrowRenders.AddRange(renderers_6);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderers_6), 3f));
                break;
            case 6:
                m_CurrentStepIndex = 6;
                stepArrows[7].SetActive(true);
                stepArrows[8].SetActive(true);
                var renderers_7 = stepArrows[7].GetComponentsInChildren<Renderer>();
                var renderers_8 = stepArrows[8].GetComponentsInChildren<Renderer>();
                var renderersList_7_8 = renderers_7.Concat(renderers_8).ToArray();
                allArrowRenders.AddRange(renderersList_7_8);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderersList_7_8), 3f));
                break;
            default:
                break;
        }
    }

    void SelectArrows_2()
    {
        StopAllCoroutines();
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
                stepArrows[1].SetActive(true);
                var renderers_0 = stepArrows[0].GetComponentsInChildren<Renderer>();
                var renderers_1 = stepArrows[1].GetComponentsInChildren<Renderer>();
                var renderersList_0_1 = renderers_0.Concat(renderers_1).ToArray();
                allArrowRenders.AddRange(renderersList_0_1);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderersList_0_1), 6f));
                break;
            case 1:
                m_CurrentStepIndex = 1;
                stepArrows[0].SetActive(false);
                stepArrows[1].SetActive(false);
                stepArrows[2].SetActive(true);
                var renderers_2 = stepArrows[2].GetComponentsInChildren<Renderer>();
                allArrowRenders.AddRange(renderers_2);
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderers_2), 3f));
                break;
            default:
                break;
        }
    }


    public IEnumerator FadeOutObjects(List<Renderer> renderers, float duration)
    {
        yield return new WaitForSeconds(1);
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
        while (t < duration)
        {
            t += Time.deltaTime;
            float normalized = t / duration;

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

