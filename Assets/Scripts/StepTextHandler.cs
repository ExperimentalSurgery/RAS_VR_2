using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using TMPro;
using TMPro;
using UnityEngine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StepTextHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textLabelObject;
    [SerializeField] TextMeshProUGUI textBodyObject;
    [SerializeField] string[] stepLabelTexts;
    [SerializeField] GameObject[] stepArrows;
    //[SerializeField] string[] stepBodyTexts;
    [SerializeField] TMP_Text nextButton;
    int m_CurrentStepIndex = 0;
    [SerializeField] private bool toLoadNextScene = false;

    private void Start()
    {
        foreach (var arrow in stepArrows)
        {
            arrow.SetActive(false);
        }
        NextSetting();
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

        //label
        textLabelObject.text = stepLabelTexts[m_CurrentStepIndex];
        // body
        //textBodyObject.text = stepBodyTexts[m_CurrentStepIndex];
        SelectArrows();

        m_CurrentStepIndex = (m_CurrentStepIndex + 1) % stepLabelTexts.Length;
    }


    void LoadNextScene()
    {
        // Load the next scene here
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        Debug.Log("Load next scene");
    }

    void SelectArrows()
    {
        switch (m_CurrentStepIndex)
        {
            case 0:
                foreach (var arrow in stepArrows)
                {
                    arrow.SetActive(false);
                }
                stepArrows[0].SetActive(true);
                var renderers_0 = stepArrows[0].GetComponentsInChildren<Renderer>();
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderers_0), 1.5f));
                break;
            case 1:
                foreach (var arrow in stepArrows)
                {
                    arrow.SetActive(false);
                }
                stepArrows[1].SetActive(true);
                var renderers_1 = stepArrows[1].GetComponentsInChildren<Renderer>();
                StartCoroutine(FadeOutObjects(new List<Renderer>(renderers_1), 1.5f));
                break;
            case 2:
                foreach (var arrow in stepArrows)
                {
                    arrow.SetActive(false);
                }
                stepArrows[2].SetActive(true);
                break;
            case 3:
                foreach (var arrow in stepArrows)
                {
                    arrow.SetActive(false);
                }
                stepArrows[3].SetActive(true);
                stepArrows[4].SetActive(true);
                break;
            case 4:
                foreach (var arrow in stepArrows)
                {
                    arrow.SetActive(false);
                }
                stepArrows[5].SetActive(true);
                break;
            case 5:
                foreach (var arrow in stepArrows)
                {
                    arrow.SetActive(false);
                }
                stepArrows[6].SetActive(true);
                stepArrows[7].SetActive(true);
                break;
            case 6:
                foreach (var arrow in stepArrows)
                {
                    arrow.SetActive(false);
                }
                stepArrows[8].SetActive(true);
                break;
            default:
                break;
        }
    }

    //public IEnumerator FadeOutMaterial(Material mat, float duration)
    //{
    //    // Aktuellen Farbwert speichern
    //    Color startColor = mat.color;
    //    Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

    //    float t = 0f;

    //    while (t < duration)
    //    {
    //        t += Time.deltaTime;
    //        float normalized = t / duration;

    //        // Alpha interpolieren
    //        mat.color = Color.Lerp(startColor, endColor, normalized);

    //        yield return null;
    //    }

    //    // Sicherheit: Alpha am Ende auf 0 setzen
    //    mat.color = endColor;
    //}

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
}

