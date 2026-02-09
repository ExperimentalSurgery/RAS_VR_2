using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Passthrough mode settings")]
    [SerializeField] private OVRPassthroughLayer oVRPassthroughLayer;

    private void OnEnable()
    {
        oVRPassthroughLayer.passthroughLayerResumed.AddListener(OnPassthroughLayerResumed);
    }

    private void OnDisable()
    {
        oVRPassthroughLayer.passthroughLayerResumed.RemoveListener(OnPassthroughLayerResumed);
    }

    private void OnPassthroughLayerResumed(OVRPassthroughLayer passthroughLayer)
    {
        oVRPassthroughLayer.enabled = true;
        oVRPassthroughLayer.textureOpacity = 1f;
    }
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
