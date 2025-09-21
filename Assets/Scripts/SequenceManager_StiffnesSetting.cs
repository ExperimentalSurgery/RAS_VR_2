using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SequenceManager_StiffnesSetting : MonoBehaviour
{
    [SerializeField] private Image imageForFading;
    [SerializeField] private readonly float loadingSpeed = 1;
    [SerializeField] private readonly float fadeSpeed = 1;

    private void Start()
    {
    }

    IEnumerator StartLoading()
    {
        yield return new WaitForSeconds(loadingSpeed);
        StartCoroutine(FadeOut(fadeSpeed));
    }

    public IEnumerator FadeOut(float duration)
    {
        if (imageForFading == null) yield break;
        Color color = imageForFading.color;
        float startAlpha = color.a;
        float time = 0f;
        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, 0f, time / duration);
            imageForFading.color = new Color(color.r, color.g, color.b, alpha);
            time += Time.deltaTime;
            Debug.Log(time + " / " + duration);
            yield return null;
        }
    }

    public IEnumerator FadeIn(float duration)
    {
        if (imageForFading == null) yield break;
        Color color = imageForFading.color;
        float startAlpha = color.a;
        float time = 0f;
        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, 1f, time / duration);
            imageForFading.color = new Color(color.r, color.g, color.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }
        imageForFading.color = new Color(color.r, color.g, color.b, 1f);
    }
}
