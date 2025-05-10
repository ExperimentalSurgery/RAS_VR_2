using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class HapticDevicesHandler : MonoBehaviour
{
    [SerializeField] Transform[] objectsToRotate;
    [SerializeField] Transform[] rotations;
    [SerializeField] GameObject[] HapticDevices;
    [SerializeField] GameObject[] stylusMeshes;
    [SerializeField] HapticPlugin[] hapticPlugins;
    [SerializeField] Image imageForFading;
    [SerializeField] float loadingSpeed = 1;
    [SerializeField] float fadeSpeed = 1;
    [SerializeField] GameObject[] sceneObjects;

    private void OnEnable()
    {
       // ParentConstraintHandler.onCallibrated += AssignRotation;
    }
    private void Start()
    {
       
        imageForFading.gameObject.SetActive(true);
        StartCoroutine(StartLoading());
    }

    void AssignRotation(bool isCallibrated)
    {
        if (isCallibrated)
        {
            for (int i = 0; i < objectsToRotate.Length; i++)
            {
                if (objectsToRotate[i] != null)
                objectsToRotate[i].rotation = rotations[i].rotation;
            }
        }   
    }

    public void ActivateDevices()
    {
        StartCoroutine(ActivationTimer(0f));
    }

    public void DeactivateDevices()
    {
        StartCoroutine(DeactivationTimer(0));
    }

    IEnumerator DeactivationTimer(float sek)
    {
        ToggleSceneObjects(false);
        ToggleHapticPlugins(false);
        ToggleHapticDevices(false);
        yield return new WaitForSeconds(sek);
    }

    IEnumerator ActivationTimer(float sek)
    {
        ToggleHapticDevices(true);
        yield return new WaitForSeconds(sek);
        ToggleHapticPlugins(true);
        InitializeHapticDevices();
        //ToggleSceneObjects(true);
    }
    void ToggleHapticDevices(bool toActivate)
    {
        foreach (var device in HapticDevices)
        {
            if (toActivate)
            {
                device.SetActive(true);
            }
            else
            {
                device.SetActive(false);
            }
        }

       foreach (var stylus in stylusMeshes) {
            if (toActivate)
            {
                stylus.SetActive(true);
            }
            else
            {
                stylus.SetActive(false);
            }
        }
    }

    void ToggleSceneObjects(bool toActivate)
    {
        foreach (var sceneObject in sceneObjects)
        {
            if (toActivate)
            {
                sceneObject.SetActive(true);
            }
            else
            {
                sceneObject.SetActive(false);
            }
        }
    }

    void ToggleHapticPlugins(bool toActivate)
    {
        foreach (var hapticPlugin in hapticPlugins)
        {
            if (toActivate)
            {
                hapticPlugin.enabled = true;
            }
            else
            {
                hapticPlugin.enabled = false;
            }
        }
    }

    void InitializeHapticDevices()
    {
        foreach (var hapticPlugin in hapticPlugins)
        {
            if (!hapticPlugin.InitializeHapticDevice())
            {
                hapticPlugin.InitializeHapticDevice();
            }
          
        }
    }

    public void InitializeHapticDevices(HapticPlugin hapticPlugin)
    {
        
         hapticPlugin.InitializeHapticDevice();
    }

    IEnumerator StartLoading() {
        yield return new WaitForSeconds(loadingSpeed);
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut() { 
        Color imageColor = imageForFading.color;
    while (imageForFading.color.a > 0)
        {
            float fadeAmount = imageColor .a - (fadeSpeed * Time.deltaTime);
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, fadeAmount);
            imageForFading.color = imageColor;  
            yield return null;
        }
        yield return null;
    }

    private void OnDisable()
    {
        foreach (var hapticPlugin in hapticPlugins)
        {                hapticPlugin.UpdateDeviceInformation();
            Debug.Log("hapticPlugin.UpdateDeviceInformation()");
        }
    }
}
