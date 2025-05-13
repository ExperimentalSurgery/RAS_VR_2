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
    [SerializeField] GameObject rightTouchDevice;
    [SerializeField] GameObject leftTouchDevice;
    [SerializeField] GameObject platform;
    [SerializeField] Device device;
    public static bool isRightStylusUsed = false;
    public static bool isLeftStylusUsed = false;
    bool isCallibrated = false;
    int fadeValue = 0;
    private void OnEnable()
    {
        ParentConstraintHandler.onCallibrated += ToggleDeviceBodyHandler;

        foreach (var bodyMat in bodyMaterials)
        {
            StopCoroutine(StartFadingIn());
            bodyMat.sharedMaterial.SetFloat("_FadeStart", 0);
            bodyMat.sharedMaterial.SetFloat("_FadeSize", 0);
        }
    }


    private void OnDisable()
    {
        foreach (var bodyMat in bodyMaterials)
        {
            StopCoroutine(StartFadingIn());
            bodyMat.sharedMaterial.SetFloat("_FadeStart", 0);
            bodyMat.sharedMaterial.SetFloat("_FadeSize", 0);
        }
    }

    void ToggleDeviceBodyHandler(bool isCallibrated)
    {
        this.isCallibrated = isCallibrated;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isCallibrated)
        {
            foreach (var bodyMat in bodyMaterials)
            {
                StopCoroutine(StartFadingIn());
                bodyMat.sharedMaterial.SetFloat("_FadeStart", 0);
                bodyMat.sharedMaterial.SetFloat("_FadeSize", 0);
            }
            return; 
        }
        if (device == Device.RightDevice && other.gameObject.tag == "HapticCollider_Right" && isRightStylusUsed)
        {
            isRightStylusUsed = false;
            ToggleDevices(true, true);

            if(!isLeftStylusUsed)
            {
                ToggleBody(false);
                platform.SetActive(true);
            }
         
        }
        else if (device == Device.LeftDevice && other.gameObject.tag == "HapticCollider_Left" && isLeftStylusUsed)
        {
            isLeftStylusUsed = false;
            ToggleDevices(true, false);

            if (!isRightStylusUsed)
            {
                ToggleBody(false);
                platform.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isCallibrated)
        {
            foreach (var bodyMat in bodyMaterials)
            {
                StopCoroutine(StartFadingIn());
                bodyMat.sharedMaterial.SetFloat("_FadeStart", 0);
                bodyMat.sharedMaterial.SetFloat("_FadeSize", 0);
            }
            return;
        }
        if (device == Device.RightDevice && other.gameObject.tag == "HapticCollider_Right" && !isRightStylusUsed)
        {
            isRightStylusUsed = true;
            ToggleDevices(false, true);
            if (!isLeftStylusUsed)
            {
                ToggleBody(true);
                platform.SetActive(false);
            }
        }
        else if (device == Device.LeftDevice && other.gameObject.tag == "HapticCollider_Left" && !isLeftStylusUsed)
        {
            isLeftStylusUsed = true;
            ToggleDevices(false, false);
            if (!isRightStylusUsed)
            {
                ToggleBody(true);
                platform.SetActive(false);
            }    
        }
    }

    void ToggleDevices(bool toActivate, bool isRightDevice)
    {     
            if (toActivate)
            {
            if (isRightDevice)
            {
                rightTouchDevice.gameObject.SetActive(true);
                Debug.Log("isRightStylusUsed: " + isRightStylusUsed);
            }
            else
            {
                leftTouchDevice.gameObject.SetActive(true);
                Debug.Log("isLeftStylusUsed: " + isLeftStylusUsed);
            }
              
            }
            else
            {
            if (isRightDevice)
            {
                rightTouchDevice.gameObject.SetActive(false);
                Debug.Log("isRightStylusUsed: " + isRightStylusUsed);
            }
            else
            {
                leftTouchDevice.gameObject.SetActive(false);
                Debug.Log("isLeftStylusUsed: " + isLeftStylusUsed);
            }
        }

    }


    void ToggleBody(bool toActivate)
    {
        fadeValue = 0;
        foreach (var bodyMat in bodyMaterials)
        {
            if (toActivate)
            {
                StartCoroutine(StartFadingIn());

            }
            else 
            {
                StopCoroutine(StartFadingIn());
                bodyMat.sharedMaterial.SetFloat("_FadeStart", 0); 
                bodyMat.sharedMaterial.SetFloat("_FadeSize", 0);
            }
        }

    }

    IEnumerator StartFadingIn()
    {
        foreach (var bodyMat in bodyMaterials)
        {
            bodyMat.sharedMaterial.SetFloat("_FadeSize", 1);
        }
        while (fadeValue < 8)
        {
            yield return new WaitForSeconds(0.05f);
            fadeValue++;
            foreach (var bodyMat in bodyMaterials)
            {
                bodyMat.sharedMaterial.SetFloat("_FadeStart", fadeValue);
            }
        }
    }
}

