using TMPro;
using UnityEngine;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StepTextHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textLabelObject;
    [SerializeField] TextMeshProUGUI textBodyObject;
    [SerializeField] string[] stepLabelTexts;
    //[SerializeField] string[] stepBodyTexts;
    [SerializeField] TMP_Text nextButton;
    int m_CurrentStepIndex = 0;
    [SerializeField] private bool toLoadNextScene = false;

    private void Start()
    {
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

        m_CurrentStepIndex = (m_CurrentStepIndex + 1) % stepLabelTexts.Length;
    }


    void LoadNextScene()
    {
        // Load the next scene here
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
        Debug.Log("Load next scene");
    }
}

