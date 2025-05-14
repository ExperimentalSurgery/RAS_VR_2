using Oculus.Interaction.OVR;
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
    [SerializeField] OVRControllerInHandActiveState OVRControllerInHandActiveState_right;
    [SerializeField] OVRControllerInHandActiveState OVRControllerInHandActiveState_left;
    public static bool isRightStylusUsed = false;
    public static bool isLeftStylusUsed = false;
    bool isCalibrated = false;
    int fadeValue = 0;
    private void OnEnable()
    {
        ParentConstraintHandler.onCalibrated += ToggleDeviceBodyHandler;

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

    void ToggleDeviceBodyHandler(bool isCalibrated)
    {
        this.isCalibrated = isCalibrated;
        Debug.Log("isCalibrated: " + isCalibrated);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isCalibrated)
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
            OVRControllerInHandActiveState_right.ShowState = OVRInput.InputDeviceShowState.ControllerInHandOrNoHand;
            isRightStylusUsed = false;
            ToggleDevices(true, true);

            if (!isLeftStylusUsed)
            {
                ToggleBody(false);
                platform.SetActive(true);
            }

        }
        else if (device == Device.LeftDevice && other.gameObject.tag == "HapticCollider_Left" && isLeftStylusUsed)
        {
            OVRControllerInHandActiveState_left.ShowState = OVRInput.InputDeviceShowState.ControllerInHandOrNoHand;
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
        if (!isCalibrated)
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
            OVRControllerInHandActiveState_right.ShowState = OVRInput.InputDeviceShowState.NoHand;
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
            OVRControllerInHandActiveState_left.ShowState = OVRInput.InputDeviceShowState.NoHand;
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

