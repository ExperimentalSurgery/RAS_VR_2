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
    public bool isRightStylusUsed = false;
    public bool isLeftStylusUsed = false;
    bool isCallibrated = false;

    private void OnEnable()
    {
        ParentConstraintHandler.onCallibrated += ToggleDeviceBodyHandler;
    }

    void ToggleDeviceBodyHandler(bool isCallibrated)
    {
        this.isCallibrated = isCallibrated;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!isCallibrated) { return; }
        if (device == Device.RightDevice && other.gameObject.tag == "HapticCollider_Right")
        {
            ToggleDevices(true, true);

            if(!isLeftStylusUsed)
            {
                ToggleBody(false);
                platform.SetActive(true);
            }
         
        }
        else if (device == Device.LeftDevice && other.gameObject.tag == "HapticCollider_Left")
        {
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
        if (!isCallibrated) { return; }
        if (device == Device.RightDevice && other.gameObject.tag == "HapticCollider_Right")
        {
            ToggleDevices(false, true);
            if (!isLeftStylusUsed)
            {
                ToggleBody(true);
                platform.SetActive(false);
            }
        }
        else if (device == Device.LeftDevice && other.gameObject.tag == "HapticCollider_Left")
        {
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
                isRightStylusUsed = true;
            }
            else
            {
                leftTouchDevice.gameObject.SetActive(true);
                isLeftStylusUsed = true;
            }
              
            }
            else
            {
            if (isRightDevice)
            {
                rightTouchDevice.gameObject.SetActive(false);
                isRightStylusUsed = false;
            }
            else
            {
                leftTouchDevice.gameObject.SetActive(false);
                isLeftStylusUsed = false;
            }
        }

    }

    //void ToggleDevices(bool toActivate)
    //{
    //    foreach (var device in touchDevices)
    //    {
    //        if (toActivate)
    //        {
    //            device.gameObject.SetActive(true);
    //        }
    //        else
    //        {
    //            device.gameObject.SetActive(false);
    //        }
    //    }

    //}

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

