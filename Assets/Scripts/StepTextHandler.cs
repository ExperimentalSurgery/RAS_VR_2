using TMPro;
using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Reflection.Emit;

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
                break;
            case 1:
                foreach (var arrow in stepArrows)
                {
                    arrow.SetActive(false);
                }
                stepArrows[1].SetActive(true);
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
}

