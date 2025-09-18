using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadScene(int sceneIndex)
    {

        SceneManager.LoadScene(sceneIndex);
        Debug.Log("Load next scene");
    }

    public void EndSimulation()
    {
        Application.Quit();
    }
}
