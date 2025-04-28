using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;


public enum Device
{
    RightDevice,
    LeftDevice
}
public class DevicesBodyHandler : MonoBehaviour
{
    [SerializeField] Renderer[] bodyMaterials;
    [SerializeField] GameObject[] touchDevices;
    [SerializeField] Device device;

    private void OnTriggerEnter(Collider other)
    {
        if (device == Device.RightDevice && other.gameObject.tag == "HapticCollider_Right")
        {
            ToggleDevices(true);
            ToggleBody(false);
        }
        else if (device == Device.LeftDevice && other.gameObject.tag == "HapticCollider_Left")
        {
            ToggleDevices(true);
            ToggleBody(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (device == Device.RightDevice && other.gameObject.tag == "HapticCollider_Right")
        {
            ToggleDevices(false);
            ToggleBody(true);
        }
        else if (device == Device.LeftDevice && other.gameObject.tag == "HapticCollider_Left")
        {
            ToggleDevices(false);
            ToggleBody(true);
        }
    }

    void ToggleDevices(bool toActivate)
    {
        foreach (var device in touchDevices)
        {
            if (toActivate)
            {
                device.gameObject.SetActive(true);
            }
            else
            {
                device.gameObject.SetActive(false);
            }
        }

    }

    void ToggleBody(bool toActivate)
    {
        foreach (var bodyMat in bodyMaterials)
        {
            if (toActivate)
            {
                StartCoroutine(StartFadingIn());

            }
            else
            {
                bodyMat.sharedMaterial.SetFloat("_FadeStart", 0); 
                bodyMat.sharedMaterial.SetFloat("_FadeSize", 0);
            }
        }

    }

    IEnumerator StartFadingIn()
    {
        int fadeValue = 0;
        foreach (var bodyMat in bodyMaterials)
        {
            bodyMat.sharedMaterial.SetFloat("_FadeSize", 1);
        }
        while (fadeValue < 10)
        {
            yield return new WaitForSeconds(0.05f);
            fadeValue++;
            foreach (var bodyMat in bodyMaterials)
            {
                bodyMat.sharedMaterial.SetFloat("_FadeStart", fadeValue);
            }
        }

        fadeValue = 0;
    }
}

